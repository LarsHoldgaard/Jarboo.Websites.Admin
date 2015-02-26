using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using File = Google.Apis.Drive.v2.Data.File;

namespace Jarboo.Admin.Integration.GoogleDrive
{
    public class GoogleDriveFolderCreator : IFolderCreator
    {
        public const string FOLDER_MIME_TYPE = "application/vnd.google-apps.folder";
        public const string DOC_MIME_TYPE = "application/vnd.google-apps.document";

        private readonly Setting _setting;
        private DriveService _driveService;


        public GoogleDriveFolderCreator(ISettingService settingService)
        {
            _setting = settingService.GetCurrentSetting();

            EnsureService();
        }

        private void EnsureService()
        {
            if (!_setting.UseGoogleDrive)
            {
                return;
            }

            if (_driveService != null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_setting.GoogleClientId) || string.IsNullOrEmpty(_setting.GoogleClientSecret) ||
                 string.IsNullOrEmpty(_setting.GoogleRefreshToken))
            {
                throw new ApplicationException("Missing google drive configuration");
            }

            try
            {
                var initializer = new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _setting.GoogleClientId,
                        ClientSecret = _setting.GoogleClientSecret
                    }
                };
                var flow = new AuthorizationCodeFlow(initializer);
                //flow.RefreshTokenAsync("user", Configuration.GoogleRefreshToken, new CancellationTokenSource().Token);

                var tokenResponse = new TokenResponse { RefreshToken = _setting.GoogleRefreshToken };
                var userCredential = new UserCredential(flow, _setting.GoogleLocalUserId, tokenResponse);

                _driveService = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = "Jarboo.Admin"
                });
            }
            catch (Exception ex)
            {
                _driveService = null;

                throw;
            }
        }

        public string Create(string customerName, string taskIdentifier)
        {
            if (!_setting.UseGoogleDrive)
            {
                return null;
            }


            var driveFolders = LoadGoogleDriveFolderHierarchy();

            var newFolder = CreateFolders(CreateFolderPath(customerName, taskIdentifier), driveFolders);
            CopyTemplate(taskIdentifier, driveFolders, newFolder);

            return newFolder.File.AlternateLink;
        }
        public void Delete(string customerName, string taskIdentifier)
        {
            if (!_setting.UseGoogleDrive)
            {
                return;
            }

            DeleteFolder(CreateFolderPath(customerName, taskIdentifier));
        }

        private string[] CreateFolderPath(string customerName, string taskIdentifier)
        {
            var date = DateTime.Now;
            return Path.Combine(_setting.GoogleBasePath,
                customerName,
                date.Year.ToString(CultureInfo.CurrentCulture),
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month),
                taskIdentifier).Split('\\');
        }

        private FolderHierarchy LoadGoogleDriveFolderHierarchy()
        {
            var files = _driveService.Files.List();
            files.MaxResults = int.MaxValue;
            var driveFiles = files.Execute();

            var about = _driveService.About.Get().Execute();

            return new FolderHierarchy(about.RootFolderId, driveFiles);
        }

        private FolderHierarchy.Folder CreateFolders(IEnumerable<string> folders, FolderHierarchy driveFolders)
        {
            var parentFolder = driveFolders.Root;
            foreach (var folderName in folders)
            {
                var driveFolder = parentFolder.FindFolder(folderName);
                if (driveFolder != null)
                {
                    parentFolder = driveFolder;
                    continue;
                }

                var driveFile = GetExistsFolder(folderName, parentFolder.Id);
                if (driveFile != null)
                {
                    parentFolder = new FolderHierarchy.Folder(driveFile);

                }
            }

            return parentFolder;
        }

        private File CreateFolder(string title, string parentId)
        {
            return CreateFile(title, parentId, FOLDER_MIME_TYPE);
        }

        private File CreateDoc(string title, string parentId)
        {
            return CreateFile(title, parentId, DOC_MIME_TYPE);
        }
 
        private File CreateFile(string title, string parentId, string mimeType)
        {
            var body = new File
            {
                Title = title,
                MimeType = mimeType,
                Parents = new[]
                              {
                                  new ParentReference
                                  {
                                          Id = parentId
                                      }
                              }
            };

            var request = _driveService.Files.Insert(body);
            var file = request.Execute();
            if (file == null)
            {
                throw new ApplicationException("Couldn't create google drive file/folder");
            }

            return file;
        }

        private File GetExistsFolder(string title, string parentId)
        {
            string query = "mimeType='" + FOLDER_MIME_TYPE + "' AND trashed=false AND title='" + title + "' AND '" + parentId + "' in parents";
            FilesResource.ListRequest list = _driveService.Files.List();
            list.MaxResults = int.MaxValue;
            list.Q = query;
            FileList files = list.Execute();

            if (files.Items.Any())
            {
                return null;
            }

            return CreateFolder(title, parentId);
        }

        private File CopyFile(string newFileName, File file, string parentId)
        {
            var body = new File
            {
                Title = newFileName,
                MimeType = file.MimeType,
                Parents = new[]
                              {
                                  new ParentReference
                                  {
                                          Id = parentId
                                      }
                              }
            };

            var request = _driveService.Files.Copy(body, file.Id);
            var copy = request.Execute();
            if (copy == null)
            {
                throw new ApplicationException("Couldn't copy google drive file");
            }

            return copy;
        }

        private void CopyTemplate(string newFileName, FolderHierarchy driveFolders, FolderHierarchy.Folder newFolder)
        {
            if (string.IsNullOrEmpty(_setting.GoogleTemplatePath))
            {
                return;
            }

            var path = _setting.GoogleTemplatePath.Split('\\');
            var fileName = path[path.Length - 1];

            var parentFolder = driveFolders.Root;
            foreach (var folderName in path.Take(path.Length - 1))
            {
                var driveFolder = parentFolder.FindFolder(folderName);
                if (driveFolder == null)
                {
                    throw new ApplicationException("Couldn't find template to copy. Folder '" + folderName + "' is missing");
                }

                parentFolder = driveFolder;
            }

            var templateFile = parentFolder.FindFile(fileName);
            if (templateFile == null)
            {
                throw new ApplicationException("Couldn't find template to copy. File is missing");
            }

            CopyFile(newFileName, templateFile, newFolder.Id);
        }

        private void DeleteFolder(IEnumerable<string> folders)
        {
            var driveFolders = LoadGoogleDriveFolderHierarchy();

            var parentFolder = driveFolders.Root;
            foreach (var folderName in folders)
            {
                var driveFolder = parentFolder.FindFolder(folderName);
                if (driveFolder == null)
                {
                    return;
                }

                parentFolder = driveFolder;
            }

            _driveService.Files.Delete(parentFolder.Id).Execute();
        }
    }
}
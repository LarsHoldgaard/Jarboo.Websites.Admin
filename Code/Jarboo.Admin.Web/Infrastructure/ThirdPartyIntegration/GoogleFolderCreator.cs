using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;

using Jarboo.Admin.BL.Other;

using File = Google.Apis.Drive.v2.Data.File;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class GoogleFolderCreator : IFolderCreator
    {
        public const string FOLDER_MIME_TYPE = "application/vnd.google-apps.folder";
        public const string DOC_MIME_TYPE = "application/vnd.google-apps.document";

        private DriveService driveService = null;

        private void EnsureService()
        {
            if (this.driveService != null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Configuration.GoogleClientId) || string.IsNullOrEmpty(Configuration.GoogleClientSecret) ||
                 string.IsNullOrEmpty(Configuration.GoogleRefreshToken))
            {
                throw new ApplicationException("Missing google drive configuration");
            }

            try
            {
                var initializer = new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets()
                    {
                        ClientId = Configuration.GoogleClientId,
                        ClientSecret = Configuration.GoogleClientSecret
                    }
                };
                var flow = new AuthorizationCodeFlow(initializer);
                //flow.RefreshTokenAsync("user", Configuration.GoogleRefreshToken, new CancellationTokenSource().Token);

                var tokenResponse = new TokenResponse { RefreshToken = Configuration.GoogleRefreshToken };
                var userCredential = new UserCredential(flow, AppFlowMetadata.UserId, tokenResponse);

                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = "Jarboo.Admin",
                });
            }
            catch (Exception ex)
            {
                this.driveService = null;

                throw;
            }
        }

        public string Create(string customerName, string taskTitle)
        {
            EnsureService();

            var driveFolders = this.LoadGoogleDriveFolderHierarchy();

            var newFolder = CreateFolders(CreateFolderPath(customerName, taskTitle), driveFolders);
            CopyTemplate(taskTitle, driveFolders, newFolder);

            return newFolder.File.AlternateLink;
        }
        public void Delete(string customerName, string taskTitle)
        {
            EnsureService();

            DeleteFolder(CreateFolderPath(customerName, taskTitle));
        }

        private string[] CreateFolderPath(string customerName, string taskTitle)
        {
            var date = DateTime.Now;
            return Path.Combine(Configuration.GoogleDrivePath,
                customerName,
                date.Year.ToString(CultureInfo.CurrentCulture),
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month),
                taskTitle).Split('\\');
        }
        private GoogleDriveFolderHierarchy LoadGoogleDriveFolderHierarchy()
        {
            var files = this.driveService.Files.List();
            files.MaxResults = int.MaxValue;
            var driveFiles = files.Execute();
            return new GoogleDriveFolderHierarchy(driveFiles);
        }

        private GoogleDriveFolderHierarchy.Folder CreateFolders(IEnumerable<string> folders, GoogleDriveFolderHierarchy driveFolders)
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

                var driveFile = this.CreateFolder(folderName, parentFolder.Id);
                parentFolder = new GoogleDriveFolderHierarchy.Folder(driveFile);
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
                Parents = new ParentReference[]
                              {
                                  new ParentReference()
                                      {
                                          Id = parentId
                                      }
                              }
            };

            var request = driveService.Files.Insert(body);
            var file = request.Execute();
            if (file == null)
            {
                throw new ApplicationException("Couldn't create google drive file/folder");
            }

            return file;
        }
        private File CopyFile(string newFileName, File file, string parentId)
        {
            var body = new File
            {
                Title = newFileName,
                MimeType = file.MimeType,
                Parents = new ParentReference[]
                              {
                                  new ParentReference()
                                      {
                                          Id = parentId
                                      }
                              }
            };

            var request = driveService.Files.Copy(body, file.Id);
            var copy = request.Execute();
            if (copy == null)
            {
                throw new ApplicationException("Couldn't copy google drive file");
            }

            return copy;
        }

        private void CopyTemplate(string newFileName, GoogleDriveFolderHierarchy driveFolders, GoogleDriveFolderHierarchy.Folder newFolder)
        {
            if (string.IsNullOrEmpty(Configuration.GoogleDriveTemplatePath))
            {
                return;
            }

            var path = Configuration.GoogleDriveTemplatePath.Split('\\');
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

            this.CopyFile(newFileName, templateFile, newFolder.Id);
        }

        private void DeleteFolder(IEnumerable<string> folders)
        {
            var driveFolders = this.LoadGoogleDriveFolderHierarchy();

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

            driveService.Files.Delete(parentFolder.Id).Execute();
        }
    }
}
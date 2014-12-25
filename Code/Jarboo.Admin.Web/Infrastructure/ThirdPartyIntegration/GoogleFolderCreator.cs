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

using Jarboo.Admin.BL.ThirdParty;

using File = Google.Apis.Drive.v2.Data.File;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class GoogleFolderCreator : IFolderCreator
    {
        public const string FOLDER_MIME_TYPE = "application/vnd.google-apps.folder";
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

        public void Create(string customerName, string taskTitle)
        {
            EnsureService();

            CreateFolders(CreateFolderPath(customerName, taskTitle));
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
            var driveFiles = this.driveService.Files.List().Execute();
            return new GoogleDriveFolderHierarchy(driveFiles);
        }

        private void CreateFolders(IEnumerable<string> folders)
        {
            EnsureService();

            var driveFolders = this.LoadGoogleDriveFolderHierarchy();

            var parentFolder = driveFolders.Root;
            foreach (var folderName in folders)
            {
                var driveFolder = parentFolder.Find(folderName);
                if (driveFolder != null)
                {
                    parentFolder = driveFolder;
                    continue;
                }

                var driveFile = this.CreateFolder(folderName, parentFolder.Id);
                parentFolder = new GoogleDriveFolderHierarchy.Folder(driveFile);
            }
        }
        private File CreateFolder(string title, string parentId)
        {
            EnsureService();

            var body = new File
            {
                Title = title,
                MimeType = FOLDER_MIME_TYPE,
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
                throw new ApplicationException("Couldn't create google drive folder");
            }

            return file;
        }

        private void DeleteFolder(IEnumerable<string> folders)
        {
            EnsureService();

            var driveFolders = this.LoadGoogleDriveFolderHierarchy();

            var parentFolder = driveFolders.Root;
            foreach (var folderName in folders)
            {
                var driveFolder = parentFolder.Find(folderName);
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
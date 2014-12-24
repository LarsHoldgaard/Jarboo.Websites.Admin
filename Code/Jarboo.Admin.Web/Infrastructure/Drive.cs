using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Services;

using File = Google.Apis.Drive.v2.Data.File;

namespace GoogleDriveSamples
{
    public class DriveCommandLineSample
    {
        //DriveCommandLineSample.Auth(Server.MapPath(@"App_Data\GoogleProject.p12"));
        public static string Auth(string path)
        {
            /*X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.Subject.Contains("552485355365-th8p5oh0id55lvf1u30iscdfu9ofihso.apps.googleusercontent.com"))
                {
                    var csp = (RSACryptoServiceProvider)cert.PrivateKey;

                    var initializer =
                        new ServiceAccountCredential.Initializer(
                            "552485355365-th8p5oh0id55lvf1u30iscdfu9ofihso.apps.googleusercontent.com",
                            "https://www.googleapis.com/auth/drive");
                    initializer.Key = csp;
                    var cred = new ServiceAccountCredential(initializer);
                    var cts = new CancellationTokenSource();
                    cred.RequestAccessTokenAsync(cts.Token).Wait();
                    return cred.Token;
                }
            }*/


            const string serviceAccountEmail = "552485355365-th8p5oh0id55lvf1u30iscdfu9ofihso@developer.gserviceaccount.com";
            var certificate = new X509Certificate2(path, "notasecret", X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = new[] { DriveService.Scope.Drive }
               }.FromCertificate(certificate));

            var cts = new CancellationTokenSource();
            var r = credential.RequestAccessTokenAsync(cts.Token).Result;


            // Create the service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ace-world-803",
            });

            var body = new File();
            body.Title = "Hello";
            body.Description = "A test document";
            body.MimeType = "application/vnd.google-apps.folder";

            var request = service.Files.Insert(body);
            var file = request.Execute();
            return file.Id;
        }

        public static string Main()
        {
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "552485355365-8go9lgc1se02mvjgjlfpsnr286ncr2qa.apps.googleusercontent.com",
                    ClientSecret = "QHgTjyg82nCNzfmxI9IO4vBu",
                },
                new[] { DriveService.Scope.Drive },
                "user",
                CancellationToken.None).Result;

            // Create the service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Sample",
            });

            File body = new File();
            body.Title = "Hello";
            body.Description = "A test document";
            body.MimeType = "text/plain";

            var stream = GenerateStreamFromString("Hello!");

            FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "text/plain");
            request.Upload();

            File file = request.ResponseBody;
            return file.Id;
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}

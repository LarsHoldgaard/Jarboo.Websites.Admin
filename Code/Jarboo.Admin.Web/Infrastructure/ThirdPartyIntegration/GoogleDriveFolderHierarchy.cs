using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Google.Apis.Drive.v2.Data;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class GoogleDriveFolderHierarchy
    {
        public GoogleDriveFolderHierarchy(FileList files)
        {
            CreateRoot(files);
            CreateHierarchy(files);
        }

        private void CreateRoot(FileList files)
        {
            this.Root = new Folder();

            foreach (var file in files.Items)
            {
                foreach (var parent in file.Parents)
                {
                    if (parent.IsRoot == true)
                    {
                        Root.Id = parent.Id;
                    }
                }
            }
        }

        private void CreateHierarchy(FileList files)
        {
            var folders = new Dictionary<string, Folder>();
            folders[Root.Id] = Root;

            foreach (var file in files.Items)
            {
                if (file.MimeType != GoogleFolderCreator.FOLDER_MIME_TYPE || file.ExplicitlyTrashed == true)
                {
                    continue;
                }

                folders[file.Id] = new Folder(file);
            }

            foreach (var file in files.Items)
            {
                if (file.MimeType != GoogleFolderCreator.FOLDER_MIME_TYPE || file.ExplicitlyTrashed == true)
                {
                    continue;
                }

                foreach (var parent in file.Parents)
                {
                    if (!folders.ContainsKey(parent.Id))
                    {
                        continue;
                    }

                    folders[parent.Id].Children.Add(folders[file.Id]);
                }
            }
        }

        public Folder Root { get; set; }

        public class Folder
        {
            public Folder()
            {
                Children = new List<Folder>();
            }
            public Folder(File file) : this()
            {
                File = file;
                Id = file.Id;
                Title = file.Title;
            }

            public File File { get; set; }
            public string Id { get; set; }
            public string Title { get; set; }
            public List<Folder> Children { get; set; }

            public Folder Find(string title)
            {
                return Children.FirstOrDefault(x => x.Title == title);
            }
        }
    }
}
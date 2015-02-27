using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Drive.v2.Data;

namespace Jarboo.Admin.Integration.GoogleDrive
{
    public class FolderHierarchy
    {
        public FolderHierarchy(string rootFolderId, FileList files)
        {
            CreateRoot(rootFolderId);
            CreateHierarchy(files);
        }

        private void CreateRoot(string rootFolderId)
        {
            Root = new Folder
            {
                Id = rootFolderId
            };
        }

        private void CreateHierarchy(FileList files)
        {
            var folders = new Dictionary<string, Folder>();
            folders[Root.Id] = Root;

            foreach (var file in files.Items)
            {
                if (file.MimeType != GoogleDriveFolderCreator.FOLDER_MIME_TYPE || file.ExplicitlyTrashed == true)
                {
                    continue;
                }

                folders[file.Id] = new Folder(file);
            }

            foreach (var file in files.Items)
            {
                if (file.ExplicitlyTrashed == true)
                {
                    continue;
                }

                foreach (var parent in file.Parents)
                {
                    if (!folders.ContainsKey(parent.Id))
                    {
                        continue;
                    }

                    if (file.MimeType == GoogleDriveFolderCreator.FOLDER_MIME_TYPE)
                    {
                        folders[parent.Id].NestedFolders.Add(folders[file.Id]);
                    }
                    else
                    {
                        folders[parent.Id].Files.Add(file);
                    }
                }
            }
        }

        public Folder Root { get; set; }

        public class Folder
        {
            public Folder()
            {
                NestedFolders = new List<Folder>();
                Files = new List<File>();
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
            public List<Folder> NestedFolders { get; set; }
            public List<File> Files { get; set; }

            public Folder FindFolder(string title)
            {
                return NestedFolders.FirstOrDefault(x => x.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));
            }
            public File FindFile(string title)
            {
                return Files.FirstOrDefault(x => x.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));
            }
        }
    }
}
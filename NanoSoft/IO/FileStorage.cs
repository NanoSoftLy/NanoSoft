using NanoSoft.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NanoSoft.IO
{
    public class FileStorage : IFileService
    {
        public FileStorage(string savePath, string outputPath)
        {
            SavePath = savePath;
            OutputPath = outputPath;
        }

        public string SavePath { get; }
        public string OutputPath { get; }

        protected List<File> FilesToAdd { get; } = new List<File>();
        protected List<string> FilesToRemove { get; } = new List<string>();

        public void Add(File file)
        {
            FilesToAdd.Add(file);
        }

        public void AddRange(IEnumerable<File> files)
        {
            files.ForEach(f => Add(f));
        }

        public void Remove(string filePath)
        {
            FilesToRemove.Add(filePath);
        }
        public void RemoveRange(IEnumerable<string> filePaths)
        {
            filePaths.ForEach(p => Remove(p));
        }

        public Task CommitAsync()
        {
            try
            {
                foreach (var file in FilesToAdd)
                {
                    SaveFile(file);
                }

                foreach (var file in FilesToRemove)
                {
                    RemoveFile(file);
                }

                FilesToAdd.Clear();
                FilesToRemove.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }

        protected virtual void RemoveFile(string filePath)
        {
            System.IO.File.Delete(Path.Combine(SavePath, filePath));
        }

        protected virtual void SaveFile(File file)
        {
            Directory.CreateDirectory(Path.Combine(SavePath, file.Directory));

            var newPath = Path.Combine(SavePath, file.Directory, file.NewFileName);

            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                file.Stream.CopyTo(fileStream);
            }
        }
    }
}

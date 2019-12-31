using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.IO
{
    public interface IFileService
    {
        string SavePath { get; }
        string OutputPath { get; }
        void Add(File file);
        void AddRange(IEnumerable<File> files);
        void Remove(string filePath);
        void RemoveRange(IEnumerable<string> filePaths);
        Task CommitAsync();
    }
}

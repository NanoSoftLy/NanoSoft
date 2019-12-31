using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.IO
{
    public class MockFileService : IFileService
    {
        public string SavePath => "http://localhost:0000/";
        public string OutputPath => "http://localhost:0000/";
        public string BasePath => "http://localhost:0000/";

        public void Add(File file)
        {

        }

        public void AddRange(IEnumerable<File> files)
        {

        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public void Remove(string filePath)
        {

        }

        public void RemoveRange(IEnumerable<string> filePaths)
        {

        }
    }
}

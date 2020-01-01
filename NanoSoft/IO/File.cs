using System;
using System.IO;
using System.Linq;

namespace NanoSoft.IO
{
    public class File
    {
        private string _directory;
        public string Directory
        {
            get => string.IsNullOrWhiteSpace(_directory) ? "Default" : _directory;
            set => _directory = value;
        }
        public Stream Stream { get; set; }
        public string Path { get; set; }
        public string Extension => Path.Split('.').LastOrDefault()?.ToLower();

        private string _newFileName;
        public string NewFileName
        {
            get
            {
                if (_newFileName != null)
                    return _newFileName;

                _newFileName = $"ns-{Guid.NewGuid()}{System.IO.Path.GetExtension(Path)}";

                return _newFileName;
            }
        }

        public string NewFilePath => Directory + "/" + NewFileName;
    }
}
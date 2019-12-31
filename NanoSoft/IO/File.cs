using System;
using System.IO;
using System.Linq;

namespace NanoSoft.IO
{
    public class File
    {
        private string _folderId;
        public string FolderId
        {
            get => string.IsNullOrWhiteSpace(_folderId) ? "Default" : _folderId;
            set => _folderId = value;
        }
        public Stream Stream { get; set; }
        public string Path { get; set; }
        public string Extension => Path.Split('.').LastOrDefault()?.ToLower();

        private string _newFilePath;
        public string OutputPath
        {
            get
            {
                if (_newFilePath != null)
                    return _newFilePath;

                var newFile = $"ns-{Guid.NewGuid()}{System.IO.Path.GetExtension(Path)}";

                _newFilePath = FolderId + "/" + newFile;

                return _newFilePath;
            }
        }
    }
}
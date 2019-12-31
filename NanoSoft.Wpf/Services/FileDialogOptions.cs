using System.Collections.Generic;

namespace NanoSoft.Wpf.Services
{
    public class FileDialogOptions
    {
        public string DialogTitle { get; set; }
        public bool AcceptFiles { get; set; }
        public bool AcceptWordDocuments { get; set; }
        public bool AcceptPdfDocuments { get; set; }
        public bool AcceptExcelDocuments { get; set; }
        public ICollection<IO.File> Files { get; set; }
        public IO.File File { get; set; }
        public bool Multiple => Files != null;
    }
}
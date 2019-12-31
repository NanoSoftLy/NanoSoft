using Microsoft.Win32;
using NanoSoft.Wpf.Resources;
using System;
using System.Collections.Generic;
using System.Windows;

namespace NanoSoft.Wpf.Services
{
    public class FileManager
    {
        public static void Attach(FileDialogOptions options)
        {
            try
            {
                var filter = new List<string>();

                if (options.AcceptFiles)
                    filter.Add("Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png");

                if (options.AcceptPdfDocuments)
                    filter.Add("PDF Documents | *.pdf");

                if (options.AcceptWordDocuments)
                    filter.Add("Word Documents |*.doc; *.docx");

                if (options.AcceptExcelDocuments)
                    filter.Add("Excel Worksheets|*.xls; *.xlsx");

                var fileDialog = new OpenFileDialog
                {
                    Filter = string.Join(" | ", filter),
                    Title = options.DialogTitle ?? (options.Multiple
                            ? SharedPhrases.AttachFiles
                            : SharedPhrases.AttachFile),
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = options.Multiple
                };

                if (fileDialog.ShowDialog() != true)
                    return;

                if (options.Multiple)
                {
                    var streams = fileDialog.OpenFiles();

                    for (var i = 0; i < fileDialog.FileNames.Length; i++)
                    {
                        options.Files.Add(new IO.File()
                        {
                            Path = fileDialog.FileNames[i],
                            Stream = streams[i]
                        });
                    }

                    return;
                }

                options.File = new IO.File()
                {
                    Path = fileDialog.FileName,
                    Stream = fileDialog.OpenFile()
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
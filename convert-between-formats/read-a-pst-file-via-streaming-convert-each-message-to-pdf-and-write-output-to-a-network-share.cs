using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string outputDirectory = @"\\networkshare\pdfs";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine($"Output directory not found: {outputDirectory}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
    {
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    Aspose.Email.HtmlSaveOptions mhtmlOptions = new Aspose.Email.HtmlSaveOptions()
                    {
                        MailMessageSaveType = MailMessageSaveType.MHtmlFormat
                    };
                    mailMessage.Save(mhtmlStream, mhtmlOptions);
                    mhtmlStream.Position = 0;

                    Document doc = new Document(mhtmlStream);
                    string subject = string.IsNullOrWhiteSpace(messageInfo.Subject) ? "Untitled" : messageInfo.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }

                    string pdfFilePath = Path.Combine(outputDirectory, $"{subject}.pdf");
                    doc.Save(pdfFilePath, Aspose.Words.SaveFormat.Pdf);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert message '{messageInfo.Subject}': {ex.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDirectory);
        }
    }
}

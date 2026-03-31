using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputMsgPath = "sample.msg";

            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Directory.GetCurrentDirectory();
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // ---------- Save as EML ----------
                string emlPath = Path.Combine(outputDirectory, "output.eml");
                using (MailMessage mail = msg.ToMailMessage(new MailConversionOptions()))
                {
                    mail.Save(emlPath, SaveOptions.DefaultEml);
                }

                // ---------- Convert to PDF and DOCX via MHTML ----------
                string mhtmlPath = Path.Combine(outputDirectory, "temp.mhtml");
                msg.Save(mhtmlPath, SaveOptions.DefaultMhtml);

                Document doc = new Document(mhtmlPath);
            {
                    string pdfPath = Path.Combine(outputDirectory, "output.pdf");
                    doc.Save(pdfPath, SaveFormat.Pdf);

                    string docxPath = Path.Combine(outputDirectory, "output.docx");
                    doc.Save(docxPath, SaveFormat.Docx);
                }

                // Clean up temporary MHTML file
                try
                {
                    if (File.Exists(mhtmlPath))
                    {
                        File.Delete(mhtmlPath);
                    }
                }
                catch (Exception cleanupEx)
                {
                    Console.Error.WriteLine($"Warning: Could not delete temporary file – {cleanupEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

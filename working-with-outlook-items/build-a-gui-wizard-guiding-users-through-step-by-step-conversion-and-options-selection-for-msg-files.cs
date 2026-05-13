using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Prompt for MSG file path
            Console.Write("Enter the full path to the MSG file: ");
            string inputPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                Console.Error.WriteLine("The specified MSG file does not exist.");
                return;
            }

            // Prompt for output directory
            Console.Write("Enter the output directory: ");
            string outputDir = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(outputDir))
            {
                Console.Error.WriteLine("Invalid output directory.");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Choose conversion option
            Console.WriteLine("Select conversion option:");
            Console.WriteLine("1 - Convert to EML");
            Console.WriteLine("2 - Convert to HTML");
            Console.WriteLine("3 - Convert to PDF");
            Console.Write("Enter choice (1-3): ");
            string choiceInput = Console.ReadLine();
            if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > 3)
            {
                Console.Error.WriteLine("Invalid choice.");
                return;
            }

            // Load the MSG file as a MailMessage
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string baseFileName = Path.GetFileNameWithoutExtension(inputPath);
                switch (choice)
                {
                    case 1:
                        {
                            // Convert to EML
                            string emlPath = Path.Combine(outputDir, baseFileName + ".eml");
                            message.Save(emlPath, Aspose.Email.SaveOptions.DefaultEml);
                            Console.WriteLine($"EML file saved to: {emlPath}");
                            break;
                        }
                    case 2:
                        {
                            // Convert to HTML
                            string htmlPath = Path.Combine(outputDir, baseFileName + ".html");
                            Aspose.Email.HtmlSaveOptions htmlOptions = new Aspose.Email.HtmlSaveOptions();
                            message.Save(htmlPath, htmlOptions);
                            Console.WriteLine($"HTML file saved to: {htmlPath}");
                            break;
                        }
                    case 3:
                        {
                            // Convert to PDF via intermediate MHTML
                            string mhtmlPath = Path.Combine(outputDir, baseFileName + ".mhtml");
                            message.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);

                            Document doc = new Document(mhtmlPath);
            {
                                string pdfPath = Path.Combine(outputDir, baseFileName + ".pdf");
                                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                                doc.Save(pdfPath, pdfOptions);
                                Console.WriteLine($"PDF file saved to: {pdfPath}");
                            }
                            break;
                        }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;
using Aspose.Words;

namespace EmailMetadataPdfReport
{
    class Program
    {
        static void Main()
        {
            // This example extracts metadata from MSG files and creates a PDF report using Aspose.Email and Aspose.Words.

            string inputFolder = "InputMsgs";
            string outputPdfPath = "EmailReport.pdf";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPdfPath));
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {ex.Message}");
                    return;
                }
            }

            // Create a new Word document for the report
            Document reportDocument = new Document();
            DocumentBuilder builder = new DocumentBuilder(reportDocument);

            builder.Writeln("Email Metadata Report");
            builder.Writeln($"Generated on: {DateTime.Now}");
            builder.Writeln();

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating MSG files: {ex.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                try
                {
                    // Load the MSG file
                    MapiMessage msg = MapiMessage.Load(msgFilePath);

                    builder.Writeln($"File: {Path.GetFileName(msgFilePath)}");
                    builder.Writeln($"Subject: {msg.Subject}");
                    builder.Writeln($"From: {msg.SenderName}");
                    builder.Writeln($"Sent: {msg.ClientSubmitTime}");
                    builder.Writeln(); // Blank line between entries
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process '{msgFilePath}': {ex.Message}");
                    // Continue with next file
                }
            }

            // Save the report as PDF
            try
            {
                reportDocument.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                Console.WriteLine($"PDF report saved to '{outputPdfPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save PDF report: {ex.Message}");
            }
        }
    }
}

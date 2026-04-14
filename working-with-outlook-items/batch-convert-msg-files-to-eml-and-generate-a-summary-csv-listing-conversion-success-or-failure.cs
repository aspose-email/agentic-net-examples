using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirectory = "InputMsgs";
            string outputDirectory = "OutputEmls";
            string csvPath = "conversion_summary.csv";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                return;
            }

            List<string[]> csvRows = new List<string[]>();
            csvRows.Add(new string[] { "FileName", "Status", "Message" });

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Error: Unable to enumerate MSG files – {fileEx.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                string fileName = Path.GetFileName(msgFilePath);
                try
                {
                    // Load MSG file as MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                    {
                        // Convert to MailMessage
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                        {
                            // Save as EML
                            string emlFileName = Path.GetFileNameWithoutExtension(msgFilePath) + ".eml";
                            string emlFilePath = Path.Combine(outputDirectory, emlFileName);
                            mailMessage.Save(emlFilePath);
                        }
                    }

                    csvRows.Add(new string[] { fileName, "Success", "" });
                }
                catch (Exception ex)
                {
                    csvRows.Add(new string[] { fileName, "Failure", ex.Message });
                }
            }

            // Write CSV summary
            try
            {
                using (StreamWriter csvWriter = new StreamWriter(csvPath, false))
                {
                    foreach (string[] row in csvRows)
                    {
                        string line = string.Join(",", row);
                        csvWriter.WriteLine(line);
                    }
                }
            }
            catch (Exception csvEx)
            {
                Console.Error.WriteLine($"Error: Unable to write CSV summary – {csvEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            return;
        }
    }
}

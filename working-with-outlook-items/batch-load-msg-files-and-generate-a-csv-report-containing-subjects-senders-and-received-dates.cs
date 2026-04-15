using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input directory containing MSG files
            string inputDirectory = "InputMsgs";
            // Output CSV file path
            string outputCsvPath = "report.csv";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputCsvPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare CSV lines
            List<string> csvLines = new List<string>();
            csvLines.Add("Subject,Sender,ReceivedDate");

            // Get all MSG files in the input directory
            string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            foreach (string msgFilePath in msgFiles)
            {
                // Guard against missing files
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        string subject = message.Subject ?? string.Empty;
                        string sender = message.SenderEmailAddress ?? string.Empty;
                        DateTime deliveryTime = message.DeliveryTime;
                        string receivedDate = deliveryTime != DateTime.MinValue ? deliveryTime.ToString("o") : string.Empty;

                        // Escape double quotes for CSV
                        subject = subject.Replace("\"", "\"\"");
                        sender = sender.Replace("\"", "\"\"");
                        receivedDate = receivedDate.Replace("\"", "\"\"");

                        string csvLine = $"\"{subject}\",\"{sender}\",\"{receivedDate}\"";
                        csvLines.Add(csvLine);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process file {msgFilePath}: {ex.Message}");
                }
            }

            // Write CSV report
            try
            {
                using (StreamWriter writer = new StreamWriter(outputCsvPath, false))
                {
                    foreach (string line in csvLines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

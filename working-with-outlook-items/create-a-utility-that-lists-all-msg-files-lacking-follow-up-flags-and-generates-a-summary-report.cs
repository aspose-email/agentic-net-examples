using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "msg_files";
            string reportPath = "summary_report.csv";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input folder not found – {inputFolder}");
                return;
            }

            // Prepare the report file
            try
            {
                using (StreamWriter writer = new StreamWriter(reportPath, false))
                {
                    // Write CSV header
                    writer.WriteLine("FilePath,Subject,Sender,ReceivedDate");

                    // Enumerate all MSG files
                    string[] msgFiles;
                    try
                    {
                        msgFiles = Directory.GetFiles(inputFolder, "*.msg", SearchOption.AllDirectories);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error enumerating files: {ex.Message}");
                        return;
                    }

                    foreach (string filePath in msgFiles)
                    {
                        // Guard against missing files (should not happen after enumeration)
                        if (!File.Exists(filePath))
                        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"Warning: File not found – {filePath}");
                            continue;
                        }

                        try
                        {
                            using (MapiMessage message = MapiMessage.Load(filePath))
                            {
                                // Retrieve follow‑up options
                                FollowUpOptions options = FollowUpManager.GetOptions(message);

                                // Determine if a flag is set (FlagRequest is null or empty when no flag)
                                bool hasFlag = options != null && !string.IsNullOrEmpty(options.FlagRequest);

                                if (!hasFlag)
                                {
                                    string subject = message.Subject?.Replace("\"", "\"\"") ?? "";
                                    string sender = message.SenderEmailAddress?.Replace("\"", "\"\"") ?? "";
                                    string received = message.ClientSubmitTime != DateTime.MinValue
                                        ? message.ClientSubmitTime.ToString("o")
                                        : "";

                                    // Escape commas by surrounding with quotes
                                    writer.WriteLine($"\"{filePath}\",\"{subject}\",\"{sender}\",\"{received}\"");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                            // Continue with next file
                        }
                    }
                }

                Console.WriteLine($"Summary report generated at: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating report file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}

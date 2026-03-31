using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace EmailToMsgConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input files (can be extended as needed)
                List<string> inputFiles = new List<string>
                {
                    "sample.eml",
                    "sample.msg"
                };

                // Define output directory
                string outputDir = "ConvertedMsg";

                // Ensure output directory exists
                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Error: Unable to create output directory – {outputDir}. {dirEx.Message}");
                        return;
                    }
                }

                foreach (string inputPath in inputFiles)
                {
                    if (!File.Exists(inputPath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                        continue;
                    }

                    string extension = Path.GetExtension(inputPath).ToLowerInvariant();
                    string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".msg");

                    try
                    {
                        if (extension == ".eml")
                        {
                            // Load EML as MailMessage
                            using (MailMessage mailMessage = MailMessage.Load(inputPath))
                            {
                                // Save as MSG with Unicode format preserving original dates
                                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                                {
                                    PreserveOriginalDates = true
                                };
                                mailMessage.Save(outputPath, saveOptions);
                            }
                        }
                        else if (extension == ".msg")
                        {
                            // Load MSG as MapiMessage and re‑save (preserves content)
                            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
                            {
                                mapiMessage.Save(outputPath);
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine($"Warning: Unsupported file format – {inputPath}");
                        }
                    }
                    catch (Exception fileEx)
                    {
                        Console.Error.WriteLine($"Error processing file {inputPath}: {fileEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Mbox;

namespace AsposeEmailMIMEExample
{
    // Author: Aspose.Email .NET sample author
    class Program
    {
        static void Main()
        {
            try
            {
                // Define file paths
                const string emlInputPath = "input.eml";
                const string msgInputPath = "input.msg";
                const string mboxInputPath = "storage.mbox";

                // -------------------------------------------------
                // 1. Convert EML to MSG preserving original dates
                // -------------------------------------------------
                if (File.Exists(emlInputPath))
                {
                    try
                    {
                        // Load EML with options to preserve TNEF and embedded messages
                        EmlLoadOptions emlLoadOptions = new EmlLoadOptions
                        {
                            PreserveTnefAttachments = true,
                            PreserveEmbeddedMessageFormat = true
                        };

                        using (MailMessage emlMessage = MailMessage.Load(emlInputPath, emlLoadOptions))
                        {
                            // Save as MSG with preserved dates
                            MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                            {
                                PreserveOriginalDates = true
                            };

                            const string msgOutputPath = "converted.msg";
                            
            string outputDir = Path.GetDirectoryName(msgOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
emlMessage.Save(msgOutputPath, msgSaveOptions);
                            Console.WriteLine($"EML converted to MSG: {msgOutputPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error converting EML to MSG: {ex.Message}");
                    }
                }
                else
                {
                    Console.Error.WriteLine($"EML input file not found: {emlInputPath}");
                }

                // -------------------------------------------------
                // 2. Convert MSG to EML preserving embedded message format
                // -------------------------------------------------
                if (File.Exists(msgInputPath))
                {
                    try
                    {
                        using (MapiMessage mapiMessage = MapiMessage.Load(msgInputPath))
                        {
                            // Convert MAPI message to MailMessage
                            MailConversionOptions conversionOptions = new MailConversionOptions();
                            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                            {
                                // Save as EML with preserved embedded message format
                                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                                {
                                    PreserveEmbeddedMessageFormat = true
                                };

                                const string emlOutputPath = "converted_from_msg.eml";
                                mailMessage.Save(emlOutputPath, emlSaveOptions);
                                Console.WriteLine($"MSG converted to EML: {emlOutputPath}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error converting MSG to EML: {ex.Message}");
                    }
                }
                else
                {
                    Console.Error.WriteLine($"MSG input file not found: {msgInputPath}");
                }

                // -------------------------------------------------
                // 3. Process MBOX storage: extract each message to .eml
                // -------------------------------------------------
                if (File.Exists(mboxInputPath))
                {
                    try
                    {
                        using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxInputPath, new MboxLoadOptions()))
                        {
                            foreach (MboxMessageInfo mboxInfo in mboxReader.EnumerateMessageInfo())
                            {
                                using (MailMessage extractedMessage = mboxReader.ExtractMessage(mboxInfo.EntryId, new EmlLoadOptions()))
                                {
                                    string subject = string.IsNullOrEmpty(extractedMessage.Subject) ? "Message" : extractedMessage.Subject;
                                    string safeFileName = SanitizeFileName(subject) + ".eml";

                                    extractedMessage.Save(safeFileName);
                                    Console.WriteLine($"Extracted message saved: {safeFileName}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
                    }
                }
                else
                {
                    Console.Error.WriteLine($"MBOX input file not found: {mboxInputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Helper to create a file‑system safe name from a subject line
        private static string SanitizeFileName(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string escaped = Regex.Replace(name, $"[{Regex.Escape(invalidChars)}]+", "_");
            return escaped.Length > 200 ? escaped.Substring(0, 200) : escaped;
        }
    }
}

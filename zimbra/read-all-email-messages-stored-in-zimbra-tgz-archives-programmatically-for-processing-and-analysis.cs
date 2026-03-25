using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            // Path to the Zimbra TGZ archive
            string tgzPath = "archive.tgz";

            // Verify that the TGZ file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Directory where extracted messages will be saved
            string outputDirectory = "ExtractedMessages";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                return;
            }

            // Open the TGZ archive using TgzReader
            try
            {
                using (TgzReader reader = new TgzReader(tgzPath))
                {
                    // Display total number of items in the archive
                    int totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Total messages in archive: {totalItems}");

                    // Iterate through all messages
                    while (true)
                    {
                        try
                        {
                            // Move to the next message; if no more messages, break the loop
                            reader.ReadNextMessage();

                            // Retrieve the current message
                            Aspose.Email.MailMessage currentMessage = reader.CurrentMessage;
                            if (currentMessage == null)
                            {
                                break;
                            }

                            // Process the message (e.g., display basic metadata)
                            Console.WriteLine($"Subject: {currentMessage.Subject}");
                            Console.WriteLine($"From: {currentMessage.From}");
                            Console.WriteLine($"Date: {currentMessage.Date}");
                            Console.WriteLine(new string('-', 40));

                            // Save the message as an .eml file
                            string emlFilePath = Path.Combine(outputDirectory,
                                $"{Guid.NewGuid()}.eml");
                            try
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    currentMessage.Save(ms, SaveOptions.DefaultEml);
                                    File.WriteAllBytes(emlFilePath, ms.ToArray());
                                }
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
                            }
                        }
                        catch (Exception readEx)
                        {
                            // Assume end of archive or read error; exit loop
                            Console.Error.WriteLine($"Reading stopped: {readEx.Message}");
                            break;
                        }
                    }

                    // Optionally export the entire archive structure to the output directory
                    try
                    {
                        reader.ExportTo(outputDirectory);
                    }
                    catch (Exception exportEx)
                    {
                        Console.Error.WriteLine($"Error exporting archive: {exportEx.Message}");
                    }
                }
            }
            catch (Exception tgzEx)
            {
                Console.Error.WriteLine($"Error processing TGZ archive: {tgzEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
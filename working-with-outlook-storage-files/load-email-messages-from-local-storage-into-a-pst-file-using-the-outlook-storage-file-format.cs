using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputDirectory = "Emails";
                string pstFilePath = "output.pst";

                // Verify input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                    return;
                }

                // Create or open PST file
                PersonalStorage pstStorage;
                try
                {
                    if (File.Exists(pstFilePath))
                    {
                        pstStorage = PersonalStorage.FromFile(pstFilePath);
                    }
                    else
                    {
                        pstStorage = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create or open PST file – {ex.Message}");
                    return;
                }

                using (pstStorage)
                {
                    // Get the Inbox predefined folder
                    FolderInfo inboxFolder;
                    try
                    {
                        inboxFolder = pstStorage.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Unable to retrieve Inbox folder – {ex.Message}");
                        return;
                    }

                    // Process each .eml file in the input directory
                    string[] emlFiles;
                    try
                    {
                        emlFiles = Directory.GetFiles(inputDirectory, "*.eml");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Unable to enumerate .eml files – {ex.Message}");
                        return;
                    }

                    foreach (string emlPath in emlFiles)
                    {
                        if (!File.Exists(emlPath))
                        {
                            Console.Error.WriteLine($"Warning: File not found – {emlPath}");
                            continue;
                        }

                        try
                        {
                            using (MailMessage mailMessage = MailMessage.Load(emlPath))
                            {
                                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                                {
                                    string entryId = inboxFolder.AddMessage(mapiMessage);
                                    Console.WriteLine($"Added message '{mailMessage.Subject}' with EntryId {entryId}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing file '{emlPath}': {ex.Message}");
                        }
                    }

                    // Save changes (if any) – PST is saved automatically on dispose
                    Console.WriteLine("All messages have been processed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}

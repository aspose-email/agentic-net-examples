using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MSG file and output MBOX file paths
            string msgFilePath = "input.msg";
            string mboxFilePath = "output.mbox";

            // Verify input MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Ensure the directory for the output MBOX exists
            string mboxDirectory = Path.GetDirectoryName(mboxFilePath);
            if (!string.IsNullOrEmpty(mboxDirectory) && !Directory.Exists(mboxDirectory))
            {
                try
                {
                    Directory.CreateDirectory(mboxDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {mboxDirectory}. {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgFilePath))
            {
                // Convert MailMessage to MapiMessage with advanced options
                MapiConversionOptions conversionOptions = MapiConversionOptions.UnicodeFormat;
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage, conversionOptions))
                {
                    // Create a temporary PST file to hold the message
                    string tempPstPath = Path.Combine(Path.GetTempPath(), $"temp_{Guid.NewGuid()}.pst");
                    try
                    {
                        // Create PST with Unicode format
                        using (PersonalStorage pst = PersonalStorage.Create(tempPstPath, FileFormatVersion.Unicode))
                        {
                            // Add the MapiMessage to the PST root folder
                            pst.RootFolder.AddMessage(mapiMessage);

                            // Convert the PST (PersonalStorage) to MBOX format
                            MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxFilePath, null);
                        }
                    }
                    finally
                    {
                        // Clean up temporary PST file
                        if (File.Exists(tempPstPath))
                        {
                            try
                            {
                                File.Delete(tempPstPath);
                            }
                            catch
                            {
                                // Suppress any cleanup errors
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

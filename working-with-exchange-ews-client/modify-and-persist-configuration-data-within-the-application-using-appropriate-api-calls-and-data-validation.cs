using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "config.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new PST file with Unicode format
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Validate that the root folder is accessible
                if (pst.RootFolder == null)
                {
                    Console.Error.WriteLine("Root folder is not accessible.");
                    return;
                }

                // Define the configuration folder name
                const string configFolderName = "ConfigFolder";

                // Check if the configuration folder already exists; create if not
                bool folderExists = false;
                foreach (FolderInfo existingFolder in pst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(existingFolder.DisplayName, configFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        folderExists = true;
                        break;
                    }
                }

                if (!folderExists)
                {
                    try
                    {
                        pst.RootFolder.AddSubFolder(configFolderName);
                        Console.WriteLine($"Added folder '{configFolderName}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add folder: {ex.Message}");
                        return;
                    }
                }

                // Retrieve the configuration folder
                FolderInfo configFolderInfo = pst.RootFolder.GetSubFolder(configFolderName);
                if (configFolderInfo == null)
                {
                    Console.Error.WriteLine($"Unable to retrieve folder '{configFolderName}'.");
                    return;
                }

                // Prepare configuration data
                const string configSubject = "Application Configuration";
                const string configBody = "Setting1=ValueA;Setting2=ValueB;";

                // Simple validation of configuration data
                if (string.IsNullOrWhiteSpace(configSubject))
                {
                    Console.Error.WriteLine("Configuration subject cannot be empty.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(configBody))
                {
                    Console.Error.WriteLine("Configuration body cannot be empty.");
                    return;
                }

                // Create a MailMessage representing the configuration
                MailMessage configMessage = new MailMessage(
                    "config@myapp.local",
                    "config@myapp.local",
                    configSubject,
                    configBody);

                // Add the configuration message to the PST folder
                try
                {
                    configFolderInfo.AddMessage(MapiMessage.FromMailMessage(configMessage));
                    Console.WriteLine("Configuration message saved to PST.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

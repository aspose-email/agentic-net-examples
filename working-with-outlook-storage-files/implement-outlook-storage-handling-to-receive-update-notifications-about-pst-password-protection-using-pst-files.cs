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
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the message store.
                MessageStore store = pst.Store;

                // Check if the PST is password protected.
                bool isProtected = store.IsPasswordProtected;
                Console.WriteLine($"PST password protected: {isProtected}");

                // If protected, validate a sample password.
                if (isProtected)
                {
                    string samplePassword = "oldPassword";
                    bool isValid = store.IsPasswordValid(samplePassword);
                    Console.WriteLine($"Sample password valid: {isValid}");
                }
                else
                {
                    // Set a new password.
                    string newPassword = "mySecret123";
                    try
                    {
                        store.ChangePassword(newPassword);
                        Console.WriteLine("Password set successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error setting password: {ex.Message}");
                    }
                }

                // Example of monitoring the PST file for external changes (e.g., password updates).
                using (FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(pstPath) ?? ".", Path.GetFileName(pstPath)))
                {
                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                    watcher.Changed += (sender, e) =>
                    {
                        Console.WriteLine($"PST file changed: {e.ChangeType}");
                        // Re-open to reflect possible password changes.
                        try
                        {
                            using (PersonalStorage refreshed = PersonalStorage.FromFile(pstPath))
                            {
                                Console.WriteLine($"Refreshed password protection status: {refreshed.Store.IsPasswordProtected}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error reloading PST after change: {ex.Message}");
                        }
                    };
                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine("Press Enter to exit and stop watching...");
                    Console.ReadLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory '{pstDirectory}'. {dirEx.Message}");
                    return;
                }
            }

            // If the PST file does not exist, create a minimal placeholder PST
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created; no further action needed
                    }
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create PST file '{pstPath}'. {createEx.Message}");
                    return;
                }
            }

            // Open the PST file with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                MessageStore store = pst.Store;

                // Add a password if the PST is not already protected
                if (!store.IsPasswordProtected)
                {
                    try
                    {
                        store.ChangePassword("myPassword");
                        Console.WriteLine("Password added to PST.");
                    }
                    catch (Exception pwdEx)
                    {
                        Console.Error.WriteLine($"Error adding password: {pwdEx.Message}");
                        return;
                    }
                }

                // Modify the existing password
                if (store.IsPasswordProtected && store.IsPasswordValid("myPassword"))
                {
                    try
                    {
                        store.ChangePassword("newPassword");
                        Console.WriteLine("Password changed to a new value.");
                    }
                    catch (Exception modEx)
                    {
                        Console.Error.WriteLine($"Error changing password: {modEx.Message}");
                        return;
                    }
                }

                // Remove the password (set to empty string)
                if (store.IsPasswordProtected && store.IsPasswordValid("newPassword"))
                {
                    try
                    {
                        store.ChangePassword(string.Empty);
                        Console.WriteLine("Password removed from PST.");
                    }
                    catch (Exception remEx)
                    {
                        Console.Error.WriteLine($"Error removing password: {remEx.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

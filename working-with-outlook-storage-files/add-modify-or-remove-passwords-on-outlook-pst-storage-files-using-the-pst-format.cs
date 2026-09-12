using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

// Author: Aspose.Email PST password management example
class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                MessageStore store = pst.Store;

                // Check whether the PST is already password protected.
                if (store.IsPasswordProtected)
                {
                    Console.WriteLine("PST is currently password protected.");

                    // Example: validate an existing password (replace with actual password).
                    string oldPassword = "oldpass";
                    bool isValid = store.IsPasswordValid(oldPassword);
                    Console.WriteLine($"Provided old password is {(isValid ? "valid" : "invalid")}.");

                    // Change to a new password.
                    string newPassword = "newpass";
                    store.ChangePassword(newPassword);
                    Console.WriteLine("Password changed successfully.");
                }
                else
                {
                    Console.WriteLine("PST is not password protected.");

                    // Set a new password.
                    string newPassword = "newpass";
                    store.ChangePassword(newPassword);
                    Console.WriteLine("Password set successfully.");
                }

                // To remove the password, set it to an empty string.
                // Uncomment the following lines to clear the password.
                /*
                store.ChangePassword(string.Empty);
                Console.WriteLine("Password removed.");
                */
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        const string pstPath = "sample.pst";

        // Ensure the PST file exists; create a minimal placeholder if missing.
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
                Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                return;
            }
        }

        // Open the PST file with write access.
        try
        {
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Check if the PST is password protected.
                bool isProtected = pst.Store.IsPasswordProtected;
                Console.WriteLine($"PST password protected: {isProtected}");

                if (isProtected)
                {
                    // Attempt to validate a sample password.
                    const string testPassword = "test";
                    bool isValid = pst.Store.IsPasswordValid(testPassword);
                    Console.WriteLine($"Is test password valid? {isValid}");
                }
                else
                {
                    // Set a new password for the PST.
                    const string newPassword = "MySecret123";
                    pst.Store.ChangePassword(newPassword);
                    Console.WriteLine("Password set on PST.");

                    // Verify the new password.
                    bool isNowProtected = pst.Store.IsPasswordProtected;
                    bool isValidNow = pst.Store.IsPasswordValid(newPassword);
                    Console.WriteLine($"PST now password protected: {isNowProtected}");
                    Console.WriteLine($"New password validation result: {isValidNow}");
                }

                // Example of accessing a property (display name) and updating it.
                string originalName = pst.Store.DisplayName;
                Console.WriteLine($"Original PST display name: {originalName}");

                const string newDisplayName = "Updated PST Store";
                pst.Store.ChangeDisplayName(newDisplayName);
                Console.WriteLine($"PST display name changed to: {pst.Store.DisplayName}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
        }
    }
}

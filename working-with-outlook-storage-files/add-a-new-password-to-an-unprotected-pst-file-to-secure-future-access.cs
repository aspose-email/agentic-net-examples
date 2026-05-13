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
            string pstPath = "unprotected.pst";
            string newPassword = "MySecurePassword";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access and set a password if not already protected.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    MessageStore store = pst.Store;
                    if (store.IsPasswordProtected)
                    {
                        Console.WriteLine("PST is already password protected.");
                    }
                    else
                    {
                        store.ChangePassword(newPassword);
                        Console.WriteLine("Password has been set on the PST file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

namespace AsposeEmailPstPasswordDemo
{
    class Program
    {
        static void Main()
        {
            // Path to the PST file
            const string pstPath = "sample.pst";

            // Ensure a PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create a new PST file with Unicode format
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
            }

            // Existing password (if any) and the new password to set
            const string existingPassword = "oldPassword";
            const string newPassword = "newPassword";

            try
            {
                // Open PST in read‑only mode to inspect password protection
                using (PersonalStorage pstReadOnly = PersonalStorage.FromFile(pstPath, true))
                {
                    bool isProtected = pstReadOnly.Store.IsPasswordProtected;
                    Console.WriteLine($"Is password protected: {isProtected}");

                    if (isProtected)
                    {
                        bool isValid = pstReadOnly.Store.IsPasswordValid(existingPassword);
                        Console.WriteLine($"Existing password valid: {isValid}");

                        if (!isValid)
                        {
                            Console.Error.WriteLine("The provided existing password is invalid.");
                            return;
                        }
                    }
                }

                // Open PST in writable mode to change the password
                using (PersonalStorage pstWritable = PersonalStorage.FromFile(pstPath, false))
                {
                    pstWritable.Store.ChangePassword(newPassword);
                    Console.WriteLine("Password has been changed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

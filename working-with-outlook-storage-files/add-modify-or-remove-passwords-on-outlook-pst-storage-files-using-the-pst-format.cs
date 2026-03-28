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

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file with write access.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    MessageStore store = pst.Store;

                    Console.WriteLine($"PST Display Name: {store.DisplayName}");
                    Console.WriteLine($"Is Password Protected: {store.IsPasswordProtected}");

                    // Set a new password if none exists.
                    if (!store.IsPasswordProtected)
                    {
                        try
                        {
                            store.ChangePassword("mySecret");
                            Console.WriteLine("Password set to 'mySecret'.");
                        }
                        catch (Exception pwdEx)
                        {
                            Console.Error.WriteLine($"Error setting password: {pwdEx.Message}");
                            return;
                        }
                    }

                    // Validate the current password.
                    try
                    {
                        bool isValid = store.IsPasswordValid("mySecret");
                        Console.WriteLine($"Is 'mySecret' valid? {isValid}");
                    }
                    catch (Exception valEx)
                    {
                        Console.Error.WriteLine($"Error validating password: {valEx.Message}");
                        return;
                    }

                    // Change the password to a new value.
                    try
                    {
                        store.ChangePassword("newSecret");
                        Console.WriteLine("Password changed to 'newSecret'.");
                    }
                    catch (Exception changeEx)
                    {
                        Console.Error.WriteLine($"Error changing password: {changeEx.Message}");
                        return;
                    }

                    // Validate the new password.
                    try
                    {
                        bool isNewValid = store.IsPasswordValid("newSecret");
                        Console.WriteLine($"Is 'newSecret' valid? {isNewValid}");
                    }
                    catch (Exception valNewEx)
                    {
                        Console.Error.WriteLine($"Error validating new password: {valNewEx.Message}");
                        return;
                    }

                    // Remove the password by setting an empty string.
                    try
                    {
                        store.ChangePassword(string.Empty);
                        Console.WriteLine("Password removed.");
                    }
                    catch (Exception removeEx)
                    {
                        Console.Error.WriteLine($"Error removing password: {removeEx.Message}");
                        return;
                    }

                    // Final check: should not be password protected.
                    Console.WriteLine($"Is Password Protected after removal: {store.IsPasswordProtected}");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

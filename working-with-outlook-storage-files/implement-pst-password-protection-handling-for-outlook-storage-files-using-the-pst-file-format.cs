using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                MessageStore store = pst.Store;
                Console.WriteLine($"Is password protected: {store.IsPasswordProtected}");

                if (!store.IsPasswordProtected)
                {
                    string newPassword = "Secret123";
                    try
                    {
                        store.ChangePassword(newPassword);
                        Console.WriteLine("Password set successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error setting password: {ex.Message}");
                        return;
                    }

                    bool isValid = store.IsPasswordValid(newPassword);
                    Console.WriteLine($"Password validation result: {isValid}");
                }
                else
                {
                    Console.Write("Enter password to validate: ");
                    string inputPassword = Console.ReadLine();
                    bool isValid = store.IsPasswordValid(inputPassword);
                    Console.WriteLine($"Password validation result: {isValid}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

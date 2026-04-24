using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter PST file path: ");
            string pstPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pstPath))
            {
                Console.Error.WriteLine("PST path is empty.");
                return;
            }

            string directory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory: {dirEx.Message}");
                    return;
                }
            }

            if (File.Exists(pstPath))
            {
                Console.WriteLine("File already exists and will be overwritten.");
                try
                {
                    File.Delete(pstPath);
                }
                catch (Exception delEx)
                {
                    Console.Error.WriteLine($"Failed to delete existing file: {delEx.Message}");
                    return;
                }
            }

            Console.Write("Enter password for PST: ");
            string password = Console.ReadLine() ?? string.Empty;

            try
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    pst.Store.ChangePassword(password);
                }

                Console.WriteLine("PST file created successfully with the specified password.");
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Failed to create PST: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

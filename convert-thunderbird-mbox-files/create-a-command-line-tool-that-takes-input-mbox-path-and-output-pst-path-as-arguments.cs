using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace MboxToPstTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate arguments
                if (args == null || args.Length < 2)
                {
                    Console.Error.WriteLine("Usage: MboxToPstTool <input.mbox> <output.pst>");
                    return;
                }

                string mboxPath = args[0];
                string pstPath = args[1];

                // Verify input MBOX file exists
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure output directory exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(pstDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{pstDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                // Perform conversion using Aspose.Email's MailStorageConverter
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    Console.WriteLine($"Conversion completed successfully. PST saved to: {pstPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

namespace MboxToPstConverter
{
    // Author: Generated example using Aspose.Email for .NET
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate arguments
                if (args.Length < 2)
                {
                    Console.Error.WriteLine("Usage: MboxToPstConverter <input.mbox> <output.pst>");
                    return;
                }

                string mboxPath = args[0];
                string pstPath = args[1];

                // Guard input file existence
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Perform conversion
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // The conversion is completed when the method returns.
                }

                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

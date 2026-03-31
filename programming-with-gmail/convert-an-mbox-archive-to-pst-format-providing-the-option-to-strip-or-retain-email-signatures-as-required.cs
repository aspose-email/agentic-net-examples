using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

namespace MboxToPstConversion
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths for the source MBOX and the target PST files
                string mboxPath = "input.mbox";
                string pstPath = "output.pst";

                // Set to true to remove signatures, false to retain them
                bool removeSignature = true;

                // Ensure the input MBOX file exists; create a minimal placeholder if it does not
                if (!File.Exists(mboxPath))
                {
                    string placeholderContent = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\n";
                    string mboxDirectory = Path.GetDirectoryName(mboxPath);
                    if (!string.IsNullOrEmpty(mboxDirectory) && !Directory.Exists(mboxDirectory))
                    {
                        Directory.CreateDirectory(mboxDirectory);
                    }
                    File.WriteAllText(mboxPath, placeholderContent);
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }

                // Ensure the output directory exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Validation: read through the MBOX using MboxStorageReader as required by the rules
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageCount = 0;
                    MailMessage message;
                    while ((message = reader.ReadNextMessage()) != null)
                    {
                        messageCount++;
                    }
                    Console.WriteLine($"MBOX contains {messageCount} message(s).");
                }

                // Configure conversion options, including signature removal if requested
                MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
                conversionOptions.RemoveSignature = removeSignature;

                // Perform the conversion from MBOX to PST
                MailStorageConverter.MboxToPst(mboxPath, pstPath, conversionOptions);
                Console.WriteLine($"Conversion completed successfully. PST saved to '{pstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

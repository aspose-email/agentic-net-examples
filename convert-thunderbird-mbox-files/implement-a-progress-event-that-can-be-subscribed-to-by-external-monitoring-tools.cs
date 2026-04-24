using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure input MBOX exists; create a minimal placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream fs = File.Create(mboxPath))
                    {
                        // Write a minimal empty MBOX header.
                        byte[] header = System.Text.Encoding.UTF8.GetBytes("From - \r\n");
                        fs.Write(header, 0, header.Length);
                    }
                    Console.WriteLine($"Placeholder MBOX created at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
                    return;
                }
            }

            // Prepare conversion options with a progress handler.
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.MessageHandler = (MailMessage msg) =>
            {
                Console.WriteLine($"Converted message: Subject = '{msg.Subject}'");
            };

            // Perform conversion inside a using block to dispose the resulting PST.
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                Console.WriteLine($"MBOX '{mboxPath}' successfully converted to PST '{pstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

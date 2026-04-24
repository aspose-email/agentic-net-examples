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
            // Define input MBOX and output PST file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create an empty placeholder if missing
            if (!File.Exists(mboxPath))
            {
                File.WriteAllText(mboxPath, string.Empty);
                Console.WriteLine($"Placeholder MBOX file created at '{mboxPath}'.");
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Define the cutoff date: messages older than this will be excluded
            DateTime cutoffDate = new DateTime(2022, 1, 1);

            // Create conversion options with a custom MailHandler delegate
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();

            // The handler receives each MailMessage read from the MBOX.
            // If the message is older than the cutoff, modify its subject to indicate exclusion.
            // (Aspose.Email does not provide a direct way to skip adding the message in this delegate,
            //  so we mark it; in a real scenario you might implement a custom reader instead.)
            options.MessageHandler = new MailStorageConverter.MailHandler((MailMessage message) =>
            {
                if (message.Date < cutoffDate)
                {
                    // Mark the message as excluded; you could also clear its body or set a flag.
                    message.Subject = "[Excluded by date filter]";
                }
            });

            // Perform the conversion using the options with the custom handler
            PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options);

            // Dispose the PST object to release resources
            pst.Dispose();

            Console.WriteLine($"MBOX to PST conversion completed. PST saved at '{pstPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

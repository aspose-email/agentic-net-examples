using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = false;
            options.MessageHandler = CustomMessageHandler;

            // Perform conversion
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                Console.WriteLine("Conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Handler invoked for each message read from the MBOX file
    private static void CustomMessageHandler(MailMessage message)
    {
        // Convert the MailMessage to a MapiMessage to access categories
        MapiMessage mapi = MapiMessage.FromMailMessage(message);

        // Example logic: map subject prefixes to categories
        if (!string.IsNullOrEmpty(message.Subject))
        {
            if (message.Subject.StartsWith("[Work]"))
            {
                FollowUpManager.AddCategory(mapi, "Work");
            }
            else if (message.Subject.StartsWith("[Personal]"))
            {
                FollowUpManager.AddCategory(mapi, "Personal");
            }
        }

        // Replace the original message body with the modified MapiMessage content
        // (Convert back to MailMessage to let the converter use the updated categories)
        MailConversionOptions convOptions = new MailConversionOptions();
        MailMessage updated = mapi.ToMailMessage(convOptions);
        // Update the reference passed to the converter
        message.Body = updated.Body;
        message.IsBodyHtml = updated.IsBodyHtml;
        message.Subject = updated.Subject;
        // Note: Categories are stored in the MapiMessage and will be persisted in PST.
    }
}

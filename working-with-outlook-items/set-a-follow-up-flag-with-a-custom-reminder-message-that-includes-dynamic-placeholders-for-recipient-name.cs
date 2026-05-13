using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output path for the draft message
            string outputPath = "draft.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a draft MAPI message
            using (MapiMessage message = new MapiMessage())
            {
                message.SenderEmailAddress = "sender@example.com";
                message.Subject = "Project Update";
                message.Body = "Please see the attached document.";

                // Set a follow‑up flag for recipients with a placeholder for the recipient name
                string flagRequest = "Please review the document, {RecipientName}.";
                FollowUpManager.SetFlagForRecipients(message, flagRequest);

                // Save the draft message to a file
                message.Save(outputPath);
            }

            Console.WriteLine("Draft message with follow‑up flag saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

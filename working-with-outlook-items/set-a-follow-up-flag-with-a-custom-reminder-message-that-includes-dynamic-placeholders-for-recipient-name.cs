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
            // Recipient details
            string recipientEmail = "john.doe@example.com";
            string recipientName = "John Doe";

            // Create a draft MAPI message (from, to, subject, body)
            using (MapiMessage draft = new MapiMessage("sender@example.com", recipientEmail, "Project Update", "Please see the attached update."))
            {
                // Custom flag request with dynamic recipient name
                string flagRequest = $"Please review the document, {recipientName}";

                // Reminder time (e.g., two days from now)
                DateTime reminderTime = DateTime.Now.AddDays(2);

                // Set follow‑up flag for recipients
                FollowUpManager.SetFlagForRecipients(draft, flagRequest, reminderTime);

                // Save the draft message to a file
                string outputPath = "draft.msg";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                    draft.Save(outputPath);
                    Console.WriteLine($"Draft saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save draft: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

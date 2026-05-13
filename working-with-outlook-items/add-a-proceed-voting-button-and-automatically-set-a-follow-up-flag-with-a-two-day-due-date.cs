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
            string outputPath = "output.msg";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MailMessage first
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To.Add("recipient@example.com");
                mail.Subject = "Test message with voting button and follow‑up flag";
                mail.Body = "Please review and proceed.";

                // Convert to MAPI message to work with voting buttons and flags
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail);

                // Add a "Proceed" voting button
                FollowUpManager.AddVotingButton(mapiMessage, "Proceed");

                // Set a follow‑up flag with a due date two days from now
                DateTime startDate = DateTime.Now;
                DateTime dueDate = startDate.AddDays(2);
                FollowUpManager.SetFlag(mapiMessage, "Please follow up", startDate, dueDate);

                // Save the MAPI message to a .msg file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    mapiMessage.Save(fs, SaveOptions.DefaultMsgUnicode);
                }
            }

            Console.WriteLine("Message saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

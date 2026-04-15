using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace FollowUpFlagExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the output file path for the draft message
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "DraftMessage.msg");

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create a draft MAPI message
                using (MapiMessage message = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Action Required",
                    "Please see the attached document."))
                {
                    // Set a custom flag with a reminder for recipients
                    DateTime reminderTime = DateTime.Now.AddHours(4);
                    FollowUpManager.SetFlagForRecipients(
                        message,
                        "Please review the attached document and respond by end of day.",
                        reminderTime);

                    // Save the draft message to a file
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        Console.WriteLine("Draft message saved to: " + outputPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine("Error saving message: " + saveEx.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
                return;
            }
        }
    }
}

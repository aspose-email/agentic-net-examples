using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MailMessage instance
            MailMessage message = new MailMessage();
            try
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Sample MSG Message";
                message.Body = "This is a sample email saved in MSG format using Aspose.Email.";

                // Configure save options for MSG format with preserved dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat)
                {
                    PreserveOriginalDates = true
                };

                // Save the message to MSG file
                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Message saved successfully to '{outputPath}'.");
            }
            finally
            {
                // Dispose the MailMessage
                if (message != null)
                    message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MailMessage instance
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample MSG Message";
                message.Body = "This is a sample email saved as MSG format.";

                // Configure MSG save options
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);

                // Save the message to MSG file
                message.Save(outputPath, saveOptions);
            }

            Console.WriteLine("MailMessage saved successfully to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

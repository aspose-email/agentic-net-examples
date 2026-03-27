using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Ensure output directory exists
            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Define the path for the MSG file
            string msgFilePath = Path.Combine(outputDirectory, "sample.msg");

            // Create a new MailMessage instance
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                MailAddressCollection toAddresses = new MailAddressCollection();
                toAddresses.Add(new MailAddress("recipient@example.com"));
                message.To = toAddresses;
                message.Subject = "Test Subject";
                message.Body = "This is a test email.";

                // Prepare save options for MSG format
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);

                // Save the message to MSG file
                message.Save(msgFilePath, saveOptions);
                Console.WriteLine($"Message saved to: {msgFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

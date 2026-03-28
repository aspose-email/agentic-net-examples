using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file
            string msgPath = "message.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage instance
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Retrieve the HTML body content
                string htmlContent = mailMessage.HtmlBody;

                // Output a simple confirmation (or the content itself)
                Console.WriteLine("HTML content extracted successfully.");
                // Uncomment the following line to display the HTML content
                // Console.WriteLine(htmlContent);
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors gracefully
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

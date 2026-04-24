using System;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Create an AMP message and set its properties
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set the subject of the message
                ampMessage.Subject = "Welcome to Aspose.Email AMP";

                // Set the sender (From) address
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");

                // Optionally set the Sender property as well
                ampMessage.Sender = new MailAddress("sender@example.com", "Sender Name");

                // Output the set values to the console
                Console.WriteLine("Subject: " + ampMessage.Subject);
                Console.WriteLine("From: " + ampMessage.From);
                Console.WriteLine("Sender: " + ampMessage.Sender);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

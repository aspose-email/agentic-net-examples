using System;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text body.";
                ampMessage.IsBodyHtml = false;
                ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                // Serialize the message to a MIME string suitable for SMTP transmission
                string mimeString = ampMessage.ToString();

                Console.WriteLine("Serialized MIME message:");
                Console.WriteLine(mimeString);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

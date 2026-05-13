using System;
using Aspose.Email;
using Aspose.Email.Mime;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Sample with AlternateView";

                // Create plain‑text alternate view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain‑text version of the email.", Encoding.UTF8, "text/plain");

                // Add the alternate view to the message
                message.AlternateViews.Add(plainView);

                Console.WriteLine("Alternate view added. Count: " + message.AlternateViews.Count);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

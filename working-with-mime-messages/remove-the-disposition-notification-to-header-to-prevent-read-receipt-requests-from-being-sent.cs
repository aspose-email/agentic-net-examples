using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage message = new MailMessage())
            {
                // Add a read receipt request header
                message.Headers.Add("Disposition-Notification-To", "sender@example.com");

                Console.WriteLine("Before removal:");
                Console.WriteLine(message.Headers["Disposition-Notification-To"]);

                // Remove the header to prevent read receipt requests
                message.Headers.Remove("Disposition-Notification-To");

                Console.WriteLine("After removal:");
                string headerValue = message.Headers["Disposition-Notification-To"];
                Console.WriteLine(headerValue ?? "Header not present");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

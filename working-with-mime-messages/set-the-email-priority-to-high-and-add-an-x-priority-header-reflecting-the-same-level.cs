using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            using (MailMessage message = new MailMessage())
            {
                // Set the priority to High
                message.Priority = MailPriority.High;

                // Add the X-Priority header that reflects the same priority level
                // X-Priority values: 1 (Highest), 2 (High), 3 (Normal), 4 (Low), 5 (Lowest)
                message.Headers.Add("X-Priority", "2 (High)");

                // (Optional) Set other required fields for a valid message
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "High Priority Email";
                message.Body = "This email is marked as high priority.";

                // The message can now be sent using an appropriate client, e.g., SmtpClient.
                // For demonstration purposes, we simply output the header values.
                Console.WriteLine("Priority set to: " + message.Priority);
                Console.WriteLine("X-Priority header: " + message.Headers["X-Priority"]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

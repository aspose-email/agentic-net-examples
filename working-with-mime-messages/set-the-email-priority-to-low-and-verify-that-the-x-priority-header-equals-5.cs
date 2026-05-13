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
                // Set the email priority to Low
                message.Priority = MailPriority.Low;

                // Verify that the X-Priority header equals 5
                string xPriorityHeader = message.Headers["X-Priority"];
                if (xPriorityHeader == "5")
                {
                    Console.WriteLine("X-Priority header correctly set to 5.");
                }
                else
                {
                    Console.WriteLine($"X-Priority header is '{xPriorityHeader ?? "null"}', expected '5'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

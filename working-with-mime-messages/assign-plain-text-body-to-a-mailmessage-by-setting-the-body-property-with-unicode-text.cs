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
                // Assign plain text body with Unicode characters
                message.Body = "Hello, world! Привет мир! こんにちは世界!";
                Console.WriteLine("Message body set to:");
                Console.WriteLine(message.Body);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create the original mail message
            using (MailMessage original = new MailMessage("alice@example.com", "bob@example.com", "Original Subject", "Original Body"))
            {
                // Clone the original message
                using (MailMessage clone = original.Clone())
                {
                    // Modify the cloned message
                    clone.Subject = "Cloned Subject";
                    clone.Body = "Cloned Body";
                    clone.To.Add("charlie@example.com");

                    // Verify that the original message remains unchanged
                    Console.WriteLine("Original Subject: " + original.Subject);
                    Console.WriteLine("Original Body: " + original.Body);
                    Console.WriteLine("Original To count: " + original.To.Count);

                    Console.WriteLine("Cloned Subject: " + clone.Subject);
                    Console.WriteLine("Cloned Body: " + clone.Body);
                    Console.WriteLine("Cloned To count: " + clone.To.Count);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

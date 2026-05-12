using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a MailAddress with email and display name
            MailAddress recipientAddress = new MailAddress("jane.doe@example.com", "Jane Doe");

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Add the recipient to the To collection
                message.To.Add(recipientAddress);

                // Display recipient information
                Console.WriteLine("Recipient added:");
                Console.WriteLine("Address: " + recipientAddress.Address);
                Console.WriteLine("Display Name: " + recipientAddress.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

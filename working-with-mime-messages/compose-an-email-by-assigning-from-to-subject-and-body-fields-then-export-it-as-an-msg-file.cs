using Aspose.Email;
using System;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string msgFilePath = "sample.msg";

            // Create a new MAPI message and set its properties
            var message = new MapiMessage(
                "sender@example.com",   // From
                "recipient@example.com", // To
                "Test Subject",          // Subject
                "This is the body of the email." // Body
            );

            // Save the message as an MSG file
            message.Save(msgFilePath);
            Console.WriteLine($"Message saved to {msgFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

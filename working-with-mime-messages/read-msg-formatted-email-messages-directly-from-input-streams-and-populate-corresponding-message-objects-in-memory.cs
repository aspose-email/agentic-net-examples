using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the input MSG file
            string inputPath = "input.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a simple placeholder MAPI message
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message body."
                    );

                    // Save the placeholder to the expected path
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Open the MSG file as a stream and load it into a MapiMessage object
            using (FileStream fileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    MapiMessage message = MapiMessage.Load(fileStream);

                    // Output some basic properties to verify successful loading
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.SenderName} <{message.SenderEmailAddress}>");
                    Console.WriteLine($"To: {message.DisplayTo}");
                    Console.WriteLine($"Body: {message.Body}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG from stream: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

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
            // Path to the Outlook MSG file
            string msgFilePath = "sample.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Load the MSG file and read its body content
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Store the body text in a string variable
                string bodyContent = message.Body;

                // Output the body content (optional)
                Console.WriteLine("Message Body:");
                Console.WriteLine(bodyContent);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

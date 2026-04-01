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
            string msgFilePath = "sample.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
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

                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and obtain its plain‑text body.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    string plainTextBody = message.Body;
                    Console.WriteLine("Plain‑text body:");
                    Console.WriteLine(plainTextBody);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file or read body: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

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
            string msgPath = "note.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "placeholder@example.com",
                        "placeholder@example.com",
                        "Placeholder",
                        "This is a placeholder message."))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file, modify its body, and overwrite the original file.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Change the body text.
                    message.Body = "This is the updated body text.";

                    // Overwrite the original file with the modified message.
                    message.Save(msgPath);
                    Console.WriteLine($"Message body updated and saved to '{msgPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

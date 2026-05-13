using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "note.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file, modify the subject to uppercase, and save it back.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    if (!string.IsNullOrEmpty(message.Subject))
                    {
                        message.Subject = message.Subject.ToUpperInvariant();
                    }

                    // Overwrite the original file with the modified message.
                    message.Save(inputPath);
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

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
            // Path to the MSG note file
            string notePath = "note.msg";

            // Verify that the input file exists
            if (!File.Exists(notePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(notePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {notePath}");
                return;
            }

            // Load the MSG note
            using (MapiMessage note = MapiMessage.Load(notePath))
            {
                // Read the original body
                string originalBody = note.Body ?? string.Empty;

                // Replace specific keywords
                string modifiedBody = originalBody
                    .Replace("keyword1", "replacement1")
                    .Replace("keyword2", "replacement2");

                // Update the body if changes were made
                if (!originalBody.Equals(modifiedBody))
                {
                    note.Body = modifiedBody;

                    // Save the modified note back to the same MSG file
                    try
                    {
                        note.Save(notePath);
                        Console.WriteLine("Note updated and saved successfully.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error saving note: {saveEx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No keywords found to replace.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

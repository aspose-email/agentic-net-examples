using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string notePath = "note.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!System.IO.File.Exists(notePath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder note."))
                {
                    placeholder.Save(notePath);
                }

                Console.WriteLine($"Placeholder MSG created at '{notePath}'.");
            }

            // Load the MSG file, modify the subject, and save it back.
            using (MapiMessage note = MapiMessage.Load(notePath))
            {
                if (!string.IsNullOrEmpty(note.Subject))
                {
                    note.Subject = note.Subject.ToUpperInvariant();
                }

                note.Save(notePath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

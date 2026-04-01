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
            string msgPath = "notes_message.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "Placeholder Subject",
                        "Placeholder Body",
                        "sender@example.com",
                        "receiver@example.com"))
                    {
                        placeholder.Save(msgPath);
                    }
                    Console.WriteLine($"Placeholder MSG file created at {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage object
            MapiMessage notesMessage;
            try
            {
                notesMessage = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Process the loaded message
            using (notesMessage)
            {
                Console.WriteLine($"Subject: {notesMessage.Subject}");
                Console.WriteLine($"Body: {notesMessage.Body}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

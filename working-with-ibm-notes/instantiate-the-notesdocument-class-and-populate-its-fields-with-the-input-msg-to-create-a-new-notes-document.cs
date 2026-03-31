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
            string inputMsgPath = "input.msg";
            string outputNotePath = "output_note.msg";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholderMessage = new MapiMessage("placeholder@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholderMessage.Save(inputMsgPath);
                    }
                    Console.WriteLine($"Placeholder MSG created at: {inputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage loadedMessage;
            try
            {
                loadedMessage = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (loadedMessage)
            {
                // Create a MapiNote and populate fields from the loaded message
                MapiNote note = new MapiNote();
                try
                {
                    note.Subject = loadedMessage.Subject;
                    note.Body = loadedMessage.Body;
                    note.Color = 0; // Default color
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error populating note fields: {ex.Message}");
                    return;
                }

                using (note)
                {
                    // Save the note as a MSG file using the correct overload
                    try
                    {
                        note.Save(outputNotePath, NoteSaveFormat.Msg);
                        Console.WriteLine($"Notes document saved to: {outputNotePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving note: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

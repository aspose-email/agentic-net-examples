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
            // Define file paths
            string imagePath = "image.png";
            string notePath = "note.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                try
                {
                    // 1x1 pixel transparent PNG (base64 encoded)
                    byte[] placeholder = Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+XK6UAAAAASUVORK5CYII=");
                    File.WriteAllBytes(imagePath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Load image bytes
            byte[] imageData;
            try
            {
                imageData = File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read image file: {ex.Message}");
                return;
            }

            // Create a MAPI note and attach the image
            using (MapiNote note = new MapiNote("Sample Note", "This note contains an embedded image."))
            {
                // Add attachment directly via the collection overload
                note.Attachments.Add(Path.GetFileName(imagePath), imageData);

                // Save the note to MSG format
                try
                {
                    note.Save(notePath, NoteSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save note: {ex.Message}");
                    return;
                }
            }

            // Reload the note and verify the attachment
            try
            {
                MapiMessage loadedMessage = MapiMessage.Load(notePath);
                if (loadedMessage.SupportedType == MapiItemType.Note)
                {
                    MapiNote loadedNote = (MapiNote)loadedMessage.ToMapiMessageItem();
                    int attachmentCount = loadedNote.Attachments.Count;
                    Console.WriteLine($"Note reloaded. Attachments found: {attachmentCount}");
                    if (attachmentCount > 0)
                    {
                        Console.WriteLine($"First attachment name: {loadedNote.Attachments[0].FileName}");
                    }
                }
                else
                {
                    Console.Error.WriteLine("Loaded file is not a MAPI note.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or verify note: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

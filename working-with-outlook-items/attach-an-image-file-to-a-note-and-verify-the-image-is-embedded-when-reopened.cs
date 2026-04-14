using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string imagePath = "sample.png";
            string notePath = "note.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                try
                {
                    // 1x1 pixel transparent PNG (base64)
                    string base64Png = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+X0V8AAAAASUVORK5CYII=";
                    byte[] pngBytes = Convert.FromBase64String(base64Png);
                    File.WriteAllBytes(imagePath, pngBytes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the note file exists
            try
            {
                string noteDirectory = Path.GetDirectoryName(Path.GetFullPath(notePath));
                if (!string.IsNullOrEmpty(noteDirectory) && !Directory.Exists(noteDirectory))
                {
                    Directory.CreateDirectory(noteDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                return;
            }

            // Create a note (StickyNote) and attach the image
            try
            {
                using (MapiMessage note = new MapiMessage("", "", "Sample Note", "This is a note."))
                {
                    note.MessageClass = "IPM.StickyNote";

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

                    note.Attachments.Add(Path.GetFileName(imagePath), imageData);
                    note.Save(notePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or save the note: {ex.Message}");
                return;
            }

            // Reopen the note and verify the attachment is embedded
            try
            {
                using (MapiMessage loadedNote = MapiMessage.Load(notePath))
                {
                    if (loadedNote.Attachments.Count > 0)
                    {
                        foreach (MapiAttachment attachment in loadedNote.Attachments)
                        {
                            Console.WriteLine($"Embedded attachment found: {attachment.FileName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No attachments were found in the reopened note.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or verify the note: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

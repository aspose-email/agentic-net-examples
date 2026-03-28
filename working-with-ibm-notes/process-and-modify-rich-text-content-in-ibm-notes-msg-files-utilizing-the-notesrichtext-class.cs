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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com"))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Created placeholder MSG file at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Access the RTF body.
                string rtfContent = msg.BodyRtf ?? string.Empty;

                if (string.IsNullOrEmpty(rtfContent))
                {
                    Console.WriteLine("The message does not contain RTF content.");
                    return;
                }

                // Example modification: replace the word "old" with "new" in the RTF.
                string modifiedRtf = rtfContent.Replace("old", "new");

                // Update the message's RTF body.
                msg.BodyRtf = modifiedRtf;

                // Save the modified message back to the same file.
                try
                {
                    msg.Save(msgPath);
                    Console.WriteLine("Rich text content has been modified and saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

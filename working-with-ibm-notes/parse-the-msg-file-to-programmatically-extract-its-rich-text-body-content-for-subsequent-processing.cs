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
            string msgPath = "input.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject",
                        "Placeholder body text."))
                    {
                        placeholder.Save(msgPath);
                    }

                    Console.Error.WriteLine($"Input MSG file not found. Created placeholder at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract the rich‑text (RTF) body.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    string rtfBody = message.BodyRtf ?? string.Empty;

                    // Subsequent processing of the rich‑text content can be done here.
                    Console.WriteLine("Rich‑Text Body (RTF):");
                    Console.WriteLine(rtfBody);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

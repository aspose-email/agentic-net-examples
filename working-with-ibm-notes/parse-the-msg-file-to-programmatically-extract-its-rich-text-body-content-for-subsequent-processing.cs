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

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Extract the rich‑text (RTF) body
                string rtfBody = msg.BodyRtf ?? string.Empty;

                // Example processing: output the length of the RTF content
                Console.WriteLine($"RTF body length: {rtfBody.Length}");

                // Additional processing of rtfBody can be performed here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

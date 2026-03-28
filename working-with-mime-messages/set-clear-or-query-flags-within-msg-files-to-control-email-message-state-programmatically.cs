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
            // Define input MSG file path
            string dataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
            string inputFile = Path.Combine(dataDirectory, "sample.msg");

            // Verify that the input file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Error: File not found – {inputFile}");
                return;
            }

            // Load the MSG file, modify its flags, and save it back
            using (MapiMessage message = MapiMessage.Load(inputFile))
            {
                // Set the message as read by applying the MSGFLAG_READ flag
                message.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);

                // Save the updated message (overwrites the original file)
                message.Save(inputFile);
            }

            Console.WriteLine("Message flags updated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

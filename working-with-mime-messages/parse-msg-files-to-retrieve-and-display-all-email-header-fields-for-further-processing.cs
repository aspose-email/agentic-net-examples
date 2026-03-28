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
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Retrieve the raw transport headers as a single string
                string rawHeaders = msg.TransportMessageHeaders;

                if (string.IsNullOrEmpty(rawHeaders))
                {
                    Console.WriteLine("No transport headers found in the MSG file.");
                    return;
                }

                Console.WriteLine("Transport Message Headers:");
                // Split the headers into individual lines for display
                string[] headerLines = rawHeaders.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in headerLines)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

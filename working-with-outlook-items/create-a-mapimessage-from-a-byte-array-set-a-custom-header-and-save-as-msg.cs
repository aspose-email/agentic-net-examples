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
            // Example byte array containing MSG data.
            // Replace with actual message bytes as needed.
            byte[] msgBytes = new byte[] { /* message bytes */ };

            // Define output file path.
            string outputPath = "output.msg";

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MapiMessage from the byte array.
            using (MemoryStream memoryStream = new MemoryStream(msgBytes))
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(memoryStream))
                {
                    // Add a custom header.
                    mapiMessage.Headers.Add("X-Custom-Header", "MyValue");

                    // Save the message as MSG.
                    try
                    {
                        mapiMessage.Save(outputPath);
                        Console.WriteLine("Message saved to " + outputPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine("Error saving message: " + saveEx.Message);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}

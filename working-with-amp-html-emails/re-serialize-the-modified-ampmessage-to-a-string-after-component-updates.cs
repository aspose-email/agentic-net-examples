using System;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic properties
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text body.";
                ampMessage.IsBodyHtml = false;

                // Set the AMP HTML body (the AMP component)
                ampMessage.AmpHtmlBody = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello, AMP!</h1></body></html>";

                // Re‑serialize the modified message to a string
                string serializedMessage = ampMessage.ToString();

                // Output the serialized string
                Console.WriteLine("Serialized AMP Message:");
                Console.WriteLine(serializedMessage);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

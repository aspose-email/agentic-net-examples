using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder Subject",
                    "Placeholder Body"))
                {
                    placeholder.Save(msgPath);
                    Console.WriteLine($"Placeholder MSG created at: {msgPath}");
                }
            }

            // Attempt to load the message and handle any read errors.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    // Additional processing can be added here.
                }
            }
            catch (AsposeException ex)
            {
                Console.Error.WriteLine($"Failed to read message: {ex.Message}");
                if (ex.ErrorDetails != null)
                {
                    Console.Error.WriteLine($"Error details: {ex.ErrorDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while loading message: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}

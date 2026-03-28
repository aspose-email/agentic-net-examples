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
            // Path to the MSG file exported from IBM Notes
            string msgFilePath = "NotesMessage.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                // Example processing: display basic properties
                Console.WriteLine($"Subject: {mapiMessage.Subject}");
                Console.WriteLine($"From: {mapiMessage.SenderName}");
                Console.WriteLine($"Body: {mapiMessage.Body}");

                // Additional processing of the message can be performed here
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

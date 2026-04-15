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
            // Path to the MSG file to be validated
            string msgFilePath = "sample.msg";

            // Verify that the file exists before attempting any operation
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Ensure the file has a valid MSG format
            bool isMsgFormat;
            try
            {
                isMsgFormat = MapiMessage.IsMsgFormat(msgFilePath);
            }
            catch (Exception formatEx)
            {
                Console.Error.WriteLine($"Failed to determine MSG format: {formatEx.Message}");
                return;
            }

            if (!isMsgFormat)
            {
                Console.Error.WriteLine("The specified file is not a valid MSG format.");
                return;
            }

            // Attempt to load the MSG file; any corruption will raise an exception
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Basic integrity check: ensure essential properties are present
                if (string.IsNullOrEmpty(message.Subject))
                {
                    Console.WriteLine("Message loaded, but the Subject is empty – the file may be corrupted.");
                }
                else
                {
                    Console.WriteLine($"Message loaded successfully. Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors and report them without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

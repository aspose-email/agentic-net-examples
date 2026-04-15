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
            // Define the folder that will contain the message files
            string folderPath = "Emails";

            // Ensure the directory exists; create it if it does not
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Ensure there is at least one placeholder MSG file to avoid empty input
            string placeholderPath = Path.Combine(folderPath, "placeholder.msg");
            if (!File.Exists(placeholderPath))
            {
                // Create a minimal MAPI message and save it as a .msg file
                using (MapiMessage msg = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder",
                    "This is a placeholder message."))
                {
                    msg.Save(placeholderPath);
                }
            }

            // Retrieve the list of files in the directory
            string[] files;
            try
            {
                files = Directory.GetFiles(folderPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving files: {ex.Message}");
                return;
            }

            // Iterate through each file using a foreach loop
            foreach (string file in files)
            {
                Console.WriteLine($"Found file: {file}");

                // Attempt to load the message and display its subject
                try
                {
                    using (MapiMessage message = MapiMessage.Load(file))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load '{file}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}

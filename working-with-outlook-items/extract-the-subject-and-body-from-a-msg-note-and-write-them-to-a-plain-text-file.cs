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
            // Paths for input MSG file and output text file
            string inputMsgPath = "input.msg";
            string outputTxtPath = "output.txt";

            // Verify that the input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputTxtPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file and extract subject and body
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                string subject = msg.Subject ?? string.Empty;
                string body = msg.Body ?? string.Empty;

                // Write the extracted information to a plain text file
                using (StreamWriter writer = new StreamWriter(outputTxtPath, false))
                {
                    writer.WriteLine("Subject: " + subject);
                    writer.WriteLine();
                    writer.WriteLine(body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

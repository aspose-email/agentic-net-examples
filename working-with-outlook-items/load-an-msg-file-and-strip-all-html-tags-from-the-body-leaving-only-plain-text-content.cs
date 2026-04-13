using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputMsgPath = "input.msg";
            string outputTxtPath = "output.txt";

            // Verify that the input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputTxtPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file with load options
            using (MailMessage message = MailMessage.Load(inputMsgPath, new MsgLoadOptions()))
            {
                // Retrieve plain‑text body. If the body is HTML, GetHtmlBodyText parses it.
                string plainTextBody = message.IsBodyHtml
                    ? message.GetHtmlBodyText(true)
                    : message.Body;

                // Save the plain‑text body to a text file
                using (StreamWriter writer = new StreamWriter(outputTxtPath, false))
                {
                    writer.Write(plainTextBody);
                }
            }

            Console.WriteLine($"Plain‑text body saved to: {outputTxtPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

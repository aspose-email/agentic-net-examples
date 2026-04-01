using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputHtmlPath = "amp_body.html";

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file into an AmpMessage
            using (FileStream inputStream = new FileStream(inputMsgPath, FileMode.Open, FileAccess.Read))
            using (AmpMessage ampMessage = new AmpMessage())
            {
                try
                {
                    ampMessage.Import(inputStream);
                }
                catch (Exception importEx)
                {
                    Console.Error.WriteLine($"Failed to import MSG file: {importEx.Message}");
                    return;
                }

                // Access the AMP HTML body
                string ampHtmlBody = ampMessage.AmpHtmlBody;
                if (string.IsNullOrEmpty(ampHtmlBody))
                {
                    Console.WriteLine("The message does not contain AMP content.");
                }
                else
                {
                    Console.WriteLine("AMP HTML Body:");
                    Console.WriteLine(ampHtmlBody);

                    // Save AMP HTML to a file
                    try
                    {
                        File.WriteAllText(outputHtmlPath, ampHtmlBody);
                        Console.WriteLine($"AMP HTML body saved to: {outputHtmlPath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Failed to write AMP HTML to file: {writeEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

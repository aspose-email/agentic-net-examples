using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "input.msg";
            // Output RTF file path
            string outputRtfPath = "output.rtf";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputRtfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {outputDirectory}");
                    Console.Error.WriteLine(dirEx.Message);
                    return;
                }
            }

            // Load the MSG file and extract RTF body
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                string rtfBody = msg.BodyRtf;
                if (string.IsNullOrEmpty(rtfBody))
                {
                    Console.Error.WriteLine("Error: No RTF body found in the MSG file.");
                    return;
                }

                // Write RTF content to file
                try
                {
                    using (FileStream fs = new FileStream(outputRtfPath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] rtfBytes = Encoding.UTF8.GetBytes(rtfBody);
                        fs.Write(rtfBytes, 0, rtfBytes.Length);
                    }
                    Console.WriteLine($"RTF file saved successfully to {outputRtfPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error: Failed to write RTF file – {outputRtfPath}");
                    Console.Error.WriteLine(writeEx.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

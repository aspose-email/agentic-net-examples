using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output MSG file paths
                string inputMsgPath = "input.msg";
                string outputMsgPath = "output.msg";

                // Verify that the input file exists
                if (!File.Exists(inputMsgPath))
                {
                    Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                    return;
                }

                // Load the MSG file, preserve all properties, and save it back
                using (MapiMessage message = MapiMessage.Load(inputMsgPath))
                {
                    // Ensure the directory for the output file exists
                    string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Save the message to a new MSG file
                    message.Save(outputMsgPath);
                }

                Console.WriteLine($"Message successfully copied from '{inputMsgPath}' to '{outputMsgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

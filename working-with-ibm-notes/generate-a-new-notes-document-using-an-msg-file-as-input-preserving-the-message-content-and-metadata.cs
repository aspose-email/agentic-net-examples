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
            // Input MSG file path
            string inputMsgPath = "input.msg";
            // Output MSG file path
            string outputMsgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage
            MapiMessage sourceMessage;
            try
            {
                sourceMessage = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error: Failed to load MSG file – {loadEx.Message}");
                return;
            }

            // Clone the message to preserve content and metadata
            MapiMessage notesMessage = sourceMessage.Clone() as MapiMessage;
            if (notesMessage == null)
            {
                Console.Error.WriteLine("Error: Failed to clone the message.");
                return;
            }

            // Save the cloned message as a new MSG file (acts as a Notes document)
            try
            {
                notesMessage.Save(outputMsgPath);
                Console.WriteLine($"Notes document created successfully at: {outputMsgPath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Error: Failed to save MSG file – {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output MSG file paths
                string inputMsgPath = "input.msg";
                string outputMsgPath = "output.msg";

                // Guard input file existence
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

                    Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the MSG file inside a using block to ensure disposal
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                {
                    // Query current flags
                    MapiMessageFlags currentFlags = msg.Flags;
                    Console.WriteLine($"Current Flags: {currentFlags}");

                    // Set the Read flag
                    msg.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);
                    Console.WriteLine("Set MSGFLAG_READ flag.");

                    // Clear the Read flag (remove it from the flag set)
                    MapiMessageFlags clearedFlags = msg.Flags & ~MapiMessageFlags.MSGFLAG_READ;
                    msg.SetMessageFlags(clearedFlags);
                    Console.WriteLine("Cleared MSGFLAG_READ flag.");

                    // Save the modified message
                    msg.Save(outputMsgPath);
                    Console.WriteLine($"Modified MSG saved to {outputMsgPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception: {ex.Message}");
                // Graceful exit without rethrowing
            }
        }
    }
}

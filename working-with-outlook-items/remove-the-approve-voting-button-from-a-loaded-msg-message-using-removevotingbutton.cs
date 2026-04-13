using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace RemoveVotingButtonSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.msg";
                string outputPath = "output.msg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the MSG file, remove the "Approve" voting button, and save
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    FollowUpManager.RemoveVotingButton(message, "Approve");
                    message.Save(outputPath);
                }

                Console.WriteLine("Voting button removed and message saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

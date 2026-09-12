using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailRecipientModification
{
    // Author: Aspose.Email example for modifying MSG recipients
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output MSG file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Guard against missing input file
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the MSG file
                MapiMessage msg = MapiMessage.Load(inputPath);

                // ----- Modify existing recipients -----
                // Example: update the first recipient's display name and email address
                if (msg.Recipients.Count > 0)
                {
                    MapiRecipient firstRecipient = msg.Recipients[0];
                    firstRecipient.DisplayName = "Updated Name";
                    firstRecipient.EmailAddress = "updated@example.com";
                }

                // ----- Add a new TO recipient -----
                // Use the Add overload that accepts address, display name, and MAPI_TO type
                msg.Recipients.Add("newto@example.com", "New To Recipient", MapiRecipientType.MAPI_TO);

                // ----- Remove a specific recipient -----
                // Remove any recipient whose email address matches the target
                for (int i = msg.Recipients.Count - 1; i >= 0; i--)
                {
                    if (msg.Recipients[i].EmailAddress.Equals("remove@example.com", StringComparison.OrdinalIgnoreCase))
                    {
                        msg.Recipients.RemoveAt(i);
                    }
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified MSG file
                msg.Save(outputPath);
            }
            catch (Exception ex)
            {
                // Report any errors without throwing
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
            }
        }
    }
}

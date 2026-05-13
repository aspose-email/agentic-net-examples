using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string vcardFilePath = "contacts.vcf";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(vcardFilePath))
            {
                try
                {
                    using (var writer = new StreamWriter(vcardFilePath))
                    {
                        writer.WriteLine("BEGIN:VCARD");
                        writer.WriteLine("VERSION:3.0");
                        writer.WriteLine("FN:Placeholder Contact");
                        writer.WriteLine("END:VCARD");
                    }
                    Console.WriteLine($"Placeholder vCard file created at '{vcardFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder vCard file: {ex.Message}");
                    return;
                }
            }

            // Read the entire vCard file.
            string[] allLines;
            try
            {
                allLines = File.ReadAllLines(vcardFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read vCard file: {ex.Message}");
                return;
            }

            // Parse individual vCard entries.
            var vcardBlocks = new List<List<string>>();
            List<string> currentBlock = null;

            foreach (var line in allLines)
            {
                if (line.Trim().Equals("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                {
                    currentBlock = new List<string>();
                    vcardBlocks.Add(currentBlock);
                }

                if (currentBlock != null)
                {
                    currentBlock.Add(line);
                }

                if (line.Trim().Equals("END:VCARD", StringComparison.OrdinalIgnoreCase))
                {
                    currentBlock = null;
                }
            }

            if (vcardBlocks.Count == 0)
            {
                Console.WriteLine("No contacts found in the vCard file.");
                return;
            }

            // Prepare output directory for individual vCard files.
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int index = 1;
            foreach (var block in vcardBlocks)
            {
                Console.WriteLine($"Contact #{index}:");

                // Extract and display fields.
                foreach (var line in block)
                {
                    if (line.StartsWith("FN:", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"  Full Name: {line.Substring(3)}");
                    }
                    else if (line.StartsWith("EMAIL", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(':');
                        if (parts.Length > 1)
                        {
                            Console.WriteLine($"  Email: {parts[1]}");
                        }
                    }
                    else if (line.StartsWith("TEL", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(':');
                        if (parts.Length > 1)
                        {
                            Console.WriteLine($"  Phone: {parts[1]}");
                        }
                    }
                }

                // Save each contact as an individual vCard file.
                string individualPath = Path.Combine(outputDir, $"contact_{index}.vcf");
                try
                {
                    File.WriteAllLines(individualPath, block);
                    Console.WriteLine($"  Saved individual vCard to '{individualPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"  Failed to save individual vCard: {ex.Message}");
                }

                index++;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

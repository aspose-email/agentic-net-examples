using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string csvPath = "distributionlist.csv";

            // Ensure the CSV file exists; create a minimal placeholder if missing.
            if (!File.Exists(csvPath))
            {
                try
                {
                    string placeholder = "Name,Email\nJohn Doe,john@example.com";
                    File.WriteAllText(csvPath, placeholder);
                    Console.WriteLine($"Placeholder CSV created at '{csvPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            List<string> emailList = new List<string>();

            try
            {
                using (FileStream fileStream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    // Read header line (if any) and ignore.
                    string headerLine = reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split by commas; trim whitespace.
                        string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string part in parts)
                        {
                            string trimmed = part.Trim();
                            if (trimmed.Contains("@"))
                            {
                                emailList.Add(trimmed);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV file: {ex.Message}");
                return;
            }

            // Output the collected email addresses.
            Console.WriteLine("Parsed email addresses:");
            foreach (string email in emailList)
            {
                Console.WriteLine(email);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

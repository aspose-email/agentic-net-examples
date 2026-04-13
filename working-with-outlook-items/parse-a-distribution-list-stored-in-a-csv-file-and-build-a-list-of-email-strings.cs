using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            string csvPath = "distributionlist.csv";

            // Ensure the CSV file exists; create a minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    File.WriteAllText(csvPath, "Email\r\nexample@example.com\r\n");
                    Console.WriteLine($"Placeholder CSV created at {csvPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            List<string> emailList = new List<string>();

            // Read and parse the CSV file
            try
            {
                using (StreamReader reader = new StreamReader(csvPath))
                {
                    string line;
                    bool isFirstLine = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            // Assume first line is a header
                            isFirstLine = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Split by comma and take the first column as the email address
                        string[] parts = line.Split(',');
                        if (parts.Length > 0)
                        {
                            string email = parts[0].Trim();
                            if (!string.IsNullOrEmpty(email))
                            {
                                emailList.Add(email);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV: {ex.Message}");
                return;
            }

            // Output the parsed email addresses
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

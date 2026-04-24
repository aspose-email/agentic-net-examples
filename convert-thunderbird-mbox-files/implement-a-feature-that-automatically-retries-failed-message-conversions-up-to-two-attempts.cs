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
            // Define input and output paths
            string inputPath = "input.msg";
            string outputPath = "output.eml";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
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

            const int maxAttempts = 3; // initial attempt + two retries
            int attempt = 0;
            bool conversionSucceeded = false;

            while (attempt < maxAttempts && !conversionSucceeded)
            {
                attempt++;
                try
                {
                    // Load the MAPI message from file
                    using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
                    {
                        // Configure conversion options as needed
                        MailConversionOptions conversionOptions = new MailConversionOptions();

                        // Convert to MailMessage
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                        {
                            // Save the resulting MailMessage
                            mailMessage.Save(outputPath);
                        }
                    }

                    conversionSucceeded = true;
                }
                catch (AsposeException aspEx)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed: {aspEx.Message}");
                    if (attempt >= maxAttempts)
                    {
                        Console.Error.WriteLine("All conversion attempts have failed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Attempt {attempt} encountered an unexpected error: {ex.Message}");
                    if (attempt >= maxAttempts)
                    {
                        Console.Error.WriteLine("All conversion attempts have failed.");
                    }
                }
            }
        }
        catch (Exception unexpected)
        {
            Console.Error.WriteLine($"Unexpected error: {unexpected.Message}");
        }
    }
}

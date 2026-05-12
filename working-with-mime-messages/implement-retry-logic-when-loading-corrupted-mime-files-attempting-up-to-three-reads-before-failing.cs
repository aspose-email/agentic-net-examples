using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MIME (EML) file
            string mimeFilePath = "sample.eml";

            // Verify that the input file exists before attempting to read it
            if (!File.Exists(mimeFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(mimeFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {mimeFilePath}");
                return;
            }

            const int maxAttempts = 3;
            int attempt = 0;
            bool loadedSuccessfully = false;

            while (attempt < maxAttempts && !loadedSuccessfully)
            {
                try
                {
                    // Attempt to load the MIME file
                    using (MailMessage message = MailMessage.Load(mimeFilePath))
                    {
                        // Example processing: output the subject
                        Console.WriteLine($"Subject: {message.Subject}");
                        loadedSuccessfully = true;
                    }
                }
                catch (AsposeException ex)
                {
                    attempt++;
                    if (attempt >= maxAttempts)
                    {
                        Console.Error.WriteLine($"Failed to load MIME file after {maxAttempts} attempts: {ex.Message}");
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying...");
                    }
                }
                catch (Exception ex)
                {
                    // Catch any other unexpected exceptions during loading
                    Console.Error.WriteLine($"Unexpected error on attempt {attempt + 1}: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level guard: report any unhandled exceptions
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}

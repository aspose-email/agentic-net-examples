using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholderMessage = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholderMessage.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email message: {ex.Message}");
                return;
            }

            using (message)
            {
                // Normalize addresses in To, CC, and Bcc collections
                NormalizeAddressCollection(message.To);
                NormalizeAddressCollection(message.CC);
                NormalizeAddressCollection(message.Bcc);

                // Save the normalized message
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save normalized email message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper method to normalize a MailAddressCollection
    private static void NormalizeAddressCollection(MailAddressCollection collection)
    {
        if (collection == null || collection.Count == 0)
            return;

        List<string> normalizedAddresses = new List<string>();
        foreach (MailAddress address in collection)
        {
            // Convert to lower case and strip display name by using the address part only
            string normalized = address.Address?.ToLowerInvariant() ?? string.Empty;
            if (!string.IsNullOrEmpty(normalized))
                normalizedAddresses.Add(normalized);
        }

        collection.Clear();
        foreach (string addr in normalizedAddresses)
        {
            collection.Add(new MailAddress(addr));
        }
    }
}

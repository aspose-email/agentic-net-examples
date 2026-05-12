using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string body = message.Body ?? string.Empty;
                string[] words = body.Split(new char[] { ' ', '\r', '\n', '\t', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                int totalWords = words.Length;

                string[] spamKeywords = new string[] { "free", "winner", "prize", "click", "offer" };
                int keywordCount = 0;
                foreach (string word in words)
                {
                    foreach (string keyword in spamKeywords)
                    {
                        if (string.Equals(word, keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            keywordCount++;
                        }
                    }
                }

                double score = totalWords > 0 ? ((double)keywordCount / totalWords) * 10.0 : 0.0;
                string scoreString = score.ToString("F2");

                // Add custom X‑Spam‑Score header
                message.Headers.Add("X-Spam-Score", scoreString);

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the modified message
                message.Save(outputPath);
                Console.WriteLine($"Message saved with X-Spam-Score header: {scoreString}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

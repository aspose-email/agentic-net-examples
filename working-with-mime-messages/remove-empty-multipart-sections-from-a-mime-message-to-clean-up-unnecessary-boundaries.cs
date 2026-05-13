using System;
using System.Collections.Generic;
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

            // Ensure the input file exists; create a minimal placeholder if it does not.
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

                using (MailMessage placeholder = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                {
                    placeholder.Save(inputPath);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputPath}'.");
                return;
            }

            // Load the MIME message.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Collect empty alternate views (multipart sections) to remove.
                List<AlternateView> emptyViews = new List<AlternateView>();
                foreach (AlternateView view in message.AlternateViews)
                {
                    if (view.ContentStream == null || view.ContentStream.Length == 0)
                    {
                        emptyViews.Add(view);
                    }
                }

                // Remove the empty views.
                foreach (AlternateView emptyView in emptyViews)
                {
                    message.AlternateViews.Remove(emptyView);
                }

                // Save the cleaned message.
                message.Save(outputPath);
                Console.WriteLine($"Cleaned message saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

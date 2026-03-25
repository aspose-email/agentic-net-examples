using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
                {
                    // Convert to a regular MailMessage
                    MailConversionOptions conversionOptions = new MailConversionOptions();
                    MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions);

                    // Create an AMP message and copy basic properties
                    using (AmpMessage ampMessage = new AmpMessage())
                    {
                        ampMessage.From = mailMessage.From;
                        ampMessage.To.AddRange(mailMessage.To);
                        ampMessage.Subject = mailMessage.Subject;

                        // Set AMP HTML body (the AMP component)
                        ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                        // Provide a fallback HTML body
                        ampMessage.HtmlBody = "<p>Hello fallback</p>";

                        // Save the AMP message as an MSG file
                        try
                        {
                            ampMessage.Save(outputPath);
                            Console.WriteLine($"AMP message saved to: {outputPath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Error saving AMP message: {saveEx.Message}");
                        }
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading MSG file: {loadEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            try
            {
                using (FileStream fileStream = new FileStream(msgPath, FileMode.Open, FileAccess.Read))
                {
                    using (AmpMessage ampMessage = new AmpMessage())
                    {
                        ampMessage.Import(fileStream);

                        // Basic properties
                        Console.WriteLine("Subject: " + ampMessage.Subject);
                        Console.WriteLine("From: " + (ampMessage.From != null ? ampMessage.From.ToString() : "N/A"));
                        Console.WriteLine("To: " + (ampMessage.To != null ? ampMessage.To.ToString() : "N/A"));

                        // AMP HTML body
                        Console.WriteLine("AMP HTML Body:");
                        Console.WriteLine(ampMessage.AmpHtmlBody ?? "[None]");

                        // Alternate views (e.g., plain text, HTML)
                        if (ampMessage.AlternateViews != null && ampMessage.AlternateViews.Count > 0)
                        {
                            Console.WriteLine("Alternate Views:");
                            foreach (AlternateView view in ampMessage.AlternateViews)
                            {
                                string content = ampMessage.GetAlternateViewContent(view.ContentId);
                                Console.WriteLine($"- Content ID: {view.ContentId}");
                                Console.WriteLine(content);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No alternate views found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
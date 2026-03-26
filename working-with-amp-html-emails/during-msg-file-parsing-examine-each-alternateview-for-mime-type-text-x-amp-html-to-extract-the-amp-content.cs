using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the file exists before attempting to read it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Open the file stream and load the message into an AmpMessage instance
            using (FileStream fileStream = File.OpenRead(msgPath))
            {
                using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
                {
                    ampMessage.Import(fileStream);

                    // Iterate through each AlternateView and extract AMP content
                    foreach (Aspose.Email.AlternateView view in ampMessage.AlternateViews)
                    {
                        if (view.ContentType != null &&
                            string.Equals(view.ContentType.MediaType, "text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                        {
                            // Retrieve the content of the AMP view
                            string ampContent = ampMessage.GetAlternateViewContent(view.ContentType.MediaType);
                            Console.WriteLine("AMP Content:");
                            Console.WriteLine(ampContent);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
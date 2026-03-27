using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (FileStream fileStream = File.OpenRead(msgPath))
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Load the MSG file into the AmpMessage instance
                ampMessage.Import(fileStream);

                // Iterate through all alternate views and extract AMP content
                foreach (AlternateView view in ampMessage.AlternateViews)
                {
                    if (view.ContentType != null &&
                        string.Equals(view.ContentType.MediaType, "text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                    {
                        string ampContent = ampMessage.GetAlternateViewContent(view.ContentType.MediaType);
                        Console.WriteLine("AMP Content:");
                        Console.WriteLine(ampContent);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

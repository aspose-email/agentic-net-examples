using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

namespace MsgAmpExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgPath = "sample.msg";

                // Verify that the file exists before attempting to load it
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                // Load the MSG file into a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    // Convert the MapiMessage to a MailMessage to access AlternateViews
                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                    {
                        // Iterate through each AlternateView
                        foreach (AlternateView alternateView in mailMessage.AlternateViews)
                        {
                            // Check for the AMP MIME type
                            if (alternateView.ContentType != null &&
                                alternateView.ContentType.MediaType.Equals("text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                            {
                                // Extract the AMP content as a string
                                string ampContent = mailMessage.GetAlternateViewContent(alternateView.ContentType.MediaType);
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
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}

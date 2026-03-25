using System;
using System.IO;
using Aspose.Email;
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
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(msgPath))
            {
                bool found = false;
                foreach (AlternateView view in message.AlternateViews)
                {
                    if (view.ContentType != null && string.Equals(view.ContentType.MediaType, "text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                    {
                        string ampContent = message.GetAlternateViewContent(view.ContentType.MediaType);
                        Console.WriteLine("AMP Content:");
                        Console.WriteLine(ampContent);
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No AMP alternate view found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
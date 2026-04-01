using System;
using System.IO;
using System.Text;
using System.Net.Mime;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                MailConversionOptions conversionOptions = new MailConversionOptions();

                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    bool ampFound = false;

                    foreach (AlternateView view in mailMessage.AlternateViews)
                    {
                        if (view.ContentType != null &&
                            string.Equals(view.ContentType.MediaType, "text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                        {
                            using (StreamReader reader = new StreamReader(view.ContentStream, Encoding.UTF8, true))
                            {
                                string ampContent = reader.ReadToEnd();
                                Console.WriteLine("AMP Content:");
                                Console.WriteLine(ampContent);
                                ampFound = true;
                            }
                        }
                    }

                    if (!ampFound)
                    {
                        Console.WriteLine("No AMP content found in the message.");
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

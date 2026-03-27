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
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine("Input file not found: " + msgPath);
                return;
            }

            using (FileStream fileStream = new FileStream(msgPath, FileMode.Open, FileAccess.Read))
            {
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.Import(fileStream);

                    foreach (AlternateView view in ampMessage.AlternateViews)
                    {
                        if (view.ContentType != null &&
                            view.ContentType.MediaType.Equals("text/x-amp-html", StringComparison.OrdinalIgnoreCase))
                        {
                            string ampContent = ampMessage.GetAlternateViewContent("text/x-amp-html");
                            Console.WriteLine("AMP Content:");
                            Console.WriteLine(ampContent);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred: " + ex.Message);
        }
    }
}

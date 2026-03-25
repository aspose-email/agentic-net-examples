using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Open the MSG file stream inside a try/catch to handle I/O errors
            try
            {
                using (FileStream fileStream = new FileStream(msgPath, FileMode.Open, FileAccess.Read))
                {
                    // Load the AMP message from the stream
                    using (AmpMessage ampMessage = new AmpMessage())
                    {
                        ampMessage.Import(fileStream);

                        // Display the AMP HTML body if present
                        Console.WriteLine("AMP HTML Body:");
                        Console.WriteLine(ampMessage.AmpHtmlBody ?? "(none)");

                        // Iterate through alternate views (e.g., plain text, HTML)
                        foreach (AlternateView view in ampMessage.AlternateViews)
                        {
                            // Show the content type of the alternate view
                            Console.WriteLine();
                            Console.WriteLine("Alternate View Content-Type: " + view.ContentType.MediaType);

                            // Read the content of the alternate view
                            if (view.ContentStream != null)
                            {
                                using (MemoryStream memory = new MemoryStream())
                                {
                                    view.ContentStream.Position = 0;
                                    view.ContentStream.CopyTo(memory);
                                    string viewContent = Encoding.UTF8.GetString(memory.ToArray());
                                    Console.WriteLine("Content:");
                                    Console.WriteLine(viewContent);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No content stream available for this view.");
                            }
                        }
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("Error processing the MSG file: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
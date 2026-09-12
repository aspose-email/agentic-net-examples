using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input MSG file path (replace with a real path)
                string msgPath = "sample.msg";

                // Guard against placeholder paths
                if (msgPath.Contains("example") || msgPath.Contains("sample"))
                {
                    Console.Error.WriteLine("Please provide a valid MSG file path before running the example.");
                    return;
                }

                // Ensure the MSG file exists; create a placeholder if it does not
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
                        Console.WriteLine($"Placeholder MSG file created at: {msgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                        return;
                    }
                }

                // Load the MSG file
                MapiMessage mapMsg;
                try
                {
                    mapMsg = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                    return;
                }

                // Convert MapiMessage to MailMessage
                MailMessage mailMsg;
                try
                {
                    MailConversionOptions conversionOpts = new MailConversionOptions();
                    mailMsg = mapMsg.ToMailMessage(conversionOpts);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting MSG to MailMessage: {ex.Message}");
                    return;
                }

                // Define output EML file path
                string emlPath = Path.ChangeExtension(msgPath, ".eml");

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(emlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the MailMessage as EML
                try
                {
                    using (mailMsg) // MailMessage implements IDisposable
                    {
                        mailMsg.Save(emlPath);
                    }
                    Console.WriteLine($"Successfully saved EML file to: {emlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving EML file: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

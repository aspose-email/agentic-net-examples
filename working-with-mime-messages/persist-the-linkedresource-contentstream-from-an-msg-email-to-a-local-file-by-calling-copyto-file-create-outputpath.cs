using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.msg";
            string outputDirectory = "output";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                int resourceIndex = 0;
                foreach (LinkedResource resource in message.LinkedResources)
                {
                    // Determine output file name (preserve original extension if possible)
                    string extension = Path.GetExtension(resource.ContentType.MediaType);
                    if (string.IsNullOrEmpty(extension))
                    {
                        extension = ".bin";
                    }
                    string outputPath = Path.Combine(outputDirectory, $"resource_{resourceIndex}{extension}");

                    // Copy the content stream to a file
                    Stream contentStream = resource.ContentStream;
                    if (contentStream.CanSeek)
                    {
                        contentStream.Position = 0;
                    }

                    using (FileStream fileStream = File.Create(outputPath))
                    {
                        contentStream.CopyTo(fileStream);
                    }

                    resourceIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

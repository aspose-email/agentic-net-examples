using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search;

namespace EmailConversionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect two arguments: source file path and destination MSG file path
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: EmailConversionSample <sourceFile> <outputMsgFile>");
                return;
            }

            string sourcePath = args[0];
            string outputPath = args[1];

            // Guard input file existence
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Input file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                ConvertToMsg(sourcePath, outputPath);
                Console.WriteLine($"Conversion succeeded: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }

        private static void ConvertToMsg(string inputFile, string outputFile)
        {
            // Determine load options based on file extension
            string ext = Path.GetExtension(inputFile).ToLowerInvariant();
            LoadOptions loadOptions = GetLoadOptions(ext);

            // Load the message with the appropriate options
            using (MailMessage message = MailMessage.Load(inputFile, loadOptions))
            {
                // Prepare MSG save options (Unicode format, preserve original dates)
                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save as MSG
                message.Save(outputFile, msgSaveOptions);
            }
        }

        private static LoadOptions GetLoadOptions(string extension)
        {
            switch (extension)
            {
                case ".eml":
                    return new EmlLoadOptions
                    {
                        PreserveTnefAttachments = true,
                        PreserveEmbeddedMessageFormat = true
                    };
                case ".msg":
                    return new MsgLoadOptions();
                case ".mhtml":
                case ".mht":
                    return new MhtmlLoadOptions();
                case ".html":
                case ".htm":
                    return new HtmlLoadOptions
                    {
                        ShouldAddPlainTextView = true
                    };
                default:
                    // Default load options (no special handling)
                    return null;
            }
        }
    }
}

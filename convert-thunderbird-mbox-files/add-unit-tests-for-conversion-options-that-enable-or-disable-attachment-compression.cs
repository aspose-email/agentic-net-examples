using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Simple unit‑style tests without external test framework.
            Console.WriteLine("Starting conversion options tests...");

            // Ensure a sample EML file exists.
            const string sampleEml = "sample.eml";
            if (!File.Exists(sampleEml))
            {
                // Create a minimal EML message as placeholder.
                MailMessage placeholder = new MailMessage
                {
                    From = "sender@example.com",
                    To = "receiver@example.com",
                    Subject = "Placeholder",
                    Body = "This is a placeholder EML file."
                };
                placeholder.Save(sampleEml);
                Console.WriteLine($"Created placeholder EML file: {sampleEml}");
            }

            // Test 1: Enable attachment (TNEF) preservation when loading EML.
            bool test1Result = TestAttachmentPreservation(sampleEml, true);
            Console.WriteLine($"Test EnableAttachmentPreservation: {(test1Result ? "Passed" : "Failed")}");

            // Test 2: Disable attachment (TNEF) preservation when loading EML.
            bool test2Result = TestAttachmentPreservation(sampleEml, false);
            Console.WriteLine($"Test DisableAttachmentPreservation: {(test2Result ? "Passed" : "Failed")}");

            // Test 3: Enable body compression option.
            bool test3Result = TestBodyCompressionOption(true);
            Console.WriteLine($"Test EnableBodyCompressionOption: {(test3Result ? "Passed" : "Failed")}");

            // Test 4: Disable body compression option.
            bool test4Result = TestBodyCompressionOption(false);
            Console.WriteLine($"Test DisableBodyCompressionOption: {(test4Result ? "Passed" : "Failed")}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Loads an EML with the specified PreserveTnefAttachments flag,
    // converts it to MSG and verifies that the save succeeds.
    static bool TestAttachmentPreservation(string emlPath, bool preserveTnef)
    {
        try
        {
            string outputMsg = preserveTnef ? "output_preserve.msg" : "output_no_preserve.msg";

            // Guard output path directory.
            string outputDir = Path.GetDirectoryName(outputMsg);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Initialize load options.
            EmlLoadOptions loadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = preserveTnef,
                PreserveEmbeddedMessageFormat = true
            };

            // Load the message with the options.
            using (MailMessage message = MailMessage.Load(emlPath, loadOptions))
            {
                // Save as MSG using default options.
                message.Save(outputMsg, SaveOptions.DefaultMsg);
            }

            // Verify the output file was created.
            return File.Exists(outputMsg);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Attachment preservation test error (preserve={preserveTnef}): {ex.Message}");
            return false;
        }
    }

    // Creates a MapiConversionOptions instance, sets UseBodyCompression,
    // and verifies the property reflects the intended value.
    static bool TestBodyCompressionOption(bool enableCompression)
    {
        try
        {
            MapiConversionOptions options = new MapiConversionOptions
            {
                UseBodyCompression = enableCompression
            };

            // Simple verification of the property value.
            return options.UseBodyCompression == enableCompression;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Body compression option test error (enable={enableCompression}): {ex.Message}");
            return false;
        }
    }
}

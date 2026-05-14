using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input Outlook MSG file path
            string msgPath = "sample.msg";

            // Output directory for extracted OLE objects
            string outputDir = "ExtractedOleObjects";

            // Guard input file existence
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Process the MSG file
            using (MapiMessageReader reader = new MapiMessageReader(msgPath))
            {
                using (MapiMessage message = reader.ReadMessage())
                {
                    MapiAttachmentCollection attachments = message.Attachments;
                    foreach (MapiAttachment attachment in attachments)
                    {
                        // Save attachment to a temporary file
                        string tempAttachmentPath = Path.Combine(Path.GetTempPath(), attachment.FileName);
                        try
                        {
                            attachment.Save(tempAttachmentPath);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {saveEx.Message}");
                            continue;
                        }

                        // Open the saved attachment and enumerate OLE objects
                        try
                        {
                            using (FileStream attachmentStream = File.OpenRead(tempAttachmentPath))
                            {
                                IDictionary<string, byte[]> oleFiles = InlineAttachmentExtractor.EnumerateMsoPackage(attachmentStream);
                                foreach (KeyValuePair<string, byte[]> kvp in oleFiles)
                                {
                                    string outFilePath = Path.Combine(outputDir, kvp.Key);
                                    try
                                    {
                                        File.WriteAllBytes(outFilePath, kvp.Value);
                                        Console.WriteLine($"Extracted OLE object to: {outFilePath}");
                                    }
                                    catch (Exception writeEx)
                                    {
                                        Console.Error.WriteLine($"Failed to write OLE object '{kvp.Key}': {writeEx.Message}");
                                    }
                                }
                            }
                        }
                        catch (Exception extractEx)
                        {
                            Console.Error.WriteLine($"Failed to extract OLE objects from attachment '{attachment.FileName}': {extractEx.Message}");
                        }
                        finally
                        {
                            // Clean up temporary attachment file
                            try
                            {
                                if (File.Exists(tempAttachmentPath))
                                {
                                    File.Delete(tempAttachmentPath);
                                }
                            }
                            catch
                            {
                                // Ignore cleanup errors
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

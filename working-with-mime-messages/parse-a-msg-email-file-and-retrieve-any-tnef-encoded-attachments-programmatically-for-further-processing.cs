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
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}");

                    if (attachment.FileName.Equals("winmail.dat", StringComparison.OrdinalIgnoreCase))
                    {
                        string tempTnefPath = Path.Combine(Path.GetTempPath(), $"tnef_{Guid.NewGuid()}.dat");

                        try
                        {
                            // Save the TNEF attachment to a temporary file
                            attachment.Save(tempTnefPath);

                            // Load the TNEF content as a MapiMessage
                            using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tempTnefPath))
                            {
                                foreach (MapiAttachment innerAttachment in tnefMessage.Attachments)
                                {
                                    Console.WriteLine($"  TNEF inner attachment: {innerAttachment.FileName}");

                                    // Example processing: save the inner attachment to the current directory
                                    string outputPath = Path.Combine(Directory.GetCurrentDirectory(), innerAttachment.FileName);
                                    innerAttachment.Save(outputPath);
                                    Console.WriteLine($"  Saved to: {outputPath}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing TNEF attachment: {ex.Message}");
                        }
                        finally
                        {
                            // Clean up the temporary file
                            if (File.Exists(tempTnefPath))
                            {
                                try
                                {
                                    File.Delete(tempTnefPath);
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

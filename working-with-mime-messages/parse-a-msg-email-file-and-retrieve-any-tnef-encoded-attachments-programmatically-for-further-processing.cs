using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailTnefExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Author note: Adjust the input file path as needed.
                string inputMsgPath = "sample.msg";

                // Verify input file exists.
                if (!File.Exists(inputMsgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                    return;
                }

                // Prepare output directory for extracted attachments.
                string outputDirectory = "ExtractedAttachments";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the MSG file.
                MapiMessage msg = MapiMessage.Load(inputMsgPath);

                // Iterate over attachments to find TNEF (winmail.dat) files.
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (attachment.FileName != null &&
                        attachment.FileName.Equals("winmail.dat", StringComparison.OrdinalIgnoreCase))
                    {
                        // Save the TNEF attachment to a temporary file.
                        string tempTnefPath = Path.Combine(Path.GetTempPath(),
                            Guid.NewGuid().ToString() + ".dat");

                        try
                        {
                            attachment.Save(tempTnefPath);

                            // Load the TNEF content as a MapiMessage.
                            MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tempTnefPath);

                            // Extract inner attachments from the TNEF message.
                            foreach (MapiAttachment innerAttachment in tnefMessage.Attachments)
                            {
                                string outputPath = Path.Combine(outputDirectory, innerAttachment.FileName);
                                innerAttachment.Save(outputPath);
                                Console.WriteLine($"Saved inner attachment: {outputPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing TNEF attachment: {ex.Message}");
                        }
                        finally
                        {
                            // Clean up the temporary TNEF file.
                            if (File.Exists(tempTnefPath))
                            {
                                try { File.Delete(tempTnefPath); } catch { /* ignore */ }
                            }
                        }
                    }
                    else
                    {
                        // Non‑TNEF attachment: save directly if desired.
                        string directPath = Path.Combine(outputDirectory, attachment.FileName);
                        try
                        {
                            attachment.Save(directPath);
                            Console.WriteLine($"Saved attachment: {directPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
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
}

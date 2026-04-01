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

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
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

                using (MapiMessage placeholder = new MapiMessage("Placeholder Sender", "Placeholder Recipient", "Placeholder Subject", "Placeholder Body"))
                {
                    placeholder.Save(msgPath);
                }
                Console.Error.WriteLine($"Info: Created placeholder MSG file at '{msgPath}'.");
                return;
            }

            // Load the MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                Console.WriteLine($"Message Subject: {msg.Subject}");
                Console.WriteLine($"Message Body: {msg.Body}");
                Console.WriteLine($"Attachments count: {msg.Attachments.Count}");

                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}");

                    // Identify possible TNEF attachment (commonly winmail.dat).
                    if (attachment.FileName != null &&
                        attachment.FileName.Equals("winmail.dat", StringComparison.OrdinalIgnoreCase) &&
                        attachment.BinaryData != null && attachment.BinaryData.Length > 0)
                    {
                        Console.WriteLine("  Detected TNEF attachment. Analyzing its contents...");

                        // Load the TNEF data as a separate MapiMessage.
                        using (MemoryStream tnefStream = new MemoryStream(attachment.BinaryData))
                        {
                            MapiMessage tnefMessage;
                            try
                            {
                                tnefMessage = MapiMessage.LoadFromTnef(tnefStream);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"  Error parsing TNEF data: {ex.Message}");
                                continue;
                            }

                            using (tnefMessage)
                            {
                                Console.WriteLine($"  TNEF Message Subject: {tnefMessage.Subject}");
                                Console.WriteLine($"  TNEF Message Body: {tnefMessage.Body}");
                                Console.WriteLine($"  TNEF Attachments count: {tnefMessage.Attachments.Count}");

                                foreach (MapiAttachment innerAtt in tnefMessage.Attachments)
                                {
                                    Console.WriteLine($"    Inner Attachment: {innerAtt.FileName}");
                                    // Optionally save inner attachment to disk.
                                    string outputPath = Path.Combine("Extracted", innerAtt.FileName ?? "attachment.bin");
                                    try
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                                        using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                                        {
                                            innerAtt.Save(fs);
                                        }
                                        Console.WriteLine($"    Saved inner attachment to: {outputPath}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"    Failed to save inner attachment: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}

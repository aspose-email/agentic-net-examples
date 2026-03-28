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
            // Path to the MSG file that may contain TNEF attachment
            string msgFilePath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                bool tnefFound = false;

                // Iterate through attachments to locate a TNEF (winmail.dat) attachment
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (attachment.FileName != null && attachment.FileName.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))
                    {
                        tnefFound = true;

                        // Save the attachment to a temporary file
                        string tempTnefPath = Path.Combine(Path.GetTempPath(), "winmail.dat");
                        try
                        {
                            attachment.Save(tempTnefPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving TNEF attachment: {ex.Message}");
                            continue;
                        }

                        // Verify the temporary TNEF file was created
                        if (!File.Exists(tempTnefPath))
                        {
                            Console.Error.WriteLine("Error: Temporary TNEF file was not created.");
                            continue;
                        }

                        // Load the TNEF data as a MapiMessage
                        try
                        {
                            using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tempTnefPath))
                            {
                                Console.WriteLine("=== TNEF Message Details ===");
                                Console.WriteLine($"Subject: {tnefMessage.Subject}");
                                Console.WriteLine($"From: {tnefMessage.SenderName}");
                                Console.WriteLine($"Body (plain text): {tnefMessage.Body}");
                                Console.WriteLine($"Number of attachments: {tnefMessage.Attachments.Count}");

                                // List attachment names inside the TNEF payload
                                foreach (MapiAttachment innerAttachment in tnefMessage.Attachments)
                                {
                                    Console.WriteLine($"  Inner attachment: {innerAttachment.FileName}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error loading TNEF data: {ex.Message}");
                        }
                        finally
                        {
                            // Clean up the temporary file
                            try
                            {
                                File.Delete(tempTnefPath);
                            }
                            catch
                            {
                                // Suppress any errors during cleanup
                            }
                        }
                    }
                }

                if (!tnefFound)
                {
                    Console.WriteLine("No TNEF (winmail.dat) attachment found in the MSG file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

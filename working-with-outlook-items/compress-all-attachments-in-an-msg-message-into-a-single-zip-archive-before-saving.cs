using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string zipPath = "attachments.zip";

            // Verify input MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG message
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // If there are no attachments, just save the original message
            if (message.Attachments == null || message.Attachments.Count == 0)
            {
                try
                {
                    message.Save(outputMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
                return;
            }

            // Prepare a temporary folder for extracting attachments
            string tempFolder = Path.Combine(Path.GetTempPath(), "AsposeAttachments");
            try
            {
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create temporary folder: {ex.Message}");
                return;
            }

            // Create ZIP archive containing all original attachments
            try
            {
                using (FileStream zipStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string tempFilePath = Path.Combine(tempFolder, attachment.FileName);
                        try
                        {
                            attachment.Save(tempFilePath);
                            zip.CreateEntryFromFile(tempFilePath, attachment.FileName);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to process attachment '{attachment.FileName}': {ex.Message}");
                        }
                        finally
                        {
                            // Clean up the temporary file
                            try { if (File.Exists(tempFilePath)) File.Delete(tempFilePath); } catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create ZIP archive: {ex.Message}");
                return;
            }

            // Replace original attachments with the single ZIP attachment
            try
            {
                message.Attachments.Clear();
                byte[] zipBytes = File.ReadAllBytes(zipPath);
                message.Attachments.Add("attachments.zip", zipBytes);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to replace attachments: {ex.Message}");
                return;
            }

            // Save the modified MSG message
            try
            {
                message.Save(outputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save modified MSG file: {ex.Message}");
            }
            finally
            {
                // Clean up the ZIP file
                try { if (File.Exists(zipPath)) File.Delete(zipPath); } catch { }
                // Clean up temporary folder
                try { if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true); } catch { }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

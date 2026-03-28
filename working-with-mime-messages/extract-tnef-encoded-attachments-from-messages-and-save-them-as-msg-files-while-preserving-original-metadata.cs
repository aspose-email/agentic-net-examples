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
            string inputMsgPath = "input.msg";
            string outputFolder = "ExtractedAttachments";

            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Read the Outlook message and its attachments.
            using (MapiMessageReader reader = new MapiMessageReader(inputMsgPath))
            {
                using (MapiMessage message = reader.ReadMessage())
                {
                    // Attachments extracted from the message (including those decoded from TNEF).
                    MapiAttachmentCollection attachments = reader.ReadAttachments();

                    foreach (MapiAttachment attachment in attachments)
                    {
                        // Process only TNEF attachments (commonly winmail.dat).
                        if (string.Equals(Path.GetExtension(attachment.FileName), ".dat", StringComparison.OrdinalIgnoreCase))
                        {
                            string tempTnefPath = Path.Combine(outputFolder, attachment.FileName);

                            // Save the TNEF attachment to a temporary file.
                            using (FileStream tnefWrite = new FileStream(tempTnefPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.SaveToTnef(tnefWrite);
                            }

                            // Load the TNEF content as a MapiMessage.
                            using (FileStream tnefRead = new FileStream(tempTnefPath, FileMode.Open, FileAccess.Read))
                            {
                                using (MapiMessage extractedMessage = MapiMessage.LoadFromTnef(tnefRead))
                                {
                                    // Save the extracted message as an MSG file, preserving metadata.
                                    string msgFileName = Path.GetFileNameWithoutExtension(attachment.FileName) + ".msg";
                                    string msgPath = Path.Combine(outputFolder, msgFileName);
                                    extractedMessage.Save(msgPath);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

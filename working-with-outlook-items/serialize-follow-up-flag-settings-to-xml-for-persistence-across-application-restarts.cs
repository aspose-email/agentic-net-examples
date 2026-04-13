using System;
using System.IO;
using System.Xml.Serialization;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string messagePath = "sample.msg";
            const string xmlPath = "followup.xml";

            // Ensure the message file exists; create a minimal placeholder if it does not.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Sample", "Body"))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message and apply follow‑up settings.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Set a follow‑up flag and add a category.
                    FollowUpManager.SetFlag(message, "Follow up");
                    FollowUpManager.AddCategory(message, "Important");

                    // Retrieve the follow‑up options for serialization.
                    FollowUpOptions options = FollowUpManager.GetOptions(message);

                    // Serialize the options to XML.
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(FollowUpOptions));
                        using (FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
                        {
                            serializer.Serialize(fs, options);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to serialize follow‑up options: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

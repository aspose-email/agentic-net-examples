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
            string msgPath = "distributionList.msg";

            // Guard file existence
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

                Console.Error.WriteLine($"Input file '{msgPath}' does not exist.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Ensure the message is a distribution list
                if (msg.SupportedType == MapiItemType.DistList)
                {
                    // Convert to MapiDistributionList
                    MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem();

                    // Update the display name
                    distList.DisplayName = "New Distribution List Name";

                    // Save the updated distribution list back to the file
                    distList.Save(msgPath);
                    Console.WriteLine("Distribution list name updated successfully.");
                }
                else
                {
                    Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

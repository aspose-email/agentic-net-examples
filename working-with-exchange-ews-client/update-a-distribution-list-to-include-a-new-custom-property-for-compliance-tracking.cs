using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the distribution list MSG file
            string dlPath = "DistributionList.msg";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(dlPath))
            {
                try
                {
                    MapiMessage placeholderMessage = new MapiMessage();
                    placeholderMessage.MessageClass = "IPM.DistList";
                    placeholderMessage.Save(dlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder distribution list file: {ex.Message}");
                    return;
                }
            }

            // Load the distribution list message
            MapiMessage loadedMessage;
            try
            {
                loadedMessage = MapiMessage.Load(dlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load distribution list file: {ex.Message}");
                return;
            }

            // Verify that the loaded message is a distribution list
            if (loadedMessage.SupportedType != MapiItemType.DistList)
            {
                Console.Error.WriteLine("The specified file is not a distribution list.");
                return;
            }

            // Convert to MapiDistributionList for easier handling
            MapiDistributionList distributionList = (MapiDistributionList)loadedMessage.ToMapiMessageItem();

            // Add a custom MAPI property for compliance tracking
            // Property name: "ComplianceTracking", value: "ComplianceRequired"
            MapiMessage underlyingMessage = distributionList.GetUnderlyingMessage();
            byte[] propertyValue = Encoding.Unicode.GetBytes("ComplianceRequired");
            underlyingMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyValue, "ComplianceTracking");

            // Save the updated distribution list back to the file
            try
            {
                distributionList.Save(dlPath);
                Console.WriteLine("Distribution list updated with custom compliance property.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save updated distribution list: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

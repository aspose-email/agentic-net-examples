using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string tgzPath = "backup.tgz";
            string exportFolder = "ExportedMessages";

            // Ensure the TGZ file exists; create a minimal placeholder if missing
            try
            {
                if (!File.Exists(tgzPath))
                {
                    // Create an empty TGZ file as a placeholder
                    File.WriteAllBytes(tgzPath, new byte[0]);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare input file: {ex.Message}");
                return;
            }

            // Ensure the export directory exists
            try
            {
                if (!Directory.Exists(exportFolder))
                {
                    Directory.CreateDirectory(exportFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create export directory: {ex.Message}");
                return;
            }

            // Read the Zimbra TGZ backup and export its contents
            try
            {
                using (TgzReader tgzReader = new TgzReader(tgzPath))
                {
                    tgzReader.ExportTo(exportFolder);
                    Console.WriteLine($"Export completed. Messages saved to '{exportFolder}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during TGZ processing: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

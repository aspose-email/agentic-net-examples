using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Olm;

class Program
{
    static void Main()
    {
        try
        {
            string olmPath = "input.olm";
            string pstPath = "output.pst";

            // Verify OLM input file exists
            if (!File.Exists(olmPath))
            {
                Console.Error.WriteLine($"Input OLM file not found: {olmPath}");
                return;
            }

            // Ensure the directory for PST output exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{pstDirectory}': {ex.Message}");
                    return;
                }
            }

            // Create PST file (Unicode format)
            try
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Load OLM storage
                    using (OlmStorage olm = OlmStorage.FromFile(olmPath))
                    {
                        // Iterate through each folder in OLM hierarchy
                        foreach (OlmFolder olmFolder in olm.FolderHierarchy)
                        {
                            // Iterate through each message in the current OLM folder
                            foreach (OlmMessageInfo messageInfo in olmFolder.EnumerateMessages())
                            {
                                // Extract the full MAPI message from OLM storage
                                MapiMessage mapiMessage = olm.ExtractMapiMessage(messageInfo);

                                // Add the message to the PST root folder (preserving flags is handled internally)
                                pst.RootFolder.AddMessage(mapiMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            Console.WriteLine("OLM to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

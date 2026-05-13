using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Olm;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace OlmToPstBatchConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Directory that contains the .olm files
                string inputDirectory = "OlmFiles";

                // Verify the directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory \"{inputDirectory}\" does not exist.");
                    return;
                }

                // Get all .olm files in the directory
                string[] olmFiles = Directory.GetFiles(inputDirectory, "*.olm");
                if (olmFiles.Length == 0)
                {
                    Console.Error.WriteLine("No .olm files found in the specified directory.");
                    return;
                }

                // Process each .olm file
                foreach (string olmFilePath in olmFiles)
                {
                    try
                    {
                        // Determine output PST file name (same base name, .pst extension)
                        string baseName = Path.GetFileNameWithoutExtension(olmFilePath);
                        string pstFilePath = Path.Combine(inputDirectory, baseName + ".pst");

                        // If a PST with the same name already exists, delete it to allow overwrite
                        if (File.Exists(pstFilePath))
                        {
                            File.Delete(pstFilePath);
                        }

                        // Create a new PST file (Unicode format)
                        using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                        {
                            // Open the OLM storage
                            using (OlmStorage olm = OlmStorage.FromFile(olmFilePath))
                            {
                                // Iterate through each folder in the OLM file
                                foreach (OlmFolder olmFolder in olm.GetFolders())
                                {
                                    // Create a corresponding folder in the PST (under the root folder)
                                    FolderInfo pstFolder = pst.RootFolder.AddSubFolder(olmFolder.Name);

                                    // Enumerate messages in the current OLM folder
                                    foreach (OlmMessageInfo messageInfo in olmFolder.EnumerateMessages())
                                    {
                                        // Extract the message as a MapiMessage
                                        using (MapiMessage mapiMessage = olm.ExtractMapiMessage(messageInfo))
                                        {
                                            // Add the message to the PST folder
                                            pstFolder.AddMessage(mapiMessage);
                                        }
                                    }
                                }
                            }
                        }

                        Console.WriteLine($"Successfully converted \"{olmFilePath}\" to \"{pstFilePath}\".");
                    }
                    catch (Exception ex)
                    {
                        // Handle errors for the individual file conversion
                        Console.Error.WriteLine($"Error converting \"{olmFilePath}\": {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception handler
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

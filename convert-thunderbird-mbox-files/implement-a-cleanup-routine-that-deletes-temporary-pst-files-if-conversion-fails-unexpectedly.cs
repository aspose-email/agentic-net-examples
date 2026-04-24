using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace CleanupPstExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths for input MBOX and temporary PST files
                string mboxFilePath = "input.mbox";
                string tempPstFilePath = "temp_output.pst";

                // Ensure the input MBOX file exists; create a minimal placeholder if missing
                if (!File.Exists(mboxFilePath))
                {
                    try
                    {
                        File.WriteAllText(mboxFilePath, string.Empty);
                        Console.WriteLine($"Placeholder MBOX file created at '{mboxFilePath}'.");
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MBOX file: {createEx.Message}");
                        return;
                    }
                }

                // Remove any existing temporary PST file before starting conversion
                if (File.Exists(tempPstFilePath))
                {
                    try
                    {
                        File.Delete(tempPstFilePath);
                    }
                    catch (Exception deleteEx)
                    {
                        Console.Error.WriteLine($"Failed to delete existing PST file: {deleteEx.Message}");
                        return;
                    }
                }

                PersonalStorage pst = null;
                try
                {
                    // Perform the conversion; this may throw if something goes wrong
                    pst = MailStorageConverter.MboxToPst(mboxFilePath, tempPstFilePath);
                    Console.WriteLine("MBOX to PST conversion succeeded.");

                    // Example usage of the resulting PST (list root folder message count)
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItems}");
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Conversion failed: {convEx.Message}");

                    // Cleanup: delete the temporary PST file if it was created
                    if (File.Exists(tempPstFilePath))
                    {
                        try
                        {
                            File.Delete(tempPstFilePath);
                            Console.WriteLine("Temporary PST file deleted due to conversion failure.");
                        }
                        catch (Exception cleanupEx)
                        {
                            Console.Error.WriteLine($"Failed to delete temporary PST file: {cleanupEx.Message}");
                        }
                    }

                    return;
                }
                finally
                {
                    // Ensure the PersonalStorage instance is disposed if it was created
                    if (pst != null)
                    {
                        pst.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

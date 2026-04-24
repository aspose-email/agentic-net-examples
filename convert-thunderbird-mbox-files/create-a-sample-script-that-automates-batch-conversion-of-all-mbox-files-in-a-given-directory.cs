using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace BatchMboxConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories
                string inputDirectory = "MboxFiles";
                string outputDirectory = "PstOutput";

                // Verify input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory \"{inputDirectory}\" does not exist.");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get all .mbox files in the input directory
                string[] mboxFiles = Directory.GetFiles(inputDirectory, "*.mbox");

                foreach (string mboxFilePath in mboxFiles)
                {
                    try
                    {
                        // Guard against missing file (create minimal placeholder if needed)
                        if (!File.Exists(mboxFilePath))
                        {
                            using (FileStream placeholder = File.Create(mboxFilePath))
                            {
                                // Empty placeholder file
                            }
                        }

                        // Determine output PST file path
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(mboxFilePath);
                        string pstFilePath = Path.Combine(outputDirectory, fileNameWithoutExtension + ".pst");

                        // Perform conversion from MBOX to PST
                        PersonalStorage pst = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);

                        // Dispose the PST object to release resources
                        pst.Dispose();

                        Console.WriteLine($"Converted \"{mboxFilePath}\" to \"{pstFilePath}\".");
                    }
                    catch (Exception conversionEx)
                    {
                        Console.Error.WriteLine($"Error converting \"{mboxFilePath}\": {conversionEx.Message}");
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

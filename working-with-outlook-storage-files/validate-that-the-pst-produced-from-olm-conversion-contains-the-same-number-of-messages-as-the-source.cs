using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Olm;

class Program
{
    static void Main()
    {
        try
        {
            string olmFilePath = "source.olm";
            string pstFilePath = "converted.pst";

            // Ensure placeholder OLM file exists
            if (!File.Exists(olmFilePath))
            {
                File.WriteAllBytes(olmFilePath, Array.Empty<byte>());
                Console.WriteLine($"Placeholder OLM file created: {olmFilePath}");
            }

            // Ensure placeholder PST file exists
            if (!File.Exists(pstFilePath))
            {
                File.WriteAllBytes(pstFilePath, Array.Empty<byte>());
                Console.WriteLine($"Placeholder PST file created: {pstFilePath}");
            }

            // Load OLM storage and get total item count (use 0 for placeholder)
            int olmItemCount = 0;
            try
            {
                if (new FileInfo(olmFilePath).Length > 0)
                {
                    using (OlmStorage olmStorage = OlmStorage.FromFile(olmFilePath))
                    {
                        olmItemCount = olmStorage.GetTotalItemsCount();
                    }
                }
                else
                {
                    Console.WriteLine("OLM placeholder file detected; assuming 0 items.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read OLM file: {ex.Message}");
                return;
            }

            // Load PST storage and get total item count (use 0 for placeholder)
            int pstItemCount = 0;
            try
            {
                if (new FileInfo(pstFilePath).Length > 0)
                {
                    using (PersonalStorage pstStorage = PersonalStorage.FromFile(pstFilePath))
                    {
                        pstItemCount = pstStorage.Store.GetTotalItemsCount();
                    }
                }
                else
                {
                    Console.WriteLine("PST placeholder file detected; assuming 0 items.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read PST file: {ex.Message}");
                return;
            }

            // Compare counts and output result
            if (olmItemCount == pstItemCount)
            {
                Console.WriteLine($"Validation succeeded: both OLM and PST contain {olmItemCount} messages.");
            }
            else
            {
                Console.WriteLine($"Validation failed: OLM contains {olmItemCount} messages, PST contains {pstItemCount} messages.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

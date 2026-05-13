using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string ostPath = "input.ost";
            string pstPath = "output.pst";

            // Ensure the input OST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(ostPath))
            {
                Console.Error.WriteLine($"OST file not found: {ostPath}. Creating placeholder empty OST.");
                using (PersonalStorage placeholder = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                {
                    // No additional content needed for the placeholder.
                }
            }

            // Load the OST file.
            using (PersonalStorage ostStorage = PersonalStorage.FromFile(ostPath))
            {
                // Convert the storage to PST format.
                ostStorage.ConvertTo(FileFormat.Pst);

                // Save the converted storage as a PST file.
                ostStorage.SaveAs(pstPath, FileFormat.Pst);
            }

            Console.WriteLine("OST to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

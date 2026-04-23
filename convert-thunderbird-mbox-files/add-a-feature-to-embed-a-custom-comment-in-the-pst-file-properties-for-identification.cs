using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    // Prepare a custom comment property (PR_COMMENT = 0x7C00).
                    int commentTag = 0x7C00;
                    byte[] commentBytes = Encoding.Unicode.GetBytes("Custom PST identification comment");
                    MapiProperty commentProperty = new MapiProperty(commentTag, commentBytes);

                    // Set the custom property on the message store.
                    pst.Store.SetProperty(commentProperty);

                    Console.WriteLine("Custom comment added to PST properties successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

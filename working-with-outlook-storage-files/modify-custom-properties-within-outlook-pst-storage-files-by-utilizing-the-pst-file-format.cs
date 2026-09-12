using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create a new Unicode PST file
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created new PST file at '{pstPath}'.");
            }

            // Open the PST file with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Access the message store (root of PST)
                MessageStore store = pst.Store;

                // Example: set a custom property (using a known property descriptor)
                // Convert string value to byte[] as required by MapiProperty for binary data
                byte[] customValue = Encoding.UTF8.GetBytes("Custom PST Path Value");
                MapiProperty customProp = new MapiProperty(KnownPropertyList.PstPath, customValue);

                // Apply the property to the store
                store.SetProperty(customProp);

                Console.WriteLine("Custom property has been set successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

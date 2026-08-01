using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

namespace AsposeEmailPstPasswordCheck
{
    class Program
    {
        static void Main()
        {
            try
            {
                string pstPath = "sample.pst";

                // Create a minimal PST file if it does not exist (placeholder)
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created as placeholder
                    }
                }

                bool isPasswordProtected = false;

                try
                {
                    // Attempt to open the PST without a password
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // Opened successfully – not password protected
                        isPasswordProtected = false;
                    }
                }
                catch (Exception ex) when (ex.Message != null && ex.Message.IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Exception indicates a password is required
                    isPasswordProtected = true;
                }
                catch (Exception ex)
                {
                    // Other exceptions – rethrow after logging
                    Console.Error.WriteLine($"Unexpected error while checking PST: {ex.Message}");
                    return;
                }

                Console.WriteLine($"Is PST password protected? {isPasswordProtected}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

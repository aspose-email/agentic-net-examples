using Aspose.Email;
using System;
using System.IO;
using System.Reflection;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            string nsfPath = "sample.nsf";

            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                return;
            }

            try
            {
                using (NotesStorageFacility facility = new NotesStorageFacility(nsfPath))
                {
                    PropertyInfo versionProperty = typeof(NotesStorageFacility).GetProperty("Version", BindingFlags.Public | BindingFlags.Instance);
                    if (versionProperty != null)
                    {
                        object versionValue = versionProperty.GetValue(facility);
                        if (versionValue is int version)
                        {
                            if (version >= 7)
                            {
                                Console.WriteLine($"NSF version {version} is compatible (>= 7).");
                            }
                            else
                            {
                                Console.WriteLine($"NSF version {version} is not compatible. Requires version 7 or higher.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unable to interpret NSF version value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("NSF version information is not available via reflection.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing NSF file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

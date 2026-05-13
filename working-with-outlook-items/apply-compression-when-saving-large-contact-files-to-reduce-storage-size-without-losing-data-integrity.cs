using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string outputDirectory = "Output";
            string tempVcfPath = Path.Combine(outputDirectory, "contact_temp.vcf");
            string compressedVcfPath = Path.Combine(outputDirectory, "contact.vcf.gz");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a MAPI contact and populate its fields
            using (MapiContact contact = new MapiContact())
            {
                contact.NameInfo.GivenName = "John";
                contact.NameInfo.Surname = "Doe";
                contact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

                // Professional details
                contact.ProfessionalInfo.Title = "Software Engineer";
                contact.ProfessionalInfo.CompanyName = "Acme Corp";

                // Save the contact to a temporary VCF file
                contact.Save(tempVcfPath);
            }

            // Compress the VCF file using GZip with optimal compression
            using (FileStream originalFileStream = new FileStream(tempVcfPath, FileMode.Open, FileAccess.Read))
            using (FileStream compressedFileStream = new FileStream(compressedVcfPath, FileMode.Create, FileAccess.Write))
            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Optimal))
            {
                originalFileStream.CopyTo(compressionStream);
            }

            // Delete the temporary uncompressed VCF file
            File.Delete(tempVcfPath);

            Console.WriteLine($"Contact saved and compressed successfully to: {compressedVcfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

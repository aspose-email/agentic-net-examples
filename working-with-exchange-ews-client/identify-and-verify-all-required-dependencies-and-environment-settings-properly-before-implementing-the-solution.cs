using System;
using System.IO;
using System.Runtime.InteropServices;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Verify that the Aspose.Email license file exists
            string licensePath = "Aspose.Email.lic";
            if (!File.Exists(licensePath))
            {
                Console.Error.WriteLine($"License file not found at '{licensePath}'. Continuing without a license.");
            }
            else
            {
                try
                {
                    License license = new License();
                    license.SetLicense(licensePath);
                    Console.WriteLine("Aspose.Email license loaded successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load Aspose.Email license: {ex.Message}");
                }
            }

            // Verify .NET runtime information
            string frameworkDescription = RuntimeInformation.FrameworkDescription;
            Console.WriteLine($".NET runtime: {frameworkDescription}");

            // Verify operating system information
            string osDescription = RuntimeInformation.OSDescription;
            Console.WriteLine($"Operating System: {osDescription}");

            // Verify that Aspose.Email assembly is accessible and report its version
            try
            {
                Type mailMessageType = typeof(MailMessage);
                string assemblyVersion = mailMessageType.Assembly.GetName().Version.ToString();
                Console.WriteLine($"Aspose.Email assembly version: {assemblyVersion}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to verify Aspose.Email assembly: {ex.Message}");
            }

            // Additional environment checks can be placed here
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Get the assembly that contains the AMP types
            Assembly ampAssembly = typeof(AmpMessage).Assembly;

            // Retrieve all classes defined in the Aspose.Email.Amp namespace
            var ampComponentNames = ampAssembly.GetTypes()
                .Where(t => t.IsClass && t.Namespace == "Aspose.Email.Amp")
                .Select(t => t.Name)
                .OrderBy(name => name)
                .ToList();

            Console.WriteLine("AMP components supported for processing MSG format:");
            foreach (string componentName in ampComponentNames)
            {
                Console.WriteLine("- " + componentName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

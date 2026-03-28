using System;
using System.Reflection;
using System.Linq;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Get the assembly that contains the AmpMessage type
            Assembly ampAssembly = typeof(Aspose.Email.Amp.AmpMessage).Assembly;

            // Find all types in the Aspose.Email.Amp namespace
            var ampTypes = ampAssembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Equals("Aspose.Email.Amp"))
                .OrderBy(t => t.Name)
                .ToList();

            Console.WriteLine("AMP components supported for processing MSG format:");
            foreach (Type type in ampTypes)
            {
                Console.WriteLine("- " + type.FullName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}

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
            // Get the assembly that contains the AMP types.
            Assembly ampAssembly = typeof(AmpMessage).Assembly;

            // Base type for all AMP components.
            Type ampComponentBase = typeof(AmpComponent);

            // Find all non‑abstract types that derive from AmpComponent.
            var componentTypes = ampAssembly.GetTypes()
                .Where(t => ampComponentBase.IsAssignableFrom(t) && !t.IsAbstract && t != ampComponentBase)
                .Select(t => t.FullName)
                .OrderBy(name => name)
                .ToList();

            Console.WriteLine("AMP components supported for processing MSG format messages:");
            foreach (var typeName in componentTypes)
            {
                Console.WriteLine("- " + typeName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

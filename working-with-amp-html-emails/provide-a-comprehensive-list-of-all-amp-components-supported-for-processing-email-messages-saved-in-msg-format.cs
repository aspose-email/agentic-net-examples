using System;
using System.Reflection;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Get the assembly that contains the AMP types.
            Assembly ampAssembly = typeof(AmpMessage).Assembly;

            // Retrieve all types in the Aspose.Email.Amp namespace.
            Type[] ampTypes = ampAssembly.GetTypes();

            Console.WriteLine("AMP components supported for processing MSG format messages:");
            foreach (Type type in ampTypes)
            {
                if (type.Namespace != null && type.Namespace.Equals("Aspose.Email.Amp"))
                {
                    // List only public, non-abstract classes.
                    if (type.IsClass && type.IsPublic && !type.IsAbstract)
                    {
                        Console.WriteLine("- " + type.Name);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}

using System;
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
            Assembly emailAssembly = typeof(Aspose.Email.Amp.AmpMessage).Assembly;

            // Retrieve all types defined in the Aspose.Email.Amp namespace
            Type[] allTypes = emailAssembly.GetTypes();
            Console.WriteLine("Supported AMP components (types) in Aspose.Email.Amp namespace:");
            foreach (Type type in allTypes)
            {
                if (type.Namespace != null && type.Namespace.Equals("Aspose.Email.Amp"))
                {
                    Console.WriteLine("- " + type.FullName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
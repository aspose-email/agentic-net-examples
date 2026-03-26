using System;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Amp;

public class Program
{
    public static void Main()
    {
        try
        {
            // Get the assembly that contains the AMP types
            Assembly ampAssembly = typeof(Aspose.Email.Amp.AmpMessage).Assembly;

            // Find all non-abstract types that derive from AmpComponent
            Type baseComponentType = typeof(Aspose.Email.Amp.AmpComponent);
            Type[] allTypes = ampAssembly.GetTypes();
            foreach (Type type in allTypes)
            {
                if (type.IsClass && !type.IsAbstract && baseComponentType.IsAssignableFrom(type))
                {
                    Console.WriteLine("Supported AMP component: " + type.FullName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
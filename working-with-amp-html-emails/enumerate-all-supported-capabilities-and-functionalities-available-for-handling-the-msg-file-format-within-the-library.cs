using System;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            Type mapMessageType = typeof(Aspose.Email.Mapi.MapiMessage);
            Console.WriteLine("Capabilities of Aspose.Email MapiMessage (MSG handling):");

            // Constructors
            Console.WriteLine("\nConstructors:");
            ConstructorInfo[] constructors = mapMessageType.GetConstructors();
            foreach (ConstructorInfo ctor in constructors)
            {
                Console.Write("- ");
                ParameterInfo[] parameters = ctor.GetParameters();
                Console.Write(mapMessageType.Name + "(");
                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.Write(parameters[i].ParameterType.Name + " " + parameters[i].Name);
                    if (i < parameters.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine(")");
            }

            // Properties
            Console.WriteLine("\nProperties:");
            PropertyInfo[] properties = mapMessageType.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                Console.WriteLine("- " + prop.PropertyType.Name + " " + prop.Name);
            }

            // Methods
            Console.WriteLine("\nMethods:");
            MethodInfo[] methods = mapMessageType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                // Skip property accessors and inherited Object methods
                if (method.IsSpecialName)
                {
                    continue;
                }
                if (method.DeclaringType != mapMessageType)
                {
                    continue;
                }

                Console.Write("- " + method.ReturnType.Name + " " + method.Name + "(");
                ParameterInfo[] parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.Write(parameters[i].ParameterType.Name + " " + parameters[i].Name);
                    if (i < parameters.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine(")");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
using System;
using System.Reflection;
using Aspose.Email.Mapi;

public class Program
{
    public static void Main()
    {
        try
        {
            // Get the type representing the MSG handling class.
            Type mapMessageType = typeof(MapiMessage);

            Console.WriteLine("=== MapiMessage Capabilities ===");

            // List constructors.
            ConstructorInfo[] constructors = mapMessageType.GetConstructors();
            foreach (ConstructorInfo ctor in constructors)
            {
                ParameterInfo[] ctorParams = ctor.GetParameters();
                string paramList = string.Empty;
                for (int i = 0; i < ctorParams.Length; i++)
                {
                    ParameterInfo p = ctorParams[i];
                    paramList += p.ParameterType.Name + " " + p.Name;
                    if (i < ctorParams.Length - 1)
                    {
                        paramList += ", ";
                    }
                }
                Console.WriteLine($"Constructor: {mapMessageType.Name}({paramList})");
            }

            // List properties.
            PropertyInfo[] properties = mapMessageType.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                string accessor = "";
                if (prop.CanRead) accessor += "get; ";
                if (prop.CanWrite) accessor += "set; ";
                Console.WriteLine($"Property: {prop.PropertyType.Name} {prop.Name} {{ {accessor}}}");
            }

            // List public instance methods (excluding property accessors).
            MethodInfo[] methods = mapMessageType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                if (method.IsSpecialName) continue; // Skip getters/setters and other special methods.

                ParameterInfo[] methodParams = method.GetParameters();
                string paramList = string.Empty;
                for (int i = 0; i < methodParams.Length; i++)
                {
                    ParameterInfo p = methodParams[i];
                    paramList += p.ParameterType.Name + " " + p.Name;
                    if (i < methodParams.Length - 1)
                    {
                        paramList += ", ";
                    }
                }
                Console.WriteLine($"Method: {method.ReturnType.Name} {method.Name}({paramList})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

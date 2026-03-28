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
            // Obtain the type representing the MSG handling class.
            Type mapMsgType = typeof(MapiMessage);
            Console.WriteLine("Capabilities for handling MSG format (MapiMessage):");

            // List all public constructors.
            ConstructorInfo[] constructors = mapMsgType.GetConstructors();
            Console.WriteLine("\nConstructors:");
            foreach (ConstructorInfo ctor in constructors)
            {
                ParameterInfo[] parameters = ctor.GetParameters();
                string paramList = "";
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramList += parameters[i].ParameterType.Name + " " + parameters[i].Name;
                    if (i < parameters.Length - 1) paramList += ", ";
                }
                Console.WriteLine($"  {mapMsgType.Name}({paramList})");
            }

            // List all public properties.
            PropertyInfo[] properties = mapMsgType.GetProperties();
            Console.WriteLine("\nProperties:");
            foreach (PropertyInfo prop in properties)
            {
                // Show get/set availability.
                string accessor = "";
                if (prop.CanRead) accessor += "get; ";
                if (prop.CanWrite) accessor += "set; ";
                Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name} {{ {accessor}}}");
            }

            // List all public methods defined on MapiMessage (excluding inherited Object methods).
            MethodInfo[] methods = mapMsgType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            Console.WriteLine("\nMethods:");
            foreach (MethodInfo method in methods)
            {
                if (method.DeclaringType != mapMsgType) continue; // Skip inherited members.

                string staticModifier = method.IsStatic ? "static " : "";
                string signature = $"{staticModifier}{method.ReturnType.Name} {method.Name}(";
                ParameterInfo[] parms = method.GetParameters();
                for (int i = 0; i < parms.Length; i++)
                {
                    signature += $"{parms[i].ParameterType.Name} {parms[i].Name}";
                    if (i < parms.Length - 1) signature += ", ";
                }
                signature += ")";
                Console.WriteLine($"  {signature}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

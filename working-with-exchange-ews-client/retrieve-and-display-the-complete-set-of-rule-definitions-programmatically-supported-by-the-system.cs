using System;
using System.Reflection;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Get the assembly that contains Aspose.Email types
            Assembly asposeAssembly = typeof(MailMessage).Assembly;

            // Retrieve all types defined in the assembly
            Type[] allTypes = asposeAssembly.GetTypes();

            foreach (Type currentType in allTypes)
            {
                // Filter only types that belong to the Aspose.Email namespace hierarchy
                if (currentType.Namespace != null && currentType.Namespace.StartsWith("Aspose.Email"))
                {
                    Console.WriteLine($"Type: {currentType.FullName}");

                    // Get public members declared directly on the type
                    MemberInfo[] members = currentType.GetMembers(
                        BindingFlags.Public |
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.DeclaredOnly);

                    foreach (MemberInfo member in members)
                    {
                        Console.WriteLine($"  Member: {member.MemberType} {member.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

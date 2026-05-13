using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Attempt to locate the UI control that provides drag‑and‑drop support.
            // This type is defined in the Aspose.Email.Windows.WPF namespace and is intended for UI applications.
            Type fileDropPanelType = Type.GetType("Aspose.Email.Windows.WPF.FileDropPanel, Aspose.Email");

            if (fileDropPanelType != null)
            {
                Console.WriteLine("Aspose.Email provides the FileDropPanel UI control for drag‑and‑drop.");
                Console.WriteLine("Drag‑and‑drop functionality is limited to Windows Forms/WPF applications.");
                Console.WriteLine("A console application cannot host this control, so the feature cannot be demonstrated here.");
            }
            else
            {
                Console.WriteLine("FileDropPanel type not found. Drag‑and‑drop UI components are unavailable.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

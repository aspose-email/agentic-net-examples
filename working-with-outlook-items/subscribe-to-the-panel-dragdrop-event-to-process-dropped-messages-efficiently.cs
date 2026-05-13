using System;

class Program
{
    static void Main()
    {
        try
        {
            // This console sample demonstrates that UI components such as FileDropPanel
            // are not used because console-only samples are required.
            Console.WriteLine("FileDropPanel UI is not applicable in a console application.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

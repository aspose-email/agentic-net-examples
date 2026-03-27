using System;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new FolderInfo instance
            FolderInfo folderInfo = new FolderInfo();

            // Change the display name of the folder
            folderInfo.ChangeDisplayName("My New Folder");

            // Verify the change
            Console.WriteLine("Folder display name set to: " + folderInfo.DisplayName);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
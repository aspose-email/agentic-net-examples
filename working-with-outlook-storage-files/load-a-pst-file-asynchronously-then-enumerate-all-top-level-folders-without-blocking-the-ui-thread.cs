using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Guard file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Load PST asynchronously
            using (PersonalStorage pst = await PersonalStorage.FromFileAsync(pstPath))
            {
                // Enumerate top‑level folders
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

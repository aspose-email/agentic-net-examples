using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstFilePath = "sample.pst";
            string xmlOutputPath = "categories.xml";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(xmlOutputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                List<PstItemCategory> categoryList;
                try
                {
                    categoryList = pst.GetCategories();
                }
                catch (Exception catEx)
                {
                    Console.Error.WriteLine($"Failed to retrieve categories: {catEx.Message}");
                    return;
                }

                // Build XML document
                XElement rootElement = new XElement("Categories");
                foreach (PstItemCategory category in categoryList)
                {
                    XElement categoryElement = new XElement("Category",
                        new XAttribute("Name", category.Name),
                        new XAttribute("Color", category.Color.ToString()));
                    rootElement.Add(categoryElement);
                }

                XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), rootElement);

                // Save XML file
                try
                {
                    xmlDocument.Save(xmlOutputPath);
                    Console.WriteLine($"Categories exported to: {xmlOutputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save XML file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

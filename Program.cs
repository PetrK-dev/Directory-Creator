using System;
using System.IO;
using Newtonsoft.Json;

class DirectoryCreator
{
    static void Main(string[] args)
    {
        // Získá cestu k adresáři projektu.
        string projectDirectory = GetProjectDirectory();
        // Načte cestu k sdílenému disku.
        string sharedDiskPath = loadSharedDisk(projectDirectory);
        // Načte název projektu od uživatele.
        string projectName = loadProjectName(sharedDiskPath);
        // Načte vybranou šablonu.
        dynamic template = loadTemplate(projectDirectory);
        // Vytvoří strukturu projektu podle šablony.
        CreateProjectStructure(sharedDiskPath, template, projectName);
    }

    /// <summary>
    /// Získá cestu k adresáři projektu přechodem z adresáře spustitelného souboru.
    /// </summary>
    /// <returns>Cesta k adresáři projektu.</returns>
    static string GetProjectDirectory()
    {
        string exeDirectory = AppContext.BaseDirectory;
        string projectDirectory = Path.GetFullPath(Path.Combine(exeDirectory, "..", "..", ".."));
        return projectDirectory;
    }

    /// <summary>
    /// Načte cestu k sdílenému disku v rámci projektu.
    /// </summary>
    /// <param name="projectDirectory">Cesta k adresáři projektu.</param>
    /// <returns>Cesta k sdílenému disku.</returns>
    static string loadSharedDisk(string projectDirectory)
    {
        string diskName = "sharedDisk";
        string sharedDiskPath = Path.Combine(projectDirectory, diskName);

        return sharedDiskPath;
    }

    /// <summary>
    /// Získá název projektu od uživatele a ověří, že projekt s tímto názvem neexistuje.
    /// </summary>
    /// <param name="sharedDiskPath">Cesta k sdílenému disku.</param>
    /// <returns>Validní název projektu.</returns>
    static string loadProjectName(string sharedDiskPath)
    {
        string projectName;
        Console.Write("Zadejte název projektu: ");

        do
        {
            projectName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(projectName))
            {
                Console.WriteLine("Název projektu nesmí být prázdný. Zadejte prosím jiný název.");
                continue;
            }

            string projectPath = Path.Combine(sharedDiskPath, projectName);
            if (Directory.Exists(projectPath))
                Console.WriteLine("Projekt s tímto názvem již existuje. Zadejte prosím jiný název.");
            else
                break;

        } while (true);
        
        return projectName;
    }

    /// <summary>
    /// Načte dostupné šablony a umožní uživateli vybrat jednu z nich.
    /// </summary>
    /// <param name="projectDirectory">Cesta k adresáři projektu.</param>
    /// <returns>Vybraná šablona jako dynamický objekt.</returns>
    static dynamic loadTemplate(string projectDirectory)
    {
        string templates = "templates";
        string templatesDirectory = Path.Combine(projectDirectory, templates);
        string[] templateFiles = Directory.GetFiles(templatesDirectory, "*.json");

        Console.WriteLine("Dostupné šablony:");
        for (int i = 0; i < templateFiles.Length; i++)
        {
            string templateName = Path.GetFileNameWithoutExtension(templateFiles[i]);
            Console.WriteLine($"{i + 1}. {templateName}");
        }

        int choice;
        while (true)
        {
            Console.Write("Zadejte číslo šablony: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out choice) && choice >= 1 && choice <= templateFiles.Length)
            {
                break;
            }
            else
            {
                Console.WriteLine("Neplatná volba, zkuste to znovu.");
            }
        }
        string templatePath = templateFiles[choice - 1];

        string json = File.ReadAllText(templatePath);
        return JsonConvert.DeserializeObject(json);
    }

    /// <summary>
    /// Vytvoří strukturu projektu na základě šablony.
    /// </summary>
    /// <param name="sharedDiskPath">Cesta k sdílenému disku.</param>
    /// <param name="template">Vybraná šablona.</param>
    /// <param name="projectName">Název projektu.</param>
    static void CreateProjectStructure(string sharedDiskPath, dynamic template, string projectName)
    {
        string projectPath = Path.Combine(sharedDiskPath, projectName);
        CreateFolders(projectPath, template.folders);
        Console.WriteLine("Struktura složek byla vytvořena.");
    }

    /// <summary>
    /// Rekurzivně vytváří složky dle specifikace v šabloně.
    /// </summary>
    /// <param name="parentPath">Cesta k nadřazené složce.</param>
    /// <param name="folders">Seznam složek k vytvoření.</param>
    static void CreateFolders(string parentPath, dynamic folders)
    {
        foreach (var folder in folders)
        {
            string folderName = folder.name;
            string folderPath = Path.Combine(parentPath, folderName);
            Directory.CreateDirectory(folderPath);

            if (folder.folders != null)
            {
                CreateFolders(folderPath, folder.folders);
            }
        }
    }
}
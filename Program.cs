using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet run -- single-page \"Your Article Title\"");
            Console.WriteLine("  dotnet run -- parent-page \"Your Parent Title\"");
            return;
        }

        string mode = args[0].Trim().ToLower();
        string rawTitle = args[1].Trim();
        string fileName = Slugify(rawTitle) + ".md";

        if (File.Exists(fileName))
        {
            Console.WriteLine($"{fileName} already exists!");
            return;
        }

        Console.WriteLine($"Creating {fileName}...");

        string content = mode switch
        {
            "single-page" => GenerateSinglePageContent(rawTitle),
            "parent-page" => GenerateParentPageContent(rawTitle),
            _ => throw new ArgumentException("Unknown mode. Use 'single-page' or 'parent-page'.")
        };

        File.WriteAllText(fileName, content);
        Console.WriteLine($"File {fileName} has been created.");
    }

    static string GenerateSinglePageContent(string rawTitle)
    {
        string title = ToTitleCase(rawTitle);
        return $@"# {title}


## Section 1

### Section 1.1

### Section 1.2

## Section 2

### Section 2.1

### Section 2.2

# Reference

- [Reference1](www.reference1.com/reference)

123
";
    }

    static string GenerateParentPageContent(string rawTitle)
    {
        string title = ToTitleCase(rawTitle);
        return $@"# {title}


## Pages

- [child-page-1](child-page-1.md)
- [child-page-2](child-page-2.md)

## Reference
";
    }

    static string Slugify(string text)
    {
        string slug = text.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, "-+", "-");
        return slug.Trim('-');
    }

    static string ToTitleCase(string text)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
    }
}

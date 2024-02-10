using System.Text.RegularExpressions;

public static class AttributeFile
{
    public static void Execute(string[] line)
    {
        Regex regex = new Regex("[RASH]{1}");

        if (line.Length < 3)
        {
            Status.Report("No arguments for AF.");
            return;
        }
        else if (!regex.IsMatch(line[2]))
        {
            Status.Report($"Attribute doesn't exist or too many arguments.");
            return;
        }
        else if (line.Length > 3)
        {
            Status.Report("Too many arguments.");
            return;
        }

        FileInfo[] files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles(line[1]);

        foreach (var file in files)
        {
            string fileName = file.Name;
            FileAttributes fa = GetFileAttributes(line, ref fileName);
            File.SetAttributes(fileName, fa);
        }
    }

    private static FileAttributes GetFileAttributes(string[] line, ref string fileName)
    {
        FileAttributes attributes = File.GetAttributes(fileName);

        for (int i = 0; i < line[2].Length; i++)
        {
            switch (line[2][i])
            {
                case 'R':
                    if ((attributes & FileAttributes.ReadOnly) != 0)
                        attributes &= ~FileAttributes.ReadOnly;
                    else
                        attributes |= FileAttributes.ReadOnly;
                    break;
                case 'A':
                    if ((attributes & FileAttributes.Archive) != 0)
                        attributes &= ~FileAttributes.Archive;
                    else
                        attributes |= FileAttributes.Archive;
                    break;
                case 'S':
                    if ((attributes & FileAttributes.System) != 0)
                        attributes &= ~FileAttributes.System;
                    else
                        attributes |= FileAttributes.System;
                    break;
                case 'H':
                    if (Regex.IsMatch(fileName, @"^\."))
                    {
                        fileName = fileName.Remove(0, 1);
                        File.Move("." + fileName, fileName);
                    }
                    else
                    {
                        File.Move(fileName, "." + fileName);
                        fileName = "." + fileName;
                    }
                    break;
                default:
                    Status.Report($"Attribute doesn't exist: {line[2][i]}");
                    break;
            }
        }

        return attributes;
    }
}
public static class CreateDirectory
{
    public static void Execute(string[] line)
    {
        Directory.CreateDirectory(line[1]);

        // if no more commands
        if (line.Length < 3)
            return;

        if (!isArgumentAdded(line)) // argument is file name for COPY or JOIN
            return;

        switch (line[2])
        {
            case "COPY":
                Copy(line);
                break;
            case "JOIN":
                Join(line);
                break;
            case "R":
                RenameDirectory(line);
                break;
            default:
                Status.Report($"Unknown command: {line[2]}");
                break;
        }
    }

    private static void RenameDirectory(string[] line)
    {
        if (line.Length > 4)
        {
            Status.Report("R recieves only 1 argument.");
            return;
        }

        if (!Directory.Exists(line[1]))
        {
            Status.Report($"Directory not found: {line[1]}");
            return;
        }

        if (!CommandProcessor.CheckName(ref line[3]))
            return;

        if (Directory.Exists(line[3]))
        {
            Status.Report($"Directory already exists: {line[3]}");
            return;
        }

        Directory.Move(line[1], line[3]);
    }

    private static void Copy(string[] line)
    {
        if (line.Length > 4)
        {
            Status.Report("COPY recieves only 1 argument.");
            return;
        }

        if (CommandProcessor.CheckName(ref line[3]))
        {
            if (!Directory.Exists(line[3]))
            {
                Status.Report($"Directory doesn't exist {line[3]}");
                return;
            }

            DirectoryInfo diSource = new DirectoryInfo(line[3]);
            DirectoryInfo diTarget = new DirectoryInfo(line[1]);

            CopyAll(diSource, diTarget);
        }
    }

    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // copy each file into the new directory
        foreach (FileInfo fi in source.GetFiles())
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);

        // copy each subdirectory using recursion
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    private static void Join(string[] line)
    {
        // check file name is correct
        for (int i = 3; i < line.Length; i++)
        {
            if (!CommandProcessor.CheckName(ref line[i]))
            {
                return;
            }
        }

        for (int i = 3; i < line.Length; i++)
        {
            FileInfo[] sourceFiles = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles(line[i]);

            if (sourceFiles.Length == 0)
            {
                Status.Report("No files found.");
                return;
            }

            foreach (var file in sourceFiles)
            {
                if (file.Name == line[1])
                    continue;

                using (StreamReader sourceFileReader = new StreamReader(file.Name))
                {
                    using (StreamWriter destinationFileWriter = File.AppendText(line[1]))
                    {
                        string? curLine;
                        while ((curLine = sourceFileReader.ReadLine()) != null)
                        {
                            destinationFileWriter.WriteLine(curLine);
                        }
                    }
                }
            }
        }
    }

    private static bool isArgumentAdded(string[] line)
    {
        if (line.Length <= 3)
        {
            Status.Report("No argument was transfered.");
            return false;
        }
        else
            return true;
    }
}
public static class CreateFile
{
    public static void Execute(string[] line)
    {
        using (var stream = File.Create(line[1])) { }
        
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
            default:
                Status.Report($"Unknown command: {line[2]}");
                break;
        }
    }

    private static void Copy(string[] line)
    {
        if (line.Length > 4)
        {
            Status.Report("COPY recieves only 1 argument.");
            return;
        }

        if (CommandProcessor.CheckName(ref line[3]))
            File.Copy(line[3], line[1], true);
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
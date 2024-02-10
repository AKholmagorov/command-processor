public static class JoinDirectories
{
    public static void Execute(string[] line)
    {
        if (line.Length < 4)
        {
            Status.Report("JOIN recieves at least 4 arguments.");
            return;
        }
        else if (!Directory.Exists(line[1]))
        {
            Status.Report($"Directory doesn't exist {line[1]}");
            return;
        }

        Join(line);
    }

    private static void Join(string[] line)
    {
        FileCopyStrategy strategy;
        switch (line[line.Length - 1])
        {
            case "OW_DEF":
                strategy = FileCopyStrategy.OW_DEF;
                break;
            case "OW_NEW":
                strategy = FileCopyStrategy.OW_NEW;
                break;
            case "OW_REN":
                strategy = FileCopyStrategy.OW_REN;
                break;
            default:
                Status.Report($"Unknown argument: {line[line.Length - 1]}");
                return;
        }

        for (int i = 2; i < line.Length - 1; i++)
        {
            if (CommandProcessor.CheckName(ref line[i]))
            {
                if (!Directory.Exists(line[i]))
                {
                    Status.Report($"Directory doesn't exist: {line[i]}");
                    return;
                }

                DirectoryInfo diSource = new DirectoryInfo(line[i]);
                DirectoryInfo diTarget = new DirectoryInfo(line[1]);

                CopyAll(diSource, diTarget, strategy);
            }
        }
    }

    public static void CopyAll(DirectoryInfo source, DirectoryInfo target, FileCopyStrategy strategy)
    {
        // copy each file into the new directory based on the strategy
        foreach (FileInfo sourceFile in source.GetFiles())
        {
            var targetFilePath = Path.Combine(target.FullName, sourceFile.Name);

            switch (strategy)
            {
                case FileCopyStrategy.OW_DEF:
                    if (!File.Exists(targetFilePath))
                    {
                        sourceFile.CopyTo(targetFilePath);
                    }
                    break;
                case FileCopyStrategy.OW_NEW:
                    if (!File.Exists(targetFilePath) || sourceFile.LastWriteTime > File.GetLastWriteTime(targetFilePath))
                    {
                        sourceFile.CopyTo(targetFilePath, true);
                    }
                    break;
                case FileCopyStrategy.OW_REN:
                    var newFilePath = targetFilePath;
                    var counter = 1;
                    while (File.Exists(newFilePath))
                    {
                        newFilePath = Path.Combine(target.FullName, $"{Path.GetFileNameWithoutExtension(sourceFile.Name)}~{counter++}{sourceFile.Extension}");
                    }
                    sourceFile.CopyTo(newFilePath);
                    break;
                default:
                    Status.Report($"Unknown file copy strategy: {strategy}");
                    return;
            }
        }

        // copy each subdirectory using recursion
        foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(sourceSubDir.Name);
            CopyAll(sourceSubDir, nextTargetSubDir, strategy);
        }
    }

}

public enum FileCopyStrategy
{
    OW_DEF,  // существующие файлы не переписывать
    OW_NEW,  // из двух одноименных файлов оставлять более новый
    OW_REN   // переименовывать файлы
}

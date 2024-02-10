public static class DeleteDirectory
{
    public static void Execute(string[] line)
    {
        Delete(line);
    }

    private static void Delete(string[] line)
    {
        if (line.Length > 2)
        {
            Status.Report("DF recieves only 1 argument.");
            return;
        }

        if (!Directory.Exists(line[1]))
        {
            Status.Report($"Directory not found: {line[1]}");
            return;
        }
        
        Directory.Delete(line[1], true);
    }
}
public static class DeleteFile
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
        else
        {
            FileInfo[] files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles(line[1]);

            if (files.Length == 0)
            {
                Status.Report("No files found.");
                return;
            }
            else
            {
                foreach (var file in files)
                {
                    file.Delete();
                }
            }
        }
    }
}
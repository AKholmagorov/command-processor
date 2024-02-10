public class Program
{
    private static void Main()
    {
        string file = "code.txt";

        CheckFile(file);

        if (Status.error)
        {
            Console.WriteLine(Status.msg);
            return;
        }
        else
        {
            string[] lines = File.ReadAllLines(file);

            foreach (var line in lines)
            {
                Status.Line++;
                CommandProcessor.ProceedCommands(line);

                if (Status.error)
                {
                    Console.WriteLine(Status.msg);
                    return;
                }
            }
        }

        if (!Status.error)
            Console.WriteLine("Success.");
    }

    private static void CheckFile(string file)
    {
        if (!File.Exists(file))
            Status.Report("Source file not found.");
        else if ((new FileInfo(file).Length == 0))
            Status.Report("Source file is empty.");
    }
}

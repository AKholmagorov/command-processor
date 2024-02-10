using System.Text.RegularExpressions;

public static class CommandProcessor
{
    public static void ProceedCommands(string line)
    {
        string[] commands = line.Split(' ');

        switch (commands[0])
        {
            case "CF":
                if (CheckName(ref commands[1]))
                    CreateFile.Execute(commands);
                break;
            case "DF":
                if (CheckName(ref commands[1]))
                    DeleteFile.Execute(commands);
                break;
            case "AF":
                if (CheckName(ref commands[1]))
                    AttributeFile.Execute(commands);
                break;
            case "CD":
                if (CheckName(ref commands[1]))
                    CreateDirectory.Execute(commands);
                break;
            case "DD":
                if (CheckName(ref commands[1]))
                    DeleteDirectory.Execute(commands);
                break;
            case "JOIN":
                if (CheckName(ref commands[1]))
                    JoinDirectories.Execute(commands);
                break;
            default:
                Status.Report($"Unknown command: {commands[0]}");
                break;
        }
    }

    public static bool CheckName(ref string name, string regularExp = @"^""[^""].*""$")
    {
        if (Regex.IsMatch(name, regularExp))
        {
            name = name.Remove(0, 1);                 // remove first "
            name = name.Remove(name.Length - 1, 1);   // remove the last "

            return true;
        }
        else
        {
            Status.Report($"File name is incorrect: {name}");
            return false;
        }
    }
}
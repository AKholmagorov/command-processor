/* Shows status of last operation. If not successed it displays error message */

public static class Status
{
    public static int Line = 0;
    public static bool error = false;
    public static string msg = "NULL";

    public static void Report(string message, bool err = true)
    {
        error = err;

        if (error)
            msg = $"\nLine {Status.Line}. " + message + '\n';
        else
            msg = "Success";
    }
}
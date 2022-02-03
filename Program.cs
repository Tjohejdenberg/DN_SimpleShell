// Loop: 
// Read 
// print and take input, if not terminated by eof or something else
//
// Evalutate: 
// parse the input
// create process and wait or dont wait for it to finish

class SimpleShell
{
    public static int Main()
    {
        while (true)
        {
            // Read 21H2
            Console.Write("[SimpleShell]>>> ");

            int result = evaluateCmdLineAndStartProcess();
        }
        return 1;
    }

    private static int setupConsoleAndCtrlCPress() {
        Console.Clear();

        // Setup so that Ctrl+C quits the application
        Console.CancelKeyPress += new ConsoleCancelEventHandler(exitAtCtrlCHandler);
        return 1;
    }

    protected static void exitAtCtrlCHandler(object sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("\n");
    }

    private static int evaluateCmdLineAndStartProcess() 
    {
        string[] args = Console.ReadLine().Split(' ');

        if (args.Last() == "&") 
        {
            Console.WriteLine("Background process");
        } 

        foreach (string arg in args) {
            Console.WriteLine(arg);
        }

        // Start a process assuming that args[0] is a filepath and the other things parameters for that process

        return 1;
    }
}
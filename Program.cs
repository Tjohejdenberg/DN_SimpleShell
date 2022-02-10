using System.Diagnostics;

class SimpleShell
{
    public static void Main()
    {

        int setup = setupConsoleAndCtrlCPress();

        while (true)
        {
            // Read 
            Console.Write("[SimpleShell]>>> ");

            int result = evaluateCmdLineAndStartProcess();
        }
    }

    private static int setupConsoleAndCtrlCPress() {
        Console.Clear();

        // Setup so that Ctrl+C quits the application
        // TODO Pressing ctrl C must stop all running child processes! and reap zombie children
        Console.CancelKeyPress += new ConsoleCancelEventHandler(exitAtCtrlCHandler);
        return 1;
    }

    protected static void exitAtCtrlCHandler(object? sender, ConsoleCancelEventArgs args)
    {
        // it automatically sets the continue property of the process to false and will terminate this process
        // TODO: Call Dispose here? 
        // TODO: Kill child processes in the dispose method or here?

        Console.WriteLine("\n");
    }

    private static int evaluateCmdLineAndStartProcess() 
    {
        /*
        Things to handle:

        1) Press enter -> Nothing happens [OK]
        2) spaces must not be interpreted as arguments [OK]
        3) Support a persistent cahce of standard programs
        4) Possible to toggle if process should go in background or not [OK]
        5) Wrappa felet om användaren skriver ett program som inte finns [OK]
        */

        string rawArgString = Console.ReadLine() ?? "";
        string[] separateArgs = rawArgString
                                .Split(' ')
                                .Select(args => args.Trim())
                                .Where(args => !string.IsNullOrWhiteSpace(args))
                                .ToArray();
        
        int result = 1;
        if (separateArgs != null && separateArgs.Length > 0) { 
            
            string filepath = separateArgs[0];
            string arguments = string.Join(' ',separateArgs[1..]);
            bool isBackGroundProcess = separateArgs.Last() == "&";

            result = StartProcessWithArgs(filepath, arguments, isBackGroundProcess); 
        }
        
        return result;
    }

    private static int StartProcessWithArgs(string filePath, string args, bool backgroundProc = false) {
        try
            {
                using (Process myProcess = new Process())
                {

                    // TODO: Make sure IDisposable stuff is implemented.
                    // TODO: Reap children. No action can leave abandoned processes.

                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = filePath;
                    myProcess.StartInfo.Arguments = args;
                    myProcess.Start();

                    if (!backgroundProc) {
                        myProcess.WaitForExit();
                    } 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        return 0;
    }
}
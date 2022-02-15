using System.Diagnostics;

class Shell
{
    public static void Main()
    {
        Job processGroup = new Job();
        processGroup.AddProcess(Process.GetCurrentProcess().Handle);

        int setup = setupConsoleAndCtrlCPress(processGroup);

        while (true)
        {
            Console.Write("[SimpleShell]>>> ");
            int result = evaluateCmdLineAndStartProcess(processGroup);
        }
    }

    private static int setupConsoleAndCtrlCPress(Job processGroup)
    {
        // Use lambda as adapter to get job parameter in
        Console.CancelKeyPress += new ConsoleCancelEventHandler((sender, args) => exitAtCtrlCHandler(sender, args, processGroup));
        return 0;
    }

    protected static void exitAtCtrlCHandler(object? sender, ConsoleCancelEventArgs args, Job processGroup) 
    {
        // it automatically sets the continue property of the process to false and will terminate this process
        ChildProcesses.DisposeChildProcOfPID((UInt32)Process.GetCurrentProcess().Id);
        processGroup.Dispose();
        // Investigate - it is possible to explicitly terminate all processes in the job, see Windows internals. Would not have to set the kill-limit.
        
        Console.WriteLine("\n");
    }

    private static int evaluateCmdLineAndStartProcess(Job processGroup)
    {
        string rawArgString = Console.ReadLine() ?? "";
        string[] separateArgs = rawArgString
                                .Split(' ')
                                .Select(args => args.Trim())
                                .Where(args => !string.IsNullOrWhiteSpace(args))
                                .ToArray();

        int result = 1;
        if (separateArgs != null && separateArgs.Length > 0)
        {

            string filepath = separateArgs[0];
            string arguments = string.Join(' ', separateArgs[1..]);
            bool isBackGroundProcess = separateArgs.Last() == "&";

            result = StartProcess(processGroup, filepath, arguments, isBackGroundProcess);
        }

        return result;
    }

    private static int StartProcess(Job processGroup, string filePath, string args, bool backgroundProc = false)
    {
        try
        {
            using (Process newProcess = ChildProcesses.StartProcessWithArgs(filePath, args))
            {
                processGroup.AddProcess(newProcess.Handle);

                if (!backgroundProc)
                {
                    newProcess.WaitForExit();
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



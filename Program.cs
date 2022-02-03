// Loop: 
// Read 
// print and take input, if not terminated by eof or something else
//
// Evalutate: 
// parse the input
// create process and wait or dont wait for it to finish
using System;
using System.Diagnostics;
using System.ComponentModel;


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

        StartProcessWithArgs(args[0]);

        foreach (string arg in args) {
            Console.WriteLine(arg);
        }

        // Start a process assuming that args[0] is a filepath and the other things parameters for that process

        return 1;
    }

    private static void StartProcessWithArgs(string filePath) {
        try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.RedirectStandardOutput = true; // Verkar som att man också måste fixa någon streamreader som läser från 
                    // processens standard output.
                    myProcess.StartInfo.FileName = filePath;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    // This code assumes the process you are starting will terminate itself.
                    // Given that it is started without a window so you cannot terminate it
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}
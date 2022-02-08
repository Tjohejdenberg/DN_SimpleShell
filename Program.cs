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
    public static void Main()
    {
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
        Console.WriteLine("\n");
    }

    private static int evaluateCmdLineAndStartProcess() 
    {
        /*
        Things to handle:

        1) Press enter -> Nothing happens [OK]
        2) spaces must not be interpreted as arguments [OK]
        3) Support a persistent cahce of standard programs
        4) Possible to toggle if process should go in background or not
        5) Wrappa felet om användaren skriver ett program som inte finns
        */
        


        string allArgs = Console.ReadLine() ?? "";
        string[] trimmedArgs = allArgs
                                .Split(' ')
                                .Select(args => args.Trim())
                                .Where(args => !string.IsNullOrWhiteSpace(args))
                                .ToArray();
        
        int result = 1;
        if (trimmedArgs != null && trimmedArgs.Length > 0) { 
            result = StartProcessWithArgs(trimmedArgs[0]); 
        }
        
        return result;
    }

    private static int StartProcessWithArgs(string filePath) {
        try
            {
                using (Process myProcess = new Process())
                {

                    // TODO: Make sure IDisposable stuff is implemented.
                    /*
                    Det är lite småkomplicerat det här. Kör jag WaitForExit() så får jag bra beteende för kommandoprogram som behöver interageras med:

                    [SimpleShell]>>> ".\SayHello.exe"
                    ".\SayHello.exe"
                    SHello!

                    [SimpleShell]>>>

                    men om jag tar bort det för att låta en process köra i bakgrunden så får jag 

                    [SimpleShell]>>> ".\SayHello.exe"
                    ".\SayHello.exe"
                    [SimpleShell]>>> SHello!

                    dvs outputen hamnar fel. Då måste jag nog köra att jag slangar om den med någon slags handler i bakgrunden.
                    

                    */

                    if (true) {
                        // Synchronous program
                        // TODO: actually pass arguments...
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = filePath;
                        myProcess.Start();
                        myProcess.WaitForExit();
                    } else {
                        // Asynchronous program
                        // Måste man ha outputen om man kör i bakgrunden?
                            // * KAnske låter den kastas bort om inte användaren specar vilken fil den ska skrivas ner till exv.
                        /* //myProcess.Arguments = args; // Take args as parameter 
                        myProcess.StartInfo.UseShellExecute = false;
                        //myProcess.StartInfo.RedirectStandardOutput = true; 
                        //myProcess.StartInfo.RedirectStandardInput = true;
                        myProcess.StartInfo.FileName = filePath;
                        //myProcess.StartInfo.CreateNoWindow = true;
                        
                        // Set our event handler to asynchronously read the sort output.
                        //myProcess.OutputDataReceived += MyOutputHandler;
                        
                        myProcess.Start();

                        //myProcess.BeginOutputReadLine();
                        
                        //myProcess.WaitForExit(); */
                    }

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        return 0;
    }

    /* private static void MyOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        } */
}
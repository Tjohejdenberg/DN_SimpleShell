using System.Management;
using System.Diagnostics;

static public class ChildProcesses
{
    static public Process StartProcessWithArgs(string filePath, string args)
    {

        Process myProcess = new Process();
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.FileName = filePath;
        myProcess.StartInfo.Arguments = args;
        myProcess.Start();

        return myProcess;
    }

    public static void DisposeChildProcOfPID(UInt32 parentProcessId)
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(
            "SELECT * " +
            "FROM Win32_Process " +
            "WHERE ParentProcessId=" + parentProcessId);
        ManagementObjectCollection collection = searcher.Get();
        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                UInt32 childProcessId = (UInt32)item["ProcessId"];
                if ((int)childProcessId != Process.GetCurrentProcess().Id)
                {
                    DisposeChildProcOfPID(childProcessId);

                    Process childProcess = Process.GetProcessById((int)childProcessId);
                    childProcess.Dispose();
                }
            }
        }
    }
}



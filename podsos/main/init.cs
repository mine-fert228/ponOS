using podsos;
using podsos.MyPonOS;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

class Init
{
    
    public static void start()
    {
        Kernel.BeforeRun();
        
        while (true)
        {
            Thread.Sleep(100);
            Kernel.Run();
        }
    }
    public static void shutdown()
    {
        Process.GetCurrentProcess().Kill();
    }
    public static void reboot()
    {

        string dir = Environment.CurrentDirectory;
        File.WriteAllText($"{dir}\\start.bat", $"start {Assembly.GetExecutingAssembly().Location}");
        Process.Start($"{dir}\\start.bat");
        Environment.Exit(0);
    }

}

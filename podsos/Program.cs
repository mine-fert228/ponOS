using System;

namespace podsos
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("starting subsystem");
            Console.WriteLine($"pc name: {Environment.MachineName}");
            Console.WriteLine($"path to virtual(synthetic) disk: {Environment.CurrentDirectory}");
            Init.start();
        }
    }
}
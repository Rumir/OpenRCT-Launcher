using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Crash_Catcher
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("test1");
                Process x = Process.Start(@"C:\Users\Fire\Documents\visual studio 2015\Projects\Crasher\Crasher\bin\Debug\Crasher.exe");

                Thread.Sleep(1000);
                Console.WriteLine(x.ProcessName);
                while (x.Responding)
                {
                    ;
                }
                x.Kill();
                Thread.Sleep(1000);
            }
            
        }
    }
}

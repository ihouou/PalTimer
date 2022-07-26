using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ModuleAddrX64Delegate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                int pid = -1;
                if (int.TryParse(args[0], out pid))
                {
                    try
                    {
                        Process p = Process.GetProcessById(pid);
                        if (args.Length > 1)
                        {
                            //指定modules名
                            string mn = args[1];
                            long res = 0;
                            foreach (ProcessModule pm in p.Modules)
                            {
                                if (pm.ModuleName == mn)
                                {
                                    res = pm.BaseAddress.ToInt64();
                                    break;
                                }
                            }
                            Console.WriteLine(res);
                            Environment.Exit(0);
                        }
                        else
                        {
                            //拿MainModule
                            Console.WriteLine(p.MainModule.BaseAddress.ToInt64());
                            Environment.Exit(0);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("E");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("E");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("E");
                Environment.Exit(1);
            }
        }
    }
}

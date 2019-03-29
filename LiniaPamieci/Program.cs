using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace LiniaPamieci
{
    class Program
    {

        unsafe private static void licz(object data)
        {
            Parameters p = (Parameters)data;
            for (int i = 0; i < 1000000; i++)
            {
                p.tab[p.indeks] += 3;
            }
        }

        unsafe static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Stopwatch watch = new Stopwatch();
            int* tab = (int*)Marshal.AllocHGlobal(sizeof(int) * 256);
            int maxHopes = 32;
            int cacheLineSize = sizeof(int) * maxHopes;
            double currentSum = 0;
            int counter = 0;
            Console.WriteLine("Sizeof element: {0} bytes, sizeof pointer: {1} bytes", sizeof(int), sizeof(int*));
            for (int i = 0; i < maxHopes; i++)
            {
                double currentAvg = currentSum / (i > 0 ? i : 1);
                Parameters p1 = new Parameters(tab, i);
                Parameters p2 = new Parameters(tab, i + 1);
                double avg = 0;
                for (int k = 0; k < 20; k++)
                {
                    Thread t1 = new Thread(new ParameterizedThreadStart(licz));
                    Thread t2 = new Thread(new ParameterizedThreadStart(licz));
                    t1.Priority = ThreadPriority.Highest;
                    t2.Priority = ThreadPriority.Highest;
                    watch.Start();
                    t1.Start(p1);
                    t2.Start(p2);
                    t1.Join();
                    t2.Join();
                    watch.Stop();
                    tab[i] = 0;
                    tab[i + 1] = 0;
                    avg += watch.ElapsedMilliseconds;
                    watch.Reset();
                }
                avg /= 20;
                if (i > 0 && Math.Abs(avg - currentAvg) > (0.4 * currentAvg))
                {
                    counter++;
                    currentSum += currentAvg;
                }
                else
                {
                    currentSum += avg;
                }

                int* p = &tab[i];
                Console.WriteLine("Threads operate at indexes: {0} and {1} in average time: {2}ms\t|\telements adresses: {3}, {4}", i, i + 1, avg, (long)p, (long)(p + 1));

            }
            Marshal.FreeHGlobal((IntPtr)tab);
            if (counter == 0)
            {
                Console.WriteLine("Nie udało się obliczyć długości linii pamięci. Wyłącz kurwa optymalizacje kodu albo wypierdol ten komputer");
            }
            else
            {
                Console.WriteLine("Cache line size is: {0} bytes", (cacheLineSize / counter));
            }
            Console.ReadKey();
        }
    }
}

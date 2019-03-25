using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiniaPamieci
{
    class Program
    {
        private static Stopwatch watch;
        private static void licz(object data)
        {
            Parameters p = (Parameters)data;
            for(int i = 0; i < 20000000; i++)
            {
                p.tab[p.indeks] += 1;
            }
        }
        static void Main(string[] args)
        {
            int[] tab = new int[256];
            watch = new Stopwatch();
            Thread[] threads = new Thread[2];
            Parameters[] param = new Parameters[2];
            int first = 0;
            int last = 63;
            Thread.Sleep(500);
            while(last >= 7)
            {
                double avg = 0;
                param[0] = new Parameters(ref tab, first);
                param[1] = new Parameters(ref tab, last);
                for (int i = 0; i < 100; i++)
                {
                    threads[0] = new Thread(new ParameterizedThreadStart(licz));
                    threads[1] = new Thread(new ParameterizedThreadStart(licz));
                    watch.Start();
                    threads[0].Start(param[0]);
                    threads[1].Start(param[1]);
                    threads[0].Join();
                    threads[1].Join();
                    watch.Stop();
                    avg += watch.ElapsedMilliseconds;
                    watch.Reset();
                }
                Console.WriteLine("Time elapsed: {0}ms", avg / 100);
                Console.WriteLine("tab[{0}] = {1}, tab[{2}] = {3}", first, tab[first], last, tab[last]);
                tab[first] = 0;
                last -= 8;
            }
            //unsafe
            //{
            //    fixed(int* pointer = &tab[0])
            //    {
            //        Console.WriteLine("Adresy: {0}, {1}, {2}, {3}", (int)pointer, (int)pointer + 1, (int)pointer + 2, (int)pointer + 3);
            //        Console.WriteLine("Wartości {0}, {1}, {2}, {3}", *pointer, *(pointer + 1), *(pointer + 2), *(pointer + 3));
            //    }

            //}
            Console.WriteLine("Koniec");
            Console.ReadKey();
        }
    }
}

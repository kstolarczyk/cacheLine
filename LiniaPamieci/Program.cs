using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            
            for(int i = 0; i < 5000000; i++)
            {
                p.tab[p.indeks] += 1;
            }
        }
        private static int[] s_counter = new int[1024];
        private static void UpdateCounter(object data)
        {
            Parameters p = (Parameters)data;
            Stopwatch s = new Stopwatch();
            s.Start();

            for (int j = 0; j < 100000000; j++)
            {
                s_counter[p.indeks] = s_counter[p.indeks] + 3;
            }

            s.Stop();
            Console.WriteLine("Thread{0} time: {1}ms", Thread.CurrentThread.ManagedThreadId, s.ElapsedMilliseconds);
        }
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            int first = 8;
            int second = 24;
            int third = 40;
            int last = 56;
            Parameters param1 = new Parameters(ref s_counter, first);
            Parameters param2 = new Parameters(ref s_counter, second);
            Parameters param3 = new Parameters(ref s_counter, third);
            Parameters param4 = new Parameters(ref s_counter, last);
            double avg = 0;
            for (int i = 0; i < 3; i++)
            {
                Thread thread1 = new Thread(new ParameterizedThreadStart(UpdateCounter));
                Thread thread2 = new Thread(new ParameterizedThreadStart(UpdateCounter));
                Thread thread3 = new Thread(new ParameterizedThreadStart(UpdateCounter));
                Thread thread4 = new Thread(new ParameterizedThreadStart(UpdateCounter));
                watch.Start();
                thread1.Start(param1);
                thread2.Start(param2);
                thread3.Start(param3);
                thread4.Start(param4);
                thread1.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();
                watch.Stop();
                avg += watch.ElapsedMilliseconds;
                s_counter[first] = 0;
                s_counter[second] = 0;
                s_counter[third] = 0;
                s_counter[last] = 0;
                watch.Reset();
            }
            unsafe
            {
                Console.WriteLine("size of pointer: {0} bytes", sizeof(int*));
                Console.WriteLine("size of int: {0} bytes", sizeof(int));
            }
            Console.WriteLine("Average time: {0}ms", avg / 3);
            Console.WriteLine("Koniec");
            Console.ReadKey();
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01 {
    class Program {
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = new long[1000000000];
        static int numthread = 64;

        static int ReadData() {
            int returnData = 0;
            FileStream fs = new FileStream("C:/Users/akara/Documents/playground/CS Case Optimize/Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se) {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally {
                fs.Close();
            }

            return returnData;
        }
        static void sum(int G_index,int i) {
            if (Data_Global[G_index] % 2 == 0) {
                Sum_Global[i] -= Data_Global[G_index];
            }
            else if (Data_Global[G_index] % 3 == 0) {
                Sum_Global[i] += (Data_Global[G_index]*2);
            }
            else if (Data_Global[G_index] % 5 == 0) {
                Sum_Global[i] += (Data_Global[G_index] / 2);
            }
            else if (Data_Global[G_index] %7 == 0) {
                Sum_Global[i] += (Data_Global[G_index] / 3);
            }
            Data_Global[G_index] = 0;  
        }
        static void sthread(int j) {
            int i;
            int x = (1000000000/numthread)*j;
            int y = (1000000000/numthread)*(j+1);
            for (i = x; i < y; i++)
                sum(i,j);
        }
        static void Main(string[] args) {
            Stopwatch sw = new Stopwatch();
            int y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0) {
                Console.WriteLine("Complete.");
            }
            else {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.WriteLine("Thread Count {0}", numthread);
            Console.Write("\n\nWorking...");
            Thread[] myThreads = new Thread[numthread];
            sw.Start();
            for (int j = 0; j < numthread; j++) {
                int l=j;
                myThreads[j] = new Thread(() => sthread(l));
                myThreads[j].Start();
            }
            for (int j = 0; j < numthread; j++) {
                myThreads[j].Join();
            }
            /*int i;
            for (i = 0; i < 1000000000; i++)
                sum();*/
            for(int i = 1;i<numthread;i++){
                Sum_Global[0]+=Sum_Global[i];
            }
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global[0]);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}

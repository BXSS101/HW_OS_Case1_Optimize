using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01 {
    class Program {
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = {0, 0, 0, 0};

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
        static void sum(int ind, int n) {
            if (Data_Global[ind] % 2 == 0) {
                Sum_Global[n] -= Data_Global[ind];
            }
            else if (Data_Global[ind] % 3 == 0) {
                Sum_Global[n] += (Data_Global[ind]*2);
            }
            else if (Data_Global[ind] % 5 == 0) {
                Sum_Global[n] += (Data_Global[ind] / 2);
            }
            else if (Data_Global[ind] %7 == 0) {
                Sum_Global[n] += (Data_Global[ind] / 3);
            }
            Data_Global[ind] = 0; 
        }
        static void newThreadA() {
            for (int i = 0; i < 250000000; i++) 
                sum(i,0);
        }
        static void newThreadB() {
            for (int j = 250000000; j < 500000000; j++)
                sum(j,1);
        }
        static void newThreadC() {
            for (int i = 500000000; i < 750000000; i++) 
                sum(i,2);
        }
        static void newThreadD() {
            for (int j = 750000000; j < 1000000000; j++)
                sum(j,3);
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
            Console.Write("\n\nWorking...");
            Thread th1 = new Thread(newThreadA);
            Thread th2 = new Thread(newThreadB);
            Thread th3 = new Thread(newThreadC);
            Thread th4 = new Thread(newThreadD);
            sw.Start();
            th1.Start();
            th2.Start();
            th3.Start();
            th4.Start();
            /*for (i = 0; i < 1000000000; i++)
                sum();*/
            th1.Join();
            th2.Join();
            th3.Join();
            th4.Join();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global[0] + Sum_Global[1] + Sum_Global[2] + Sum_Global[3]);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}

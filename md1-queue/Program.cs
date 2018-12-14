using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace md1_queue
{
    class Program
    {
        /*
        public static double fact(double n)
        {
            if (n == 1)
                return n;
            if (n == 0)
                return 1;
            return n * fact(n - 1);
        }
        */

        public static void Log()
        {
            //Console.WriteLine("Thread {0} Start", Thread.CurrentThread.Name);
            Thread.Sleep(1);
            //Console.WriteLine("Thread {0} Stop", Thread.CurrentThread.Name);
        }

        static void Main(string[] args)
        {

            while (1==1)
            {
                Console.Clear();
                double lambda = 1; //8.0 / 24.0;
                double tn = 10.0;
                double t = 0.0;
                double n = 0.0;

                Random rand = new Random();
                List<double> S = new List<double>();

                while (t < tn)
                {
                    double r = rand.NextDouble();//rand.Next() % 2;
                    t = t + (-1 / lambda) * Math.Log(r);
                    n = n + 1;
                    S.Add(t);
                    Console.WriteLine("#{0} {1}", n, t);
                }

                for (int i = 0; i < S.Count; i++)
                {
                    Thread myThread = new Thread(Log);
                    myThread.Name = i.ToString();
                    myThread.Start();
                }

                Console.WriteLine("labda = {0}", lambda);
                Console.WriteLine("");

                Console.Write("\nPress any key to continue... ");
                Console.ReadKey();
            }

            /*
            Console.Write("Число каналов n = ");
            double n = Double.Parse(Console.ReadLine());

            Console.Write("Длина очереди m = ");
            double m = Double.Parse(Console.ReadLine());

            Console.Write("Кол-во заявок k = ");
            double k = Double.Parse(Console.ReadLine());

            Console.Write("Время на одну заявку t(min) = ");
            double t = Double.Parse(Console.ReadLine());

            double lambda = k / (60 * 24);
            Console.WriteLine("labda = {0}", lambda);

            double mu = 1 / t;
            Console.WriteLine("mu = {0}", mu);

            double psi = lambda / (n * mu);
            Console.WriteLine("psi = {0}", psi);

            double sum = 0.0;

            for (int i = 0; i <= n; i++)
            {
                sum += (Math.Pow(n, i) / fact(i)) * Math.Pow(psi, i);
            }

            double p0 = Math.Pow(sum + ((n / fact(n)) * ((Math.Pow(psi, n + 1) * (1 - Math.Pow(psi, m))) / (1 - psi))), -1);
            Console.WriteLine("p0 = {0}", p0 );

            double pr = (Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + m) * p0;
            Console.WriteLine("pr = {0}", pr);

            double q = 1 - pr;
            Console.WriteLine("Q = {0}", q);

            double a = lambda * q;
            Console.WriteLine("A = {0}", a);

            double ns = a / mu;
            Console.WriteLine("Ns = {0}", ns);

            double nl = ((Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + 1)) * ((1 - Math.Pow(psi, m) * (m + 1 - m * psi)) / Math.Pow(1 - psi, 2)) * p0;
            Console.WriteLine("Nline = {0}", nl);

            double n0 = ns + nl;
            Console.WriteLine("N = {0}", n0);

            double ts = ns / lambda;
            Console.WriteLine("Ts = {0}", ts);

            double tl = nl / lambda;
            Console.WriteLine("Tline = {0}", tl);

            double t0 = ts + tl;
            Console.WriteLine("T = {0}", t0);

            //double p0 = Math.Pow(1 + (Math.Pow(n, 1) / fact(1)) + (Math.Pow(n, 2) / fact(2)) + (Math.Pow(n, 3) / fact(3)) + (Math.Pow(n, 4) / fact(4)) * 4, -1);
            //

            //Console.WriteLine("p0 = {0}", p0);

            //for (int i = 1; i < 500; i++)
            //{
            //    Reader reader = new Reader(i);
            //}

            Console.Write("\nPress any key to continue... ");
            Console.ReadKey();
            */
        }
    }
}

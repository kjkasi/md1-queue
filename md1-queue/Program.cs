using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace md1_queue
{
    class Client
    {
        public string FullName { get; set; }
    }

    class md1
    {
        private Queue<Client> queue { get; set; }
        public double lambda { get; set; }
        public double tn { get; set; }
        public double n { get; set; }
        public double m { get; set; }
        public double w { get; set; }
        static Semaphore sem { get; set; }
        private int Count { get; set; }
        public List<string> log { get; set; }
        
        ~md1()
        {
            System.IO.File.WriteAllLines(@"./log.txt", log);
            Console.Write("\nPress any key to continue... ");
            Console.ReadKey();
        }

        private void doWork()
        {
            //Count++;
            sem.WaitOne();
            Console.WriteLine("{0} Start {1} {2}", DateTime.Now, Thread.CurrentThread.Name, Count);
            log.Add(DateTime.Now + " Start " + Thread.CurrentThread.Name + " " + Count);
            Thread.Sleep((int)w * 1000);
            sem.Release();
            Count--;
            Console.WriteLine("{0} Stop {1} {2}", DateTime.Now, Thread.CurrentThread.Name, Count);
            log.Add(DateTime.Now + " Stop " + Thread.CurrentThread.Name + " " + Count);
        }

        public md1(double lambda = 1, double tn = 10, double n = 3, double m = 6, double w = 5)
        {
            this.lambda = lambda;
            this.tn = tn;
            this.n = n;
            this.m = m;
            this.w = w;
            Count = 0;
            queue = new Queue<Client>((int)m);
            sem = new Semaphore((int)n, (int)n);
        }

        public void addClient(Client client)
        {
            if (Count < m)
            {
                Count++;
                Console.WriteLine("{0} Add {1} {2}", DateTime.Now, client.FullName, Count);
                log.Add(DateTime.Now + " Add " + client.FullName + " " + Count);
                queue.Enqueue(client);
                Exec();
            }
            else
            {
                Console.WriteLine("{0} Abort {1} {2}", DateTime.Now, client.FullName, Count);
                log.Add(DateTime.Now + " Abort " + client.FullName + " " + Count);
            }
        }

        private void Exec()
        {
            Thread thread = new Thread(doWork);
            thread.Name = queue.Dequeue().FullName;
            thread.Start();
        }

        public void GenPoisson()
        {
            double t = 0.0;
            int index = 0;
            Random rand = new Random();
            while (t < tn)
            {
                double tau = (-1 / lambda) * Math.Log(rand.NextDouble());
                t = t + tau;
                Client client = new Client();
                client.FullName = index.ToString();
                //Console.WriteLine("{0} Generate {1}", DateTime.Now, client.FullName);
                addClient(client);
                Thread.Sleep((int)tau * 1000);
                index++;
            }
        }

        public void GenArcSin()
        {
            double t = 0.0;
            int index = 0;
            Random rand = new Random();
            while (t < tn)
            {
                double tau = (1 / w) + lambda * Math.Sin(Math.PI * (rand.NextDouble() - 0.5));
                t = t + tau;
                Client client = new Client();
                client.FullName = index.ToString();
                //Console.WriteLine("{0} Generate {1}", DateTime.Now, client.FullName);
                addClient(client);
                Thread.Sleep((int)tau * 1000);
                index++;
            }
        }
    }

    class Program
    {
        static double fact(double n)
        {
            if (n == 1)
                return n;
            if (n == 0)
                return 1;
            return n * fact(n - 1);
        }

        static void Main(string[] args)
        {
            List<string> log = new List<string>();

            Console.Write("Интенсивность потока Lambda(1) = ");
            double lambda = Double.Parse(Console.ReadLine());
            log.Add("Интенсивность потока Lambda(1) = " + lambda.ToString());

            Console.Write("Время моделирования Tn(10) = ");
            double tn = Double.Parse(Console.ReadLine());
            log.Add("\nВремя моделирования Tn(10) = " + tn.ToString());

            Console.Write("Число каналов n(3) = ");
            double n = Double.Parse(Console.ReadLine());
            log.Add("\nЧисло каналов n(3) = " + n.ToString());

            Console.Write("Длина очереди m(6) = ");
            double m = Double.Parse(Console.ReadLine());
            log.Add("\nДлина очереди m(6) = " + m.ToString());

            Console.Write("Время на заявку t(5) = ");
            double t = Double.Parse(Console.ReadLine());
            log.Add("\nВремя на заявку t(5) = " + t.ToString());

            Console.WriteLine();
            log.Add("");

            double mu = 1 / t;
            Console.WriteLine("mu = {0}", mu);
            log.Add("mu = " + mu.ToString());

            double psi = lambda / (n * mu);
            Console.WriteLine("psi = {0}", psi);
            log.Add("psi = " + psi.ToString());

            double sum = 0.0;
            for (int i = 0; i <= n; i++)
            {
                sum += (Math.Pow(n, i) / fact(i)) * Math.Pow(psi, i);
            }

            double p0 = Math.Pow(sum + ((n / fact(n)) * ((Math.Pow(psi, n + 1) * (1 - Math.Pow(psi, m))) / (1 - psi))), -1);
            Console.WriteLine("p0 = {0}", p0);
            log.Add("p0 = " + p0.ToString());

            double pr = (Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + m) * p0;
            Console.WriteLine("pr = {0}", pr);
            log.Add("p0 = " + p0.ToString());

            double q = 1 - pr;
            Console.WriteLine("Q = {0}", q);
            log.Add("Q = " + q.ToString());

            double a = lambda * q;
            Console.WriteLine("A = {0}", a);
            log.Add("a = " + a.ToString());

            double ns = a / mu;
            Console.WriteLine("Ns = {0}", ns);
            log.Add("ns = " + ns.ToString());

            double nl = ((Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + 1)) * ((1 - Math.Pow(psi, m) * (m + 1 - m * psi)) / Math.Pow(1 - psi, 2)) * p0;
            Console.WriteLine("Nline = {0}", nl);
            log.Add("nl = " + nl.ToString());

            double n0 = ns + nl;
            Console.WriteLine("N = {0}", n0);
            log.Add("n0 = " + n0.ToString());

            double ts = ns / lambda;
            Console.WriteLine("Ts = {0}", ts);
            log.Add("ts = " + ts.ToString());

            double tl = nl / lambda;
            Console.WriteLine("Tline = {0}", tl);
            log.Add("tl = " + tl.ToString());

            double t0 = ts + tl;
            Console.WriteLine("T = {0}", t0);
            Console.WriteLine();
            log.Add("");
            log.Add("t0 = " + t0.ToString());

            System.IO.File.WriteAllLines(@"./log.txt", log);

            md1 md = new md1(lambda, tn, n, m, t);
            md.log = log;
            md.GenPoisson();
            //md.GenArcSin();
  
        }
    }
}

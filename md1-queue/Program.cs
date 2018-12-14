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
        /*
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        */
        public string FullName { get; set; }

        static string GetFirstName()
        {
            List<string> names = new List<string>();
            names.Add("Вася");
            names.Add("Женя");
            names.Add("Саня");
            names.Add("Коля");
            Random rand = new Random();
            return names[(rand.Next() % names.Count())];
        }

        static string GetLastName()
        {
            List<string> names = new List<string>();
            names.Add("Иванов");
            names.Add("Петров");
            names.Add("Сидоров");
            names.Add("Кароль");
            Random rand = new Random();
            return names[(rand.Next() % names.Count())];
        }

        public Client()
        {
            //this.FirstName = GetFirstName();
            //this.LastName = GetLastName();
        }
    }

    class md1
    {
        private Queue<Client> queue { get; set; }
        public double lambda { get; set; }
        public double tn { get; set; }
        public int n { get; set; }
        public int m { get; set; }
        public int w { get; set; }
        static Semaphore sem { get; set; }
        private int Count { get; set; }

        private void doWork()
        {
            //Count++;
            sem.WaitOne();
            Console.WriteLine("{0} Start {1} {2}", DateTime.Now, Thread.CurrentThread.Name, Count);
            Thread.Sleep(w);
            sem.Release();
            Count--;
            Console.WriteLine("{0} Stop {1} {2}", DateTime.Now, Thread.CurrentThread.Name, Count);
        }

        public md1(double lambda = 1, double tn = 10, int n = 3, int m = 6, int w = 5000)
        {
            this.lambda = lambda;
            this.tn = tn;
            this.n = n;
            this.m = m;
            this.w = w;
            Count = 0;
            queue = new Queue<Client>(m);
            sem = new Semaphore(n, n);
        }

        public void addClient(Client client)
        {
            if (Count < m)
            {
                Count++;
                Console.WriteLine("{0} Add {1} {2}", DateTime.Now, client.FullName, Count);
                queue.Enqueue(client);
                Exec();
            }
            else
            {
                Console.WriteLine("{0} Abort {1} {2}", DateTime.Now, client.FullName, Count);
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

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            md1 md = new md1();
            md.GenPoisson();

            //Console.Write("\nPress any key to continue... ");
            Console.ReadKey();

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

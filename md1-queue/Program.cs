﻿using System;
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
        public enum Type { Poisson, ArcSin };
        public Type type { get; set; }
        private Queue<Client> queue { get; set; }
        public double lambda { get; set; }
        public double tn { get; set; }
        public double n { get; set; }
        public double m { get; set; }
        public double w { get; set; }
        static Semaphore sem { get; set; }
        private int Count { get; set; }
        public List<string> log { get; set; }
        public List<double> ls1 { get; set; }
        public List<double> ts1 { get; set; }
        Random rand { get; set; }

        static double fact(double n)
        {
            if (n == 1)
                return n;
            if (n == 0)
                return 1;
            return n * fact(n - 1);
        }

        ~md1()
        {
            Console.WriteLine();
            log.Add("");

            double lambda = ls1.Average();
            Console.WriteLine("lambda = {0}", lambda);
            log.Add("lambda = " + lambda.ToString());

            
            double w = ts1.Average();
            Console.WriteLine("t = {0}", w);
            log.Add("t = " + w.ToString());

            double mu = 1.0 / w;
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

            double p0 = Math.Pow(sum + ((n / fact(n)) * ((Math.Pow(psi, n + 1) * (1.0 - Math.Pow(psi, m))) / (1.0 - psi))), -1.0);
            Console.WriteLine("p0 = {0}", p0);
            log.Add("p0 = " + p0.ToString());

            double pr = (Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + m) * p0;
            Console.WriteLine("pr = {0}", pr);
            log.Add("pr = " + p0.ToString());

            double q = 1 - pr;
            Console.WriteLine("Q = {0}", q);
            log.Add("Q = " + q.ToString());

            double a = lambda * q;
            Console.WriteLine("A = {0}", a);
            log.Add("a = " + a.ToString());

            double ns = a / mu;
            Console.WriteLine("Ns = {0}", ns);
            log.Add("ns = " + ns.ToString());

            double nl = ((Math.Pow(n, n) / fact(n)) * Math.Pow(psi, n + 1)) * ((1.0 - Math.Pow(psi, m) * (m + 1.0 - m * psi)) / Math.Pow(1 - psi, 2)) * p0;
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
            log.Add("t0 = " + t0.ToString());
            log.Add("");

            System.IO.File.WriteAllLines(@"./log.txt", log);
            //Console.Write("\nPress any key to continue... ");
            //Console.ReadKey();
        }

        private void doWork()
        {
            //Count++;
            sem.WaitOne();
            double ts;
            if (type == Type.Poisson)
            {
                ts = GenPoisson(1.0 / w);
            }
            else
            {
                ts = GenArcSin(1.0 / w);
            }
            this.ts1.Add(ts);
            Console.WriteLine("{0} Start {1} {2} {3}", DateTime.Now, Thread.CurrentThread.Name, Count, ts);
            log.Add(DateTime.Now + " Start " + Thread.CurrentThread.Name + " " + Count + " " + ts);
            Thread.Sleep((int)ts * 1000);
            sem.Release();
            Count--;
            Console.WriteLine("{0} Stop {1} {2}", DateTime.Now, Thread.CurrentThread.Name, Count);
            log.Add(DateTime.Now + " Stop " + Thread.CurrentThread.Name + " " + Count);
        }

        public md1(double lambda, double tn, double n, double m, double w, Type tp)
        {
            this.lambda = lambda;
            this.tn = tn;
            this.n = n;
            this.m = m;
            this.w = w;
            Count = 0;
            queue = new Queue<Client>((int)m);
            sem = new Semaphore((int)n, (int)n);
            ls1 = new List<double>();
            ts1 = new List<double>();
            rand = new Random();
            type = tp;
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

        public void Start()
        {
            double t = 0.0;
            int index = 0;
            Random rand = new Random();
            while (t < tn)
            {
                double tau = GenPoisson(lambda);
                //Console.WriteLine("tau {0} {1}", index, tau);
                ls1.Add(tau);
                t = t + tau;
                Client client = new Client();
                client.FullName = index.ToString();
                addClient(client);
                Thread.Sleep((int)tau * 1000);
                index++;
            }
            //Console.Write("\nPress any key to continue... ");
            //Console.ReadKey();
        }

        public double GenPoisson(double value)
        {
            return (-1 / value) * Math.Log(rand.NextDouble());
        }

        public double GenArcSin(double value)
        {
            return (1.0 / value) + lambda * Math.Sin(Math.PI * (rand.NextDouble() - 0.5));
        }
    }

    class Program
    {
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

            //System.IO.File.WriteAllLines(@"./log.txt", log);

            md1 md = new md1(lambda, tn, n, m, t, md1.Type.Poisson); //Тут выбираешь тип
            md.log = log;
            md.Start();
            //Console.ReadKey();

        }
    }
}

using System;
using System.Net.Sockets;
using Communication;

namespace Client
{
    public class Client
    {
        private string hostname;
        private int port;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
        }

        public void start()
        {
            TcpClient comm = new TcpClient(hostname, port);
            Console.WriteLine("Connection established");
            while (true)
            {
                string expr = Console.ReadLine();
                Console.WriteLine(expr);

                string[] tabOp = expr.Split(' ');
                double op1 = double.Parse(tabOp[0]);
                double op2 = double.Parse(tabOp[2]);
                char op = char.Parse(tabOp[1]);

                Net.sendMsg(comm.GetStream(), new Expr(op1, op2, op));
                Console.WriteLine("Result = " + (Result)Net.rcvMsg(comm.GetStream()));

            }
        }
    }
}

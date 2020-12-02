using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication;

namespace Server
{
    public class Server
    {
        private int port;

        public Server(int port)
        {
            this.port = port;
        }

        public void start()
        {
            TcpListener listener = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            listener.Start();

            while (true)
            {
                TcpClient comm = listener.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                new Thread(new Receiver(comm).doOperation).Start();

            }
        }

        class Receiver
        {
            private TcpClient comm;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {

                /*
                BinaryWriter outs = new BinaryWriter(comm.GetStream());
                BinaryReader ins = new BinaryReader(comm.GetStream());

                
                // read operation
                double op1 = ins.ReadDouble();
                double op2 = ins.ReadDouble();
                char op = ins.ReadChar();
                */

                Console.WriteLine("Computing operation");
                while (true)
                {
                    // read expression
                    Expr msg = (Expr)Net.rcvMsg(comm.GetStream());

                    Console.WriteLine("expression received");
                    // send result
                    switch (msg.Op)
                    {
                        case '+':
                            Net.sendMsg(comm.GetStream(), new Result(msg.Op1 + msg.Op2, false));
                            break;

                        case '-':
                            Net.sendMsg(comm.GetStream(), new Result(msg.Op1 - msg.Op2, false));
                            break;

                        case '*':
                            Net.sendMsg(comm.GetStream(), new Result(msg.Op1 * msg.Op2, false));
                            break;

                        case '/':
                            if (msg.Op2 != 0.0)
                                Net.sendMsg(comm.GetStream(), new Result(msg.Op1 / msg.Op2, false));
                            else
                                Net.sendMsg(comm.GetStream(), new Result(0, true));

                            break;

                    }

                }

            }
        }
    }
}

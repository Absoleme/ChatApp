using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Communication.Status;

namespace Server
{
    public class Receiver
    {
        private Thread receivingThread;
        private Thread sendingThread;

        public Server Server { get; set; }
        public TcpClient Client { get; set; }
        public String Email { get; set; }
        public Status Status { get; set; }
        public List<string> MessageQueue { get; private set; }
        public long TotalBytesUsage { get; set; }
        public Receiver OtherSideReceiver { get; set; }

        public Receiver(TcpClient client, Server server)
        {
            Server = server;
            Client = client;
            Client.ReceiveBufferSize = 1024;
            Client.SendBufferSize = 1024;
        }

        public void Start()
        {
            receivingThread = new Thread(ReceivingMethod);
            receivingThread.IsBackground = true;
            receivingThread.Start();

            sendingThread = new Thread(SendingMethod);
            sendingThread.IsBackground = true;
            sendingThread.Start();
        }

        public void SendMessage(string message)
        {
            MessageQueue.Add(message);
        }

        private void SendingMethod()
        {
            while (Status != Status.Disconnected)
            {
                if (MessageQueue.Count > 0)
                {
                    var message = MessageQueue[0];

                    try
                    {
                        BinaryFormatter f = new BinaryFormatter();
                        f.Serialize(Client.GetStream(), message);
                    }
                    catch
                    {
                        Disconnect();
                    }
                    finally
                    {
                        MessageQueue.Remove(message);
                    }
                }
                Thread.Sleep(30);
            }
        }
        private void ReceivingMethod()
        {
            while (Status != Status.Disconnected)
            {
                if (Client.Available > 0)
                {
                    TotalBytesUsage += Client.Available;

                    try
                    {
                        BinaryFormatter f = new BinaryFormatter();
                        MessageBase msg = f.Deserialize(Client.GetStream()) as MessageBase;
                        OnMessageReceived(msg);
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception("Unknown message received. Could not deserialize 
                        the stream.", e);
                        Debug.WriteLine(ex.Message);
                    }
                }

                Thread.Sleep(30);
            }
        }

        private void Disconnect()
        {
            if (Status == Status.Disconnected) return;

            if (OtherSideReceiver != null)
            {
                OtherSideReceiver.OtherSideReceiver = null;
                OtherSideReceiver.Status = Status.Validated;
                OtherSideReceiver = null;
            }

            Status = Status.Disconnected;
            Client.Client.Disconnect(false);
            Client.Close();
        }
    }
}

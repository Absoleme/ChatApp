using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SharedLibrary;
using SharedLibrary.Messages.Request;
using SharedLibrary.Messages.Response;

namespace Client
{
    public class Client
    {
        private string hostname;
        private int port;
        public List<Message> MessageList { get; private set; }
        private bool connectedStatus;
        private bool connectedInTopic;
        private string globalUsername;
        private string ActualTopicName;
        
        

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
                Console.WriteLine("Please select a feature : ");
                Console.WriteLine("1- Register");
                Console.WriteLine("2- Login");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please entrer your userName");
                        string username = Console.ReadLine();
                        Console.WriteLine("Please entre your password");
                        string pwd = Console.ReadLine();
                        


                        Net.sendMsg(comm.GetStream(), new RegisterRequest(username, pwd));
                        RegisterResponse myRegRespons = (RegisterResponse)Net.rcvMsg(comm.GetStream());
                        Console.WriteLine(myRegRespons.Message);


                        break;
                    case 2:
                        Console.WriteLine("Please entrer your userName");
                        string name = Console.ReadLine();
                        Console.WriteLine("Please entre your password");
                        string pass = Console.ReadLine();
                        

                        Net.sendMsg(comm.GetStream(), new LoginRequest(name, pass));
                        LoginResponse myLogRespons = (LoginResponse)Net.rcvMsg(comm.GetStream());
                        globalUsername = myLogRespons.Username;

                        
                        connectedStatus = myLogRespons.IsConnected;
                        Console.WriteLine(myLogRespons.IsConnected);


                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                while (connectedStatus)
                {
                    Console.WriteLine("Please select a feature : ");
                    Console.WriteLine("1- List Topics");
                    Console.WriteLine("2- Create topics");
                    Console.WriteLine("3- Join topics");
                    

                    int choice2 = int.Parse(Console.ReadLine());

                    switch (choice2)
                    {
                        case 1:
                            Net.sendMsg(comm.GetStream(), new ListTopicsRequest());
                            ListTopicsResponse myListRespons = (ListTopicsResponse)Net.rcvMsg(comm.GetStream());
                            Console.WriteLine("List of topics : ");
                            int i = 1;

                            foreach(Topic t in myListRespons.AllTopic)
                            {
                                Console.WriteLine(i + ")" + t.Title);
                                i++;
                            }
                            
                            break;

                        case 2:
                            Console.WriteLine("Please Enter a title : ");
                            string title = Console.ReadLine();
                            Console.WriteLine("Please Enter the topic content : ");
                            string content = Console.ReadLine();


                            Net.sendMsg(comm.GetStream(), new CreateTopicsRequest(globalUsername, content, title, DateTime.Now));
                            CreateTopicsResponse myCreateTopicResponse = (CreateTopicsResponse)Net.rcvMsg(comm.GetStream());
                            Console.WriteLine(myCreateTopicResponse.Message);

                            break;

                        case 3:
                            Console.WriteLine("Please Enter the title of the topic : ");
                            string titleTopic = Console.ReadLine();
                            Net.sendMsg(comm.GetStream(), new JoinTopicRequest(titleTopic, globalUsername));
                            JoinTopicResponse myJoinTopicResponse = (JoinTopicResponse)Net.rcvMsg(comm.GetStream());
                            ActualTopicName = myJoinTopicResponse.MyTop.Title;
                            Console.WriteLine("-----------------ROOM--------------- :");
                            Console.WriteLine("Title : " + myJoinTopicResponse.MyTop.Title);
                            foreach (User u in myJoinTopicResponse.MyTop.AllUsers)
                            {
                                Console.WriteLine("The users into this login are : " + u.Username);
                            }
                            if (myJoinTopicResponse.IsInTopic)
                            {
                                connectedInTopic = true;
                                connectedStatus = !connectedStatus;
                            }
                             
                            break;

                        
                    }
                }

                while (connectedInTopic)
                {

                    Console.WriteLine("Please enter the number of what you want to do : ");

                    Console.WriteLine("1 - Write a message : ");
                    Console.WriteLine("2 - Exit ");

                    int choice3 = int.Parse(Console.ReadLine());

                    switch (choice3)
                    {
                        case 1:

                            Console.WriteLine("Write your message : ");
                            string topicMessage = Console.ReadLine();

                            Net.sendMsg(comm.GetStream(), new SendMessageTopicRequest(DateTime.Now, globalUsername, ActualTopicName, topicMessage));
                            SenMessageTopicResponse myMessageResponse = (SenMessageTopicResponse)Net.rcvMsg(comm.GetStream());

                            foreach (TopicMessage m in myMessageResponse.MyTopic.Messages)
                            {
                                Console.WriteLine("\n" + m.When + " - " + m.Creator + " : " + m.TopicName + "\n");
                            }
                            break;

                        case 2:
                            connectedInTopic = !connectedInTopic;
                            connectedStatus = !connectedStatus;
                            break;
                    }

                    

                }
            }


        }

    }
}

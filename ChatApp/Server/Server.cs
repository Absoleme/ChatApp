using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SharedLibrary;
using SharedLibrary.Messages.Request;
using SharedLibrary.Messages.Response;

namespace Server
{
    public class Server
    {
        private int port;

        public Server(int port)
        {
            this.port = port;
        }

        public int Port { get => port; set => port = value; }

        public void start()
        {
			Socket server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port); // Contains host information
			server.Bind(endpoint);

			server.Listen(10); // Up to 10 users in a queue
			Console.WriteLine("Waiting for clients on port " + port);

			while (true)
			{
				try
				{
					Socket client = server.Accept();
					ConnectionHandler handler = new ConnectionHandler(client);

					Thread thread = new Thread(new ThreadStart(handler.HandleConnection));
					thread.Start();
				}
				catch (Exception)
				{
					Console.WriteLine("Connection failed on port " + port);
				}
			}
		}


		class ConnectionHandler
		{

			private Socket client;
			private NetworkStream ns;
			

			private static int connections = 0;

			public ConnectionHandler(Socket client)
			{
				this.client = client;
			}

		
			public void HandleConnection()
			{
				try
				{
					ns = new NetworkStream(client);
					connections++;
					Console.WriteLine("New client accepted: {0} active connections", connections);

					Console.WriteLine("Welcome to my server");
				

					while (true)
					{
						Message msg = Net.rcvMsg(ns);

						Type type = msg.GetType();

                        Console.WriteLine(msg.GetType());

						if (type == typeof(LoginRequest))
                        {
                            
                            User myUser = new User(((LoginRequest)msg).UserName, ((LoginRequest)msg).Password);
                            List<User> allUser = User.retrieveUsers();
                            bool safe = false;
                            bool isConnected = false;


                            foreach (User u in allUser)
                            {
                                if (u.Username == myUser.Username && u.Password == myUser.Password)
                                {
									isConnected = true;
									LoginResponse myResp = new LoginResponse(isConnected);
									myResp.Username = myUser.Username;
									
									Net.sendMsg(ns, myResp);
								}
                            }
                            if (!isConnected) {
								Net.sendMsg(ns, new LoginResponse(isConnected));
							}


						}
                        else if(type == typeof(RegisterRequest))
						{
						
							User myUser = new User(((RegisterRequest)msg).UserName, ((RegisterRequest)msg).Password);
							
							List<User> allUser = User.retrieveUsers();
							bool safe = false;

							foreach (User u in allUser)
							{
                                
								if (u.Username == myUser.Username)
								{
									string message = "This username already exist";
									Net.sendMsg(ns, new RegisterResponse(message));
								}
								else
									safe = true;
							}
                            if (safe) { 
								string message = "You're successfully registered";
								allUser.Add(myUser);
								User.saveUsers(allUser);
								Net.sendMsg(ns, new RegisterResponse(message));
							}
							if (allUser.Count == 0)
							{
								string message = "You're successfully registered !";
								allUser.Add(myUser);
								User.saveUsers(allUser);
								Net.sendMsg(ns, new RegisterResponse(message));
							}
						}
						else if(type == typeof(ListTopicsRequest))
                        {
							List<Topic> allTopics = Topic.retrieveTopics();
							
							
							Net.sendMsg(ns, new ListTopicsResponse(allTopics));
                        }
						else if (type == typeof(CreateTopicsRequest))
                        {
							
							Topic myTopic = new Topic(((CreateTopicsRequest)msg).Creator, ((CreateTopicsRequest)msg).Title, ((CreateTopicsRequest)msg).Content, ((CreateTopicsRequest)msg).When);
							List<Topic> allTopic = Topic.retrieveTopics();
							bool safe = false;
							
							foreach (Topic t in allTopic)
                            {
								if (t.Title == myTopic.Title)
								{
									string me = "This Topic title already exist";
									Net.sendMsg(ns, new CreateTopicsResponse(me));
									safe = true;
								}
                            }
                            if (!safe) { 
								string message = "Your topic has been created";
								allTopic.Add(myTopic);
								Topic.saveTopics(allTopic);
								Net.sendMsg(ns, new CreateTopicsResponse(message));
							}
							if(allTopic.Count == 0)
                            {
								string message = "Your topic has been created";
								allTopic.Add(myTopic);
								Topic.saveTopics(allTopic);
								Net.sendMsg(ns, new CreateTopicsResponse(message));
							}
						}
						else if (type == typeof(JoinTopicRequest))
                        {
							Topic myTopic = new Topic(((JoinTopicRequest)msg).Title);
							User aUser = new User(((JoinTopicRequest)msg).Username);
							
							List<Topic> allTopic = Topic.retrieveTopics();
							
							bool userExist = false;
							foreach (Topic t in allTopic)
                            {
                                if (t.Title == myTopic.Title)
                                {

									foreach(User u in t.AllUsers) {
										if (u.Username == aUser.Username)
										{
                                            Console.WriteLine(userExist);
											userExist = !userExist;
										}
									}


                                    if (!userExist)
                                    {
										myTopic.AllUsers.Add(aUser);
										Topic.saveTopics(allTopic);
										
										Net.sendMsg(ns, new JoinTopicResponse(myTopic, true));
									}

								}
                            }
                        }
						else if (type == typeof(SendMessageTopicRequest))
                        {
							TopicMessage myTopicMessage = new TopicMessage(((SendMessageTopicRequest)msg).When, ((SendMessageTopicRequest)msg).Creator, ((SendMessageTopicRequest)msg).TopicName, ((SendMessageTopicRequest)msg).Text);

							Topic myTopic = new Topic(((SendMessageTopicRequest)msg).TopicName);

							myTopic.Messages.Add(myTopicMessage);

							Net.sendMsg(ns, new SenMessageTopicResponse(myTopic));

						}
					}
					ns.Close();
					client.Close();
					connections--;
					Console.WriteLine("Client disconnected: {0} active connections", connections);
				}
				catch (Exception e)
				{
					connections--;
					Console.WriteLine("Client disconnected: {0} active connections", connections);
					Console.WriteLine(e.Message);
				}
			}

		}

	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharedLibrary
{
    [Serializable]
    public class Topic
    {
        private string creator;
        private string title;
        private string content;
        private DateTime when;
        private List<TopicMessage> messages;
        private List<User> allUsers;
        
        
        public Topic(string t)
        {
            this.title = t;
            allUsers = new List<User>();
            messages = new List<TopicMessage>();
        }

        public Topic(string creator, string title, string content, DateTime when)
        {
            this.creator = creator;
            this.title = title;
            this.content = content;
            this.when = when;
            allUsers = new List<User>();
            messages = new List<TopicMessage>();

        }

        public static List<Topic> retrieveTopics()
        {
            try { 
            FileStream fs = new FileStream("topics.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<Topic> topics = (List<Topic>)bf.Deserialize(fs);
            fs.Close();

            return topics;
            }
            catch(FileNotFoundException e)
            {
                return new List<Topic>();
            }

        }

        public static void saveTopics(List<Topic> users)
        {
            FileStream fs = new FileStream("topics.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, users);
            fs.Close();
        }


        public Topic(SerializationInfo info, StreamingContext ctxt)
        {
            when = (DateTime)info.GetValue("when", typeof(DateTime));
            creator = (string)info.GetValue("creator", typeof(Guid));
            title = (string)info.GetValue("title", typeof(string));
            content = (string)info.GetValue("content", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("when", when);
            info.AddValue("creator", creator);
            info.AddValue("content", content);
            info.AddValue("title", title);
            info.AddValue("messages", messages);
        }

        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public DateTime When { get => when; set => when = value; }
        public string Creator { get => creator; set => creator = value; }
        public List<TopicMessage> Messages { get => messages; set => messages = value; }
        public List<User> AllUsers { get => allUsers; set => allUsers = value; }
    }
}

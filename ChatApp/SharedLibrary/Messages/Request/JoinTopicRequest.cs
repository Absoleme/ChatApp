using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class JoinTopicRequest : Message
    {
        private string username;
        private string title;

        public JoinTopicRequest(string t, string u)
        {
            this.title = t;
            this.username = u;
        }

        public string Title { get => title; set => title = value; }
        public string Username { get => username; set => username = value; }
    }
}

using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class SendMessageTopicRequest : Message
    {
        private DateTime when;
        private string creator;
        private string topicName;
        private string text;

        public SendMessageTopicRequest(DateTime when, string c, string tn, string t)
        {
            this.when = when;
            this.creator = c;
            this.topicName = tn;
            this.text = t;
        }

        public DateTime When { get => when; set => when = value; }
        public string Creator { get => creator; set => creator = value; }
        public string TopicName { get => topicName; set => topicName = value; }
        public string Text { get => text; set => text = value; }
    }
}

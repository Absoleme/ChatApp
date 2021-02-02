using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class SenMessageTopicResponse : Message
    {
        private Topic myTopic;

        public SenMessageTopicResponse(Topic t)
        {
            this.myTopic = t;
        }

        public Topic MyTopic { get => myTopic; set => myTopic = value; }
    }
}

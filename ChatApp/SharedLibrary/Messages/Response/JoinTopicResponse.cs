using System;
using System.Collections.Generic;

namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class JoinTopicResponse : Message
    {
        private Topic myTop;
        private bool isInTopic;

        public JoinTopicResponse(Topic myTopic, bool i)
        {
            this.myTop = myTopic;
            this.isInTopic = i;
        }

        public Topic MyTop { get => myTop; set => myTop = value; }
        public bool IsInTopic { get => isInTopic; set => isInTopic = value; }
    }
}

using System;
using System.Collections.Generic;

namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class ListTopicsResponse : Message
    {
        List<Topic> allTopic = new List<Topic>();
        public ListTopicsResponse(List<Topic> myList)
        {
            this.allTopic = myList;
        }

        public List<Topic> AllTopic { get => allTopic; set => allTopic = value; }
    }
}

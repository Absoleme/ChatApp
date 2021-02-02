using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class CreateTopicsRequest : Message
    {
        private string creator;
        private string title;
        private string content;
        private DateTime when;

        public CreateTopicsRequest(string c, string co, string t, DateTime date)
        {
            this.creator = c;
            this.title = t;
            this.content = co;
            this.when = date;
        }

        public DateTime When { get => when; set => when = value; }
        public string Content { get => content; set => content = value; }
        public string Title { get => title; set => title = value; }
        public string Creator { get => creator; set => creator = value; }
    }
}

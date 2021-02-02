using System;

namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class CreateTopicsResponse : Message
    {
        private string message;

        public CreateTopicsResponse(string m)
        {
            this.message = m;
        }

        public string Message { get => message; set => message = value; }
    }
}

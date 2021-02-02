using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class RegisterResponse : Message
    {
        private string message;

        public RegisterResponse(string m)
        {
            this.message = m;
        }

        public string Message { get => message; set => message = value; }
    }
}

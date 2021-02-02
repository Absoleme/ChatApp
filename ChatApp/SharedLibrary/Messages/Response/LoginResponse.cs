using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class LoginResponse : Message
    {
        private string message;
        private bool isConnected;
        private string username;
        public LoginResponse(bool isC)
        {
            this.isConnected = isC;
        }

        public string Message { get => message; set => message = value; }
        public bool IsConnected { get => isConnected; set => isConnected = value; }
        public string Username { get => username; set => username = value; }


      
    }
}

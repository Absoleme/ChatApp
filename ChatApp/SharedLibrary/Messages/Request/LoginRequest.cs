using System;
namespace SharedLibrary
{
    [Serializable]
    public class LoginRequest : Message
    {
        private Guid id;
        private string userName;
        private string password;

        public Guid Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }

        public LoginRequest( string u, string p)
        {
            userName = u;
            password = p;
        }
    }
}

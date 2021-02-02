using System;
namespace SharedLibrary
{
    [Serializable]
    public class RegisterRequest : Message
    {
        private Guid id;
        private string userName;
        private string password;

        public RegisterRequest(string u, string p)
        {
            this.userName = u;
            this.password = p;
        }

        public Guid Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
    }
}

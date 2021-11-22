
namespace SpirionUITests.Models
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

    }

    public class RestrictedUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RemoteTarget
    {
        public string MachineIP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RemoteBasePath { get; set; }
    }

    public class LocalAgent
    {
        public string MachineIP { get; set; }
        public string LocalAgentBasePath { get; set; }
        public string MachineName { get; set; }
    }

    public class DropBox
    {
        public string AdminAccount { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public string DropBoxToken { get; set; }
    }

    public class Cloud
    {
        public DropBox DropBox { get; set; }
    }


}

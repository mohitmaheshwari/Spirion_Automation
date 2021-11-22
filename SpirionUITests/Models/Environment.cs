namespace SpirionUITests.Models
{
    public class Environment
    {
        public string Browser { get; set; }
        public string TestUrl { get; set; }
        public string LogFile { get; set; }
        public User User { get; set; }
        public RestrictedUser RestrictedUser { get; set; }
        public RemoteTarget RemoteTarget { get; set; }
        public LocalAgent LocalAgent { get; set; }
        public string BasePath { get; set; }
        public bool DeleteSearchAfterTestRun { get; set; }
        public Cloud Cloud { get; set; }
    }
}
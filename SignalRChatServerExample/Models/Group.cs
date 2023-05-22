namespace SignalRChatServerExample.Models
{
    public class Group
    {
        public string groupName { get; set; }

        public List<Client> Clients { get; } = new List<Client>();
    }
}

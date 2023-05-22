using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.Data;
using SignalRChatServerExample.Models;

namespace SignalRChatServerExample.Hubs
{
    public class ChatHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("getCurrentConnId", Context.ConnectionId);
        }

        public async Task GetNickName(string nickName)
        {
            Client client = new Client()
            {

                ConnectionId = Context.ConnectionId,
                NickName = nickName,
            };

            ClientSource.Clients.Add(client);

            await Clients.Others.SendAsync("clientJoined", client);
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }
        public async Task SendMessageAsync(string nickName,string message)
        {
            Client senderClient = ClientSource.Clients.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if(nickName.Trim().Equals("Tümü"))
            {
                await Clients.All.SendAsync("receiveMessage", message, senderClient);
            }
            else
            {
                await Clients.Client(senderClient.ConnectionId).SendAsync("receiveMessage", message, senderClient);
            }
                      
        }
        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Group group = new Group() { groupName = groupName };
            group.Clients.Add(ClientSource.Clients.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault());
            GroupSource.Groups.Add(group);

            await Clients.All.SendAsync("groups", GroupSource.Groups);
        }
        public async Task AddClientToGroup(IEnumerable<string> groupNames)
        {
            Client client = ClientSource.Clients.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
            foreach (var groupName in groupNames)
            {
                Group group = GroupSource.Groups.Where(c => c.groupName == groupName).FirstOrDefault();

                bool result = group.Clients.Any(c => c.ConnectionId == Context.ConnectionId);
                
                if(!result)
                {
                    group.Clients.Add(client);
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.groupName);
                }
            }
        }
        public async Task GetClientsAsGroup(string groupName)
        {

            Group group = GroupSource.Groups.Where(c => c.groupName == groupName).FirstOrDefault();

            await Clients.Caller.SendAsync("clients", groupName == "all" ? ClientSource.Clients : group.Clients);

        }
        public async Task SendMessageToGroup(string groupName,string message)
        {
            await Clients.Group(groupName).SendAsync("receiveMessage", message,ClientSource.Clients.
                FirstOrDefault(c => c.ConnectionId == Context.ConnectionId));
        }

    }
}

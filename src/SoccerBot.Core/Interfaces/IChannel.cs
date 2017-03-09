using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Interfaces
{
    public enum States
    {
        Connecting,
        Connected,
        Disconnected,
    }

    public interface IChannel
    {
        String Id { get; set; }

        DateTime? LastMessageReceived { get; }

        event EventHandler<IChannel> Connected;
        event EventHandler<string> Disconnected;
        event EventHandler<byte[]> MessageReceived;
        event EventHandler<NetworkMessage> NetworkMessageReceived;

        Task WriteBuffer(byte[] buffer);

        States State { get; set; }

        Task<bool> ConnectAsync();
        Task DisconnectAsync();
    }
}

using System.Linq;
using DudeVoiceChat.Models;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;

namespace DudeVoiceChat.Utils
{
    public static class PlayerEx
    {
        public static CSteamID SteamId(this Player player) => player.channel.owner.playerID.steamID;
        public static ITransportConnection TC(this Player player) => player.channel.GetOwnerTransportConnection();
        public static VoicePlayer VoicePlayer(this Player player) => Core.Singleton.Players.Values.FirstOrDefault(vp => vp.Player.SteamId() == player.SteamId());
        public static VoicePlayer VoicePlayer(this SteamPlayer player) => Core.Singleton.Players.Values.FirstOrDefault(vp => vp.Player.SteamId() == player.playerID.steamID);
    }
}
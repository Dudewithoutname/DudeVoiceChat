using System.Collections.Generic;
using DudeVoiceChat.Utils;
using Rocket.API;
using Rocket.Unturned.Player;

namespace DudeVoiceChat.Commands
{
    public class CChangeVoice : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "changevoice";
        public string Help => "allows you to change voice type";
        public string Syntax => "/changevoice (type_name)";
        public List<string> Aliases => new () {"cv", "voice"};
        public List<string> Permissions => new () {"dudevoice.change"};

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is not UnturnedPlayer u) return;
            if (command.Length < 1) u.Player.VoicePlayer().Change();
            else u.Player.VoicePlayer().Change(command[0]);
        }
    }
}
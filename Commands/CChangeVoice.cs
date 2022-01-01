using System.Collections.Generic;
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
        public List<string> Aliases => new List<string> {"cv", "voice"};
        public List<string> Permissions => new List<string> {"dudevoice.change"};

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = ((UnturnedPlayer)caller).Player;
            
            if (player == null) return;
            
            if (command.Length < 1)
            {
                Core.Singleton.ChangeVoiceType(player);
            }
            else
            {
                Core.Singleton.ChangeVoiceType(player, command[0].ToLower());
            }
        }
    }
}
using System;
using System.Linq;
using DudeVoiceChat.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace DudeVoiceChat.Models
{
    public class VoicePlayer
    {
        private const short KEY = 7296;
        
        public readonly Player Player;
        public Voice Voice;
        public uint LatestSim;
            
        public VoicePlayer(Player _player) 
        {
            Player = _player;
            Voice = Core.Singleton.Configuration.Instance.Voices.FirstOrDefault(vc => vc.Name == Core.Singleton.Configuration.Instance.DefaultVoice);
            EffectManager.sendUIEffect(Core.Singleton.Configuration.Instance.EffectId, KEY, Player.TC(), true);
            updateHud();
        }

        public void Change(string voiceName) => 
            Change(Core.Singleton.Configuration.Instance.Voices.FirstOrDefault(vt => string.Equals(vt.Name, voiceName, StringComparison.CurrentCultureIgnoreCase)));
        
        public void Change() => Change(Voice.Order >= Core.Singleton.MaxOrder
            ? Core.Singleton.Configuration.Instance.Voices.FirstOrDefault(v => v.Order == 1)
            : Core.Singleton.Configuration.Instance.Voices.FirstOrDefault(v => v.Order == Voice.Order + 1));
        
        public void Change(Voice voice)
        {
            if (voice is null)
            {
                ChatManager.say(Player.SteamId(), Core.Trans("not_found"), Palette.COLOR_R, true);
                return;
            }
            if (voice.Permission is not (null or "") && !UnturnedPlayer.FromPlayer(Player).HasPermission(voice.Permission))
            {
                ChatManager.say(Player.SteamId(), Core.Trans("no_permission", voice.Name), Palette.COLOR_R, true);
                return;
            }
            Voice = voice;
            updateHud();
        }
        
        private void updateHud()
        {
            EffectManager.sendUIEffectImageURL(KEY, Player.TC(), true, "dude_voice", Voice.Icon);
            if (Core.Singleton.Configuration.Instance.ShouldSayMessage) ChatManager.say(Player.SteamId(), Core.Trans("changed", Voice.Name), Color.white, true);
        }
    }
}
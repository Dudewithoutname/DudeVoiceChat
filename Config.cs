using System.Collections.Generic;
using DudeVoiceChat.Models;
using Rocket.API;

namespace DudeVoiceChat
{
    public class Config : IRocketPluginConfiguration
    {
        public bool ShouldSayMessage;
        public bool EnableKeyChange;
        public int KeyCheckTick;
        public byte KeyId;
        public string DefaultVoice;
        public ushort BackgroundEffectId;
        public List<Voice> VoiceTypes;
        
        public void LoadDefaults()
        {
            ShouldSayMessage = true;
            EnableKeyChange = true;
            KeyCheckTick = 100;
            KeyId = 9;
            DefaultVoice = "Talk";
            BackgroundEffectId = 31930;
            VoiceTypes = new List<Voice>
            {
                new Voice
                {
                    Order = 1,
                    Name = "Whisper",
                    EffectId = 31931,
                    Range = 30
                },
                new Voice
                {
                    Order = 2,
                    Name = "Talk",
                    EffectId = 31932,
                    Range = 50
                },
                new Voice
                {
                    Order = 3,
                    Name = "Shout",
                    EffectId = 31933,
                    Range = 100
                }
            };
        }
    }
}
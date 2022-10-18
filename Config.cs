using System.Collections.Generic;
using DudeVoiceChat.Models;
using Rocket.API;

namespace DudeVoiceChat
{
    public class Config : IRocketPluginConfiguration
    {
        public bool ShouldSayMessage;
        public bool EnableChangeByKey;
        public byte Key;
        public string DefaultVoice;
        public ushort EffectId;
        public List<Voice> Voices;
        
        public void LoadDefaults()
        {
            ShouldSayMessage = true;
            EnableChangeByKey = true;
            Key = 0;
            DefaultVoice = "Talk";
            EffectId = 42767;
            Voices = new List<Voice>
            {
                new ()
                {
                    Order = 1,
                    Name = "Whisper",
                    Range = 15,
                    Icon = "https://i.ibb.co/KNp3Fqs/vc-3.png"
                },
                new ()
                {
                    Order = 2,
                    Name = "Talk",
                    Range = 35,
                    Icon = "https://i.ibb.co/3CgR3jN/vc-2.png"
                },
                new ()
                {
                    Order = 3,
                    Name = "Shout",
                    Range = 70,
                    Icon = "https://i.ibb.co/KVtxtcN/vc-1.png"
                },
                new ()
                {
                    Order = 0,
                    Name = "Global",
                    Icon = "https://i.ibb.co/JQMv5J7/vc-5.png",
                    Permission = "dudevoice.global",
                    IsGlobal = true
                },
                new ()
                {
                    Order = 0,
                    Name = "Police",
                    Icon = "https://i.ibb.co/7VB0b8c/vc-4.png",
                    Permission = "dudevoice.police",
                    IsChannelBased = true
                }
            };
        }
    }
}
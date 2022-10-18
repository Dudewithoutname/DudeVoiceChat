using System;

namespace DudeVoiceChat.Models
{
    [Serializable]
    public record Voice
    {
        public byte Order;
        public string Name;
        public string Icon;
        public string Permission;
        public bool IsChannelBased;
        public bool IsGlobal;
        public uint Range;
    }
}
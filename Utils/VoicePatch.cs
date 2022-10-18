using HarmonyLib;
using SDG.Unturned;

namespace DudeVoiceChat
{
    [HarmonyPatch(typeof(PlayerVoice), "handleRelayVoiceCulling_Proximity")]
    internal class VoicePatch
    {
        internal delegate bool Handle(PlayerVoice speaker, PlayerVoice listener);
        internal static Handle onHandle;

        [HarmonyPrefix]
        private static bool handler(PlayerVoice speaker, PlayerVoice listener) => onHandle.Invoke(speaker, listener);
    }
}
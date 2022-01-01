using HarmonyLib;
using SDG.Unturned;

namespace DudeVoiceChat
{
    [HarmonyPatch(typeof(PlayerVoice), "handleRelayVoiceCulling_Proximity")]
    internal class VoicePatch
    {
        internal static Handle onHandle;

        [HarmonyPrefix]
        private static bool handler(PlayerVoice speaker, PlayerVoice listener) => onHandle.Invoke(speaker, listener);

        internal delegate bool Handle(PlayerVoice speaker, PlayerVoice listener);
    }
}
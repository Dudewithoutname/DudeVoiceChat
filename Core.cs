using System.Collections.Generic;
using System.Linq;
using DudeVoiceChat.Models;
using HarmonyLib;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace DudeVoiceChat
{
    public class Core : RocketPlugin<Config>
    {
        public static Core Singleton;
        
        private static Dictionary<CSteamID, Voice> voicePlayers;
        private static Dictionary<CSteamID, KeyWatcher> watchers;
        private string logPrefix = "DudeVoiceChat";
        private Voice defaultvoiceType;
        private Harmony harmony;
        
        protected override void Load()
        {
            Singleton = this;
            voicePlayers = new Dictionary<CSteamID, Voice>();
            defaultvoiceType = Configuration.Instance.VoiceTypes.FirstOrDefault(voice => voice.Name == Configuration.Instance.DefaultVoice);
            if (Configuration.Instance.KeyId < 9 || Configuration.Instance.KeyId > 12)
            {
                Logger.LogWarning($"Auto-Fix: KeyId ({Configuration.Instance.KeyId}) cannot be higher than 12 or lower than 9");
                Logger.LogWarning("Auto-Fix: Setting KeyId to 9 (PluginHotkey1) by default");
                Configuration.Instance.KeyId = 9;
            }
            if (Configuration.Instance.EnableKeyChange) watchers = new Dictionary<CSteamID, KeyWatcher>();

            harmony = new Harmony("dudevoicechat");
            harmony.PatchAll();
            
            Player.onPlayerCreated += onPlayerCreated;
            Provider.onEnemyDisconnected += onPlayerDisconnected;
            VoicePatch.onHandle += onHandleVoice;
        }

        protected override void Unload()
        {
            harmony.UnpatchAll();
            Singleton = null;
            harmony = null;
            voicePlayers = null;
            defaultvoiceType = null;
            
            Player.onPlayerCreated -= onPlayerCreated;
            Provider.onEnemyDisconnected -= onPlayerDisconnected;
            VoicePatch.onHandle -= onHandleVoice;
        }

        public void ChangeVoiceType(Player player, string voiceName)
        {
            var newVoice = Configuration.Instance.VoiceTypes.FirstOrDefault(vt => vt.Name.ToLower() == voiceName);

            if (newVoice == null)
            {
                ChatManager.say(player.channel.owner.playerID.steamID, Translations.Instance.Translate("error_not_found"), Palette.COLOR_R, true);
                return;
            }
            
            if (!UnturnedPlayer.FromPlayer(player).HasPermission(newVoice.Permission))
            {
                ChatManager.say(player.channel.owner.playerID.steamID, Translations.Instance.Translate("no_permission", newVoice.Name), Palette.COLOR_R, true);
                return;
            }

            voicePlayers[player.channel.owner.playerID.steamID] = newVoice;
            updateHUD(player, newVoice);
            if (Configuration.Instance.ShouldSayMessage) ChatManager.say(player.channel.owner.playerID.steamID, Translations.Instance.Translate("changed", newVoice.Name), Color.white, true);
        }
        
        public void ChangeVoiceType(Player player)
        {
            var steamId = player.channel.owner.playerID.steamID;
            var voicetype = voicePlayers[steamId].Order >= Configuration.Instance.VoiceTypes.Count
                ? Configuration.Instance.VoiceTypes.FirstOrDefault(vt => vt.Order == 1)
                : Configuration.Instance.VoiceTypes.FirstOrDefault(vt => vt.Order == voicePlayers[steamId].Order + 1);

            if (!UnturnedPlayer.FromPlayer(player).HasPermission(voicetype.Permission))
            {
                ChatManager.say(steamId, Translations.Instance.Translate("no_permission", voicetype.Name), Palette.COLOR_R, true);
                return;
            }
            
            voicePlayers[steamId] = voicetype;
            updateHUD(player, voicePlayers[steamId]);
            if (Configuration.Instance.ShouldSayMessage) ChatManager.say(player.channel.owner.playerID.steamID, Translations.Instance.Translate("changed", voicePlayers[steamId].Name), Color.white, true);
        }

        private void updateHUD(Player player, Voice voiceType) => EffectManager.sendUIEffect(voiceType.EffectId, 13541, player.channel.GetOwnerTransportConnection(), true);
        

        #region Event Handling

        private bool onHandleVoice(PlayerVoice speaker, PlayerVoice listener)
        {
            if (!UnturnedPlayer.FromPlayer(speaker.player).HasPermission(voicePlayers[speaker.player.channel.owner.playerID.steamID].Permission))
            {
                ChatManager.say(speaker.player.channel.owner.playerID.steamID, Translations.Instance.Translate("no_permission", voicePlayers[speaker.player.channel.owner.playerID.steamID].Name), Palette.COLOR_R, true);
                return false;
            }

            return Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= voicePlayers[speaker.channel.owner.playerID.steamID].Range;
        }
        
        private void onPlayerCreated(Player player) 
        {
            var steamId = player.channel.owner.playerID.steamID;

            if (!voicePlayers.ContainsKey(steamId))
            {
                voicePlayers.Add(steamId, defaultvoiceType);
                updateHUD(player, defaultvoiceType);
                if (Configuration.Instance.BackgroundEffectId != 0) EffectManager.sendUIEffect(31930, 13540, player.channel.GetOwnerTransportConnection(), true);

                if (Configuration.Instance.EnableKeyChange) watchers.Add(steamId, new KeyWatcher(player));
            }
            else
            {
                CommandWindow.LogError($"[{logPrefix}] | ERROR({nameof(onPlayerCreated)}) >>  Player {steamId} is already in collection");
            }
        }

        private void onPlayerDisconnected(SteamPlayer player)
        {
            var steamId = player.playerID.steamID;

            if (voicePlayers.ContainsKey(steamId))
            {
                if (Configuration.Instance.EnableKeyChange && watchers.ContainsKey(player.playerID.steamID)) watchers[player.playerID.steamID].Stop();
                voicePlayers.Remove(steamId);
            }
            else
            {
                CommandWindow.LogError($"[{logPrefix}] | ERROR({nameof(onPlayerDisconnected)}) >> Couldn't find player with SteamID {steamId}");
            }
        }
        #endregion
        
        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                {"changed","<color=green>Voice</color> >> Your voice was successfully changed to <color=green><b>{0}</b></color>!"},
                {"no_permission","You don't have permission to use {0} voice mode!"},
                {"error_not_found","Voice type with that name doesn't exist!"},
            };
    }
}
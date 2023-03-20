using Steamworks;
using HarmonyLib;
using UnityEngine;
using SDG.Unturned;
using DudeVoiceChat.Utils;
using DudeVoiceChat.Models;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using System.Collections.Generic;
using System.Linq;
using Logger = Rocket.Core.Logging.Logger;

namespace DudeVoiceChat
{
    public class Core : RocketPlugin<Config>
    {
        public static Core Singleton;
        public int MaxOrder;
        public Dictionary<CSteamID, VoicePlayer> Players;
        private Harmony harmony;
        
        public override TranslationList DefaultTranslations => new ()
        {
            {"changed", "[color=#a93dfc]Voice[/color] > Your voice has been set to [color=#a93dfc][b]{0}[/b][/color]!"}, 
            {"no_permission", "You don't have permission to use {0} voice mode!"}, 
            {"not_found", "Voice mode with that name does not exist!"},
        };
        public static string Trans(string key, params object[] placeholder) => Singleton.Translate(key, placeholder).Replace("[", "<").Replace("]", ">");

        protected override void Load()
        {
            Singleton = this;
            Players = new Dictionary<CSteamID, VoicePlayer>();
            MaxOrder = Configuration.Instance.Voices.Count(v => v.Order != 0);

            harmony = new Harmony("dudevoicechat");
            harmony.PatchAll();
            
            Player.onPlayerCreated += onPlayerCreated;
            Provider.onEnemyDisconnected += onPlayerDisconnected;
            VoicePatch.onHandle += onHandleVoice;
            if (Configuration.Instance.EnableChangeByKey) PlayerInput.onPluginKeyTick += onKeyTick;
            
            Logger.Log("Made by Dudewithoutname#3129 with <3");
            Logger.Log($"Loaded v{Assembly.GetName().Version}!");
        }

        protected override void Unload()
        {
            harmony.UnpatchAll();
            Singleton = null;
            
            Player.onPlayerCreated -= onPlayerCreated;
            Provider.onEnemyDisconnected -= onPlayerDisconnected;
            VoicePatch.onHandle -= onHandleVoice;
            if (Configuration.Instance.EnableChangeByKey) PlayerInput.onPluginKeyTick -= onKeyTick;
            
            Logger.Log("See you later <3!");
        }
        
        private void onPlayerCreated(Player player) 
        {
            if (Players.ContainsKey(player.SteamId())) Players.Remove(player.SteamId());
            Players.Add(player.SteamId(), new VoicePlayer(player));
        }

        private void onPlayerDisconnected(SteamPlayer player) => Players.Remove(player.playerID.steamID);

        private bool onHandleVoice(PlayerVoice speaker, PlayerVoice listener) => speaker.player.VoicePlayer().Voice.IsGlobal || (speaker.player.VoicePlayer().Voice.IsChannelBased 
            ? speaker.player.VoicePlayer().Voice.Name == listener.player.VoicePlayer().Voice.Name 
            : Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= speaker.player.VoicePlayer().Voice.Range);

        private void onKeyTick(Player player, uint simulation, byte key, bool state)
        {
            if (Configuration.Instance.Key != key || !state) return;
            var p = player.VoicePlayer();
            if (simulation - p.LatestSim < 10) return;
            p.LatestSim = simulation;
            p.Change();
        }
    }
}

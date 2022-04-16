using System.Threading;
using Rocket.Core.Utils;
using SDG.Unturned;

namespace DudeVoiceChat
{
    // credits to Shimmy <3 for the base
    public class KeyWatcher
    {
        public Player Player;
        private bool isRunning;
        private bool wasPressed;
            
        public KeyWatcher(Player player)
        {
            Player = player;
            isRunning = true;
            new Thread(UpdateLoop).Start();
        }

        public void Stop()
        {
            isRunning = false;
        }

        private void UpdateLoop()
        {
            while (isRunning)
            {
                if (Player.input.keys[Core.Singleton.Configuration.Instance.KeyId])
                {
                    if (!wasPressed)
                    {
                        TaskDispatcher.QueueOnMainThread(() => Core.Singleton.ChangeVoiceType(Player));
                        wasPressed = true;   
                    }
                }
                else
                {
                    wasPressed = false;
                }
                
                Thread.Sleep(Core.Singleton.Configuration.Instance.KeyCheckTick);
            }
        }
    }
}

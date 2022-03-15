# DudeVoiceChat
- This plugin can edit player's voice distance
- 0Harmony is required
- [Workshop UI link](https://steamcommunity.com/sharedfiles/filedetails/?id=2391628792)

## Commands
- `/changevoice {voice type name}` = allows you to change your voice mode (*if argument isn't specified it goes by the order*)
- aliases : `/cv, /voice`
- permission: `dudevoice.change`
## Configuration
- **ShouldSayMessage** = should send messange when player changed voice mode?
- **EnableKeyChange** = may player change his voice mode via keybind? 
- **KeyCheckTick** = time between next key press check (*in milliseconds*, *lower number = higher cpu usage*)
- **KeyId** = which key should it react to?
- **DefaultVoice** = which voice mode should be set when player connected?
- **BackgroundEffectId** = id for UI background (*it isn't sent to client if it's 0*)

## VoiceTypes
- **Order** = order of voice modes when pressing keybind or command without argument (*if it's 0 it can be only accessed via /changevoice (name)*)
- **Name** = name ... i guess it's not neccessary to explain this parameter
- **EffectId** = Id for the UI
- **Permission** = permission for the voice type (*if "no_permission" is specified it shouldn't require any permission*)

## KeyId
- PluginHotkey1 = 9
- PluginHotkey2 = 10
- PluginHotkey3 = 11
- PluginHotkey4 = 12

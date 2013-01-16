VoiceroidAccessor
=================

.NET Library for voiceroid (message speaking application. AHS http://www.ah-soft.com/)

Using
=================

Create plugin
-----------------

1. Create class
2. extends net.azworks.VoiceroidAccess.voiceroids.Voiceroid
3. override virtual methods

Use plugins
-----------------

1. create "voiceroids" folder in application folder
2. put build plugins into "voiceroids"
3. create instance "VoiceroidManager voiceroidManager = new VoiceroidManager();"
4. send message "voiceroidManager.Talk("VOICEROID＋ 結月ゆかり", "message string");"

3rd argument of voiceroidManager.Talk are option for waiting delay.
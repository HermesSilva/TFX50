Set oShell = CreateObject ("Wscript.Shell") 
Dim strArgs
strArgs = "cmd /c TFX50.bat"
oShell.Run strArgs, 0, false

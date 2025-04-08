Set oShell = CreateObject ("Wscript.Shell") 
Dim strArgs
strArgs = "cmd /c TFX.bat"
oShell.Run strArgs, 0, false

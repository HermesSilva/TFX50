
if exist D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\Exec rmdir /q/s D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

mkdir D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

xcopy /s D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\net8.0\*.* D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

cd D:\Tootega\Source\Access-POC\App\Launchers\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

Launcher

goto fim

:ERRO
pause
:fim


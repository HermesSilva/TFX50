
if exist D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\Exec rmdir /q/s D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

mkdir D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

xcopy /s D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\net9.0\*.* D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

cd D:\Tootega\Source\TFX50\Application\Launcher\bin\Debug\Exec
IF ERRORLEVEL 1 GOTO ERRO

set SQL_SERVER_TFX=Server=localhost;Initial Catalog=TFX;Persist Security Info=False;Integrated Security=true;MultipleActiveResultSets=true;Encrypt=false;TrustServerCertificate=true;Connection Timeout=300;

Launcher

goto fim

:ERRO
pause
:fim


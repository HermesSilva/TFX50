git pull
IF ERRORLEVEL 1 GOTO ERRO

git add .
IF ERRORLEVEL 1 GOTO ERRO

SET /P msg=Message : 


if "%msg%" == "" git commit -m "Commit"
IF ERRORLEVEL 1 GOTO ERRO

if NOT "%msg%" == "" git commit -m "%msg%"
IF ERRORLEVEL 1 GOTO ERRO

git push
IF ERRORLEVEL 1 GOTO ERRO

goto fim

:ERRO
pause
:fim
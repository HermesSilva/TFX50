set RelativePath=%~dp0%
set BinPath=D:\Tootega\DASE4VSBin
set home=C:\Users\Hermes\AppData\Local\Microsoft\VisualStudio\17.0_9c90c9bdTFX\Extensions\jpbouh1k.l4h
if exist %work% set VSIX=%work%
if exist %home% set VSIX=%home%
if exist %work2% set VSIX=%work2%

if [%BinPath%] == [] goto ERRO
if [%VSIX%] == [] goto NOCOPY
rem if [%1] == [] goto NOCOPY

set DeployType=None

cd %BinPath%\


copy TFX.DASE*.* %VSIX%\
IF ERRORLEVEL 1 GOTO ERRO 

copy DASE.VSIX.Core.* %VSIX%\
IF ERRORLEVEL 1 GOTO ERRO

copy *.dll %VSIX%\
IF ERRORLEVEL 1 GOTO ERRO
copy *.pdb %VSIX%\
IF ERRORLEVEL 1 GOTO ERRO
copy DASE4VS.pkgdef %VSIX%\
IF ERRORLEVEL 1 GOTO ERRO

REM copy *.pak %VSIX%\
REM IF ERRORLEVEL 1 GOTO ERRO
set SQL_SERVER_TFX=Server=localhost;Initial Catalog=TFX;Persist Security Info=False;Integrated Security=true;MultipleActiveResultSets=true;Encrypt=false;TrustServerCertificate=true;Connection Timeout=300;
:NOCOPY
cd %RelativePath%
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" /rootSuffix  TFX TFX50.sln

goto fim

:ERRO
pause
:fim


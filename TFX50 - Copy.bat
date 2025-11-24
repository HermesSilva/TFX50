set RelativePath=%~dp0%
set BinPath=D:\Tootega\DASE4VSBin
set home=C:\Users\Hermes\AppData\Local\Microsoft\VisualStudio\18.0_a8edbb8fTFX\Extensions\5lf24y1j.kan
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
rem set SQL_SERVER_TFX=Data Source=SRV2-SAP\ERP;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=True;Trust Server Certificate=True;Application Name="SQL Server Management Studio";Command Timeout=0
:NOCOPY
cd %RelativePath%
if exist "C:\Program Files\Microsoft Visual Studio\18\Insiders\Common7\IDE\devenv.exe" "C:\Program Files\Microsoft Visual Studio\18\Insiders\Common7\IDE\devenv.exe" /rootSuffix  TFX TFX50.sln

goto fim

:ERRO
pause
:fim


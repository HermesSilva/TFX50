@echo on
setlocal enabledelayedexpansion

:: Captura o diretório completo onde o script está sendo executado
set "SCRIPT_DIR=%~dp0Application\Launcher"


echo XXXXXXXXXXXXXXXX
echo [%SCRIPT_DIR%]
:: Define o arquivo JSON como appsettings.json
set "JSON_FILE=Application\Launcher\appsettings.json"

:: Solicita ao usuário se é produção ou desenvolvimento
set /p "ENVIRONMENT=Digite o ambiente (1=producao/2=desenvolvimento): "

:: Verifica a entrada do usuário e define o repositório e tag apropriados
if /I "%ENVIRONMENT%"=="1" (
    set "JSON_FILE=Application\Launcher\appsettings.Production.json"
) else if /I "%ENVIRONMENT%"=="2" (
    set "JSON_FILE=Application\Launcher\appsettings.Development.json"
) else (
    echo Ambiente invalido. Use "1-Producao" ou "2-Desenvolvimento".
    exit /b 1
)

:: Verifica se o arquivo JSON existe
if not exist "!JSON_FILE!" (
    echo Erro: Arquivo JSON "!JSON_FILE!" não encontrado.
    exit /b 1
)

echo JSON_FILE: !JSON_FILE!

:: Lê o valor da chave "AppVersion" do arquivo JSON (com tratamento de erros e aspas)
for /f "tokens=2 delims=:, " %%a in ('type "!JSON_FILE!" ^| findstr /C:"\"AppVersion\""') do (
    set "VERSION=%%~a"
    goto :read_image
)
echo Erro: Chave "AppVersion" não encontrada em "!JSON_FILE!"
exit /b 1

:read_image
:: Lê o valor da chave "Image" dentro do objeto "Docker" do arquivo JSON (com tratamento de erros e aspas)
for /f "tokens=2 delims=:, " %%a in ('type "!JSON_FILE!" ^| findstr /C:"\"Image\""') do (
    set "IMAGE=%%~a"
    goto :read_registry
)
echo Erro: Chave "Image" não encontrada em "!JSON_FILE!"
exit /b 1

:read_registry
:: Lê o valor da chave "Url" do arquivo JSON (com tratamento de erros e aspas)
for /f "tokens=2 delims=:, " %%a in ('type "!JSON_FILE!" ^| findstr /C:"\"Url\""') do (
    set "REPOSITORY=%%~a"
    goto :done
)
echo Erro: Chave "Url" não encontrada em "!JSON_FILE!"
exit /b 1

:done

:: Remove aspas duplas ao redor dos valores (com verificação se a variável está definida)
if defined VERSION set "VERSION=!VERSION:~1,-1!"
if defined IMAGE set "IMAGE=!IMAGE:~1,-1!"
if defined REPOSITORY set "REPOSITORY=!REPOSITORY:~1,-1!"

echo SCRIPT_FOLDER: !SCRIPT_FOLDER!
echo VERSION: !VERSION!
echo IMAGE: !IMAGE!
echo REGISTRY: !REPOSITORY!

:: Construção da imagem Docker
echo Construindo a imagem Docker...
rem cd ..
docker build -t "!IMAGE!:!VERSION!" -f Dockerfile .

:: Criar uma tag para imagem
rem docker tag "!IMAGE!:!VERSION!" "!REPOSITORY!!IMAGE!:!VERSION!"

:: Publicar a imagem
rem docker push "!REPOSITORY!!IMAGE!:!VERSION!"

rem echo Imagem publicada com sucesso!

endlocal

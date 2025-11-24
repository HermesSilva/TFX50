# Guia de Configuração para Visual Studio 2022

Este projeto foi configurado para oferecer uma experiência otimizada no Visual Studio 2022. Siga os passos abaixo para preparar seu ambiente.

## 1. Executar Script de Configuração
Para garantir que todas as dependências (SDKs, pacotes Node, etc.) estejam corretas, execute o script de configuração automatizado.

1. Abra a pasta do projeto no Windows Explorer.
2. Clique com o botão direito em `Setup-Environment.ps1`.
3. Selecione **"Executar com o PowerShell"**.

O script irá:
- Verificar se o .NET 9 SDK está instalado.
- Verificar Node.js e Yarn.
- Instalar as dependências do frontend (`TFX.Core.UI`).
- Oferecer a instalação da extensão **DASE4VS** necessária.

## 2. Requisitos do Visual Studio
Certifique-se de que você instalou a carga de trabalho **"Desenvolvimento Web e ASP.NET"** no Visual Studio Installer. Isso é necessário para carregar o projeto frontend (`.esproj`) corretamente.

## 3. Abrir a Solução
Após a configuração, abra o arquivo `TFX50.sln` no Visual Studio 2022.

## 3. Executar o Projeto
O projeto de inicialização já está configurado como **Launcher**.

1. Certifique-se de que **Launcher** está selecionado como projeto de inicialização na barra de ferramentas.
2. Pressione **F5** ou clique em **Iniciar**.
3. O navegador abrirá automaticamente em `http://localhost:7000/swagger` (ou a rota padrão configurada).

## Notas Adicionais
- **Frontend**: O código do frontend está em `Core/TFX.Core.UI`. Se precisar recompilar manualmente, você pode usar o terminal integrado:
  ```bash
  cd Core/TFX.Core.UI
  yarn build
  ```
- **Extensão DASE**: Se você não instalou a extensão durante o script, pode encontrá-la em `releases/download/v1.0.0/DASE4VS.vsix`.

---
*Ambiente preparado para máxima produtividade.*

# TFX50

TFX50 é uma solução completa baseada em .NET 9 destinada ao desenvolvimento de serviços e aplicações web. O projeto inclui diversos módulos de backend e uma interface web escrita em TypeScript.

## Visão Geral do Repositório

```
Application/        Aplicativos de inicialização do servidor (Launcher e LauncherID)
Core/              Bibliotecas de domínio como TFX.Core, TFX.Core.Data e interface web (TFX.Core.UI)
Modules/           Módulos adicionais, como TFX.ESC.Core
Tests/             Conjunto de testes automatizados e projetos de stress
```

### Principais Componentes

- **Launcher**: cria o `WebApplication` ASP.NET e registra serviços, banco de dados e middlewares.
- **TFX.Core**: base de utilidades, controladores e serviços para as demais bibliotecas.
- **TFX.Core.Data**: modelos de dados e acesso a banco (Entity Framework Core).
- **TFX.Core.Access**: serviço de login e gerenciamento de sessões (cache e JWT).
- **TFX.Core.UI**: client-side em TypeScript/Sass localizado em `Core/TFX.Core.UI`. Os scripts são compilados em `wwwroot`.
- **TFX.ESC.Core**: módulo opcional que estende a solução com funcionalidade extra.

## Pré‑requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/) instalado.
- Node.js/yarn caso deseje compilar a interface web em `TFX.Core.UI`.
- DASE Instalado [Instruções de Instalação](https://github.com/HermesSilva/TFX50/tree/main/DASE4VS.md)

---
## Compilação

Para compilar toda a solução execute:

```bash
dotnet build TFX50.sln
```

Para compilar somente a interface web:

```bash
cd Core/TFX.Core.UI
yarn install   # ou npm install
yarn build     # gera JS/CSS em wwwroot
```

## Execução

O servidor pode ser iniciado pelo projeto `Launcher`:

```bash
dotnet run --project Application/Launcher/Launcher.csproj
```

A aplicação escuta por padrão em `http://localhost:7000` e fornece endpoints REST configurados nos controladores.

## Testes

São fornecidos testes unitários e de estresse. Para executá‑los utilize:

```bash
dotnet test
```

(Alguns testes dependem de serviços externos e podem precisar de configuração prévia.)

## Licença

Distribuído sob os termos da [MIT License](LICENSE).

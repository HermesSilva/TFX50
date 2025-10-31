# ===============================
# Dockerfile (raiz do repo TFX50)
# .NET 9, multi-stage
# App principal: Application/Launcher/Launcher.csproj
# wwwroot está em Core/TFX.Core.UI/wwwroot (sem compilação) e deve ir para a saída final
# ===============================

# ---------- STAGE 1: RESTORE/BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos o repositório inteiro para garantir que todos os ProjectReference existam
# (estrutura grande; mais robusto e simples)
#COPY . .
COPY ["Application/Launcher", "Application/Launcher/"]
COPY ["Core/TFX.Core.Access", "Core/TFX.Core.Access/"]
COPY ["Core/TFX.Core.Data", "Core/TFX.Core.Data/"]
COPY ["Core/TFX.Core", "Core/TFX.Core/"]
COPY ["Core/TFX.Core.UI", "Core/TFX.Core.UI/"]
COPY ["Modules/ESC/TFX.ESC.Core", "Modules/ESC/TFX.ESC.Core/"]
COPY ["Modules/Tootega.Core.CEP", "Modules/Tootega.Core.CEP/"]
COPY ["Modules/Tootega.Core.ERP", "Modules/Tootega.Core.ERP/"]
#
# Restaura e compila SOMENTE o projeto Launcher (não a solução inteira)
RUN dotnet restore ./Application/Launcher/Launcher.csproj
RUN dotnet build   ./Application/Launcher/Launcher.csproj -c $BUILD_CONFIGURATION -o /app/build --no-restore

# ---------- STAGE 2: PUBLISH (inclui wwwroot externo) ----------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Publica o projeto (sem host nativo; entrypoint será "dotnet <dll>")
RUN dotnet publish ./Application/Launcher/Launcher.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

# Garante que os assets estáticos do projeto não compilado vão para a saída final
# (copiamos Core/TFX.Core.UI/wwwroot para a pasta publish/wwwroot)
RUN mkdir -p /app/publish/wwwroot && \
    cp -a /src/Core/TFX.Core.UI/wwwroot/. /app/publish/wwwroot/

# ---------- STAGE 3: RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia artefatos publicados (inclui wwwroot)
COPY --from=publish /app/publish .

# Porta padrão em containers .NET 9 = 7000
ENV ASPNETCORE_URLS=http://+:7000
ENV SQL_SERVER_TFX="Data Source=192.168.1.7;User ID=sa;Password=senhas;Pooling=true;Initial Catalog=TFX;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Command Timeout=30"
EXPOSE 7000

# ATENÇÃO: Se o <AssemblyName> no .csproj for diferente de "Launcher", ajuste abaixo.
ENTRYPOINT ["dotnet", "Launcher.dll"]

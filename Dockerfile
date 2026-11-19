# ============================================================================
# Stage 1: Build & Publish (.NET 8 SDK win-x64 Self-Contained)
# ============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Optimización de caché de capas copiando primero la estructura de proyectos
COPY FederacionSantillan.sln ./
COPY src/FederacionSantillan.Model/FederacionSantillan.Model.csproj src/FederacionSantillan.Model/
COPY src/FederacionSantillan.Data/FederacionSantillan.Data.csproj src/FederacionSantillan.Data/
COPY src/FederacionSantillan.App/FederacionSantillan.App.csproj src/FederacionSantillan.App/
COPY tests/FederacionSantillan.Tests/FederacionSantillan.Tests.csproj tests/FederacionSantillan.Tests/

RUN dotnet restore FederacionSantillan.sln

# Copiar código fuente y compilar artefactos para win-x64
COPY . .
RUN dotnet publish src/FederacionSantillan.App/FederacionSantillan.App.csproj \
    -c Release \
    -r win-x64 \
    --self-contained true \
    -o /app/publish

# ============================================================================
# Stage 2: Runtime Visual noVNC + Wine (Web-Accessible en Mac / Linux / Windows)
# ============================================================================
FROM debian:bookworm-slim

ENV DEBIAN_FRONTEND=noninteractive
ENV DISPLAY=:0
ENV WINEPREFIX=/root/.wine
ENV WINEARCH=win64
ENV WINEDEBUG=-all
ENV SCREEN_WIDTH=1366
ENV SCREEN_HEIGHT=768
ENV SCREEN_DEPTH=24

# Dependencias visuales X11, servidor Wine, VNC y proxy WebSocket noVNC
RUN dpkg --add-architecture i386 && \
    apt-get update && \
    apt-get install -y --no-install-recommends \
        wine \
        wine64 \
        xvfb \
        fluxbox \
        x11vnc \
        novnc \
        websockify \
        fonts-dejavu-core \
        fonts-liberation \
        ca-certificates \
        procps \
        bash && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Autoconexión por defecto al abrir http://localhost:8080
RUN ln -sf /usr/share/novnc/vnc.html /usr/share/novnc/index.html

WORKDIR /app
COPY --from=build /app/publish /app/publish
COPY docker/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 8080 5900

ENTRYPOINT ["/bin/bash", "/app/entrypoint.sh"]

#!/usr/bin/env bash
set -e

echo "[Santillán Seguros] Iniciando entorno virtual visual X11..."
Xvfb :0 -screen 0 "${SCREEN_WIDTH:-1366}x${SCREEN_HEIGHT:-768}x${SCREEN_DEPTH:-24}" &
XVFB_PID=$!
sleep 1

echo "[Santillán Seguros] Iniciando gestor de ventanas Fluxbox..."
fluxbox &
FLUXBOX_PID=$!
sleep 1

echo "[Santillán Seguros] Iniciando servidor VNC (x11vnc)..."
x11vnc -display :0 -nopw -forever -shared -rfbport 5900 -quiet &
X11VNC_PID=$!
sleep 1

echo "[Santillán Seguros] Iniciando servidor noVNC WebSockets en puerto 8080..."
websockify --web /usr/share/novnc 8080 localhost:5900 &
WEBSOCKIFY_PID=$!
sleep 1

echo "[Santillán Seguros] Inicializando Wine prefix..."
wineboot --init 2>/dev/null || true

echo "[Santillán Seguros] Lanzando Federación Santillán Seguros (.NET 8 WinForms)..."
cd /app/publish
wine FederacionSantillan.App.exe

#!/usr/bin/env bash
set -e

echo "[Santillán Seguros] Iniciando entorno virtual visual X11..."
Xvfb :0 -screen 0 "${SCREEN_WIDTH:-1366}x${SCREEN_HEIGHT:-768}x${SCREEN_DEPTH:-24}" &
sleep 1

echo "[Santillán Seguros] Iniciando gestor de ventanas Fluxbox..."
fluxbox &
sleep 1

echo "[Santillán Seguros] Iniciando servidor VNC (x11vnc)..."
x11vnc -display :0 -nopw -forever -shared -rfbport 5900 -quiet &
sleep 1

echo "[Santillán Seguros] Iniciando servidor noVNC WebSockets en puerto 8080..."
websockify --web /usr/share/novnc 8080 localhost:5900 &
sleep 1

echo "[Santillán Seguros] Inicializando Wine prefix y tipografías..."
wineboot --init 2>/dev/null || true

mkdir -p /root/.wine/drive_c/windows/Fonts
if [ -d /usr/share/wine/fonts ]; then
    cp -n /usr/share/wine/fonts/* /root/.wine/drive_c/windows/Fonts/ 2>/dev/null || true
    cp -n /usr/share/wine/fonts/tahoma.ttf /root/.wine/drive_c/windows/Fonts/segoeui.ttf 2>/dev/null || true
    cp -n /usr/share/wine/fonts/tahomabd.ttf /root/.wine/drive_c/windows/Fonts/segoeuib.ttf 2>/dev/null || true
fi
if [ -d /usr/share/fonts/truetype/liberation ]; then
    cp -n /usr/share/fonts/truetype/liberation/* /root/.wine/drive_c/windows/Fonts/ 2>/dev/null || true
fi

# Deshabilitar diálogo de crash de Wine
wine reg add "HKEY_CURRENT_USER\\Software\\Wine\\WineDbg" /v ShowCrashDialog /t REG_DWORD /d 0 /f 2>/dev/null || true

echo "[Santillán Seguros] Lanzando Federación Santillán Seguros (.NET 8 WinForms)..."
cd /app/publish
exec wine FederacionSantillan.App.exe

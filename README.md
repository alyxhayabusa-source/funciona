# ProfileApp - Aplicación Blazor

Aplicación web desarrollada con Blazor WebAssembly.

## Requisitos

- .NET 7.0 o superior
- Node.js (para desarrollo)
- Navegador web moderno

## Configuración

1. Clona el repositorio:
   ```bash
   git clone [URL_DEL_REPOSITORIO]
   cd blazor/ProfileApp
   ```

2. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
   ```

3. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```

## Estructura del Proyecto

- `wwwroot/` - Archivos estáticos
- `Pages/` - Componentes de página
- `Shared/` - Componentes compartidos
- `wwwroot/_headers` - Configuración de cabeceras HTTP
- `appsettings.json` - Configuración de la aplicación

## Despliegue

Para desplegar en producción, ejecuta:
```bash
dotnet publish -c Release
```

## Licencia

MIT

# Seguridad

## Versiones soportadas

La línea soportada para el primer release público es **1.4.x**. Las versiones anteriores quedan como checkpoints internos y no reciben mantenimiento público.

## Reporte de vulnerabilidades

No publiques vulnerabilidades, secretos, credenciales ni datos sensibles en un issue público.

El canal oficial de contacto de seguridad se publicará junto con la configuración productiva de ElegíBien. Hasta disponer del canal oficial de seguridad del sitio productivo, los reportes pueden realizarse mediante los medios de contacto publicados en el perfil del autor.

## Configuración sensible

No deben versionarse:

- cadenas de conexión de producción;
- credenciales de SQL Server;
- claves de servicios externos;
- secretos de hosting;
- tokens de deployment.

`ConnectionStrings:ElegiBienDb` debe configurarse externamente en producción.

## Medidas implementadas

La aplicación incluye:

- HTTPS redirection;
- HSTS fuera de Development;
- manejo de errores sin detalles internos en producción;
- Content Security Policy;
- `X-Content-Type-Options: nosniff`;
- `X-Frame-Options: DENY`;
- `Referrer-Policy`;
- `Permissions-Policy`;
- `Cross-Origin-Opener-Policy`;
- `Cross-Origin-Resource-Policy`;
- antiforgery en formularios POST;
- rate limiting para formularios públicos;
- tokens compartidos aleatorios y con vencimiento.

## Dependencias

Antes de cada release se deben revisar las dependencias NuGet y client-side y ejecutar build/tests en CI.

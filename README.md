# ElegíBien

[![Build and Test](https://github.com/stimpfldev/ElegiBien.Public/actions/workflows/build-test.yml/badge.svg)](https://github.com/stimpfldev/ElegiBien.Public/actions/workflows/build-test.yml)

ElegíBien es una aplicación web gratuita orientada a ayudar a tomar decisiones de compra mediante cálculos y comparaciones explicables.

El producto está diseñado para ofrecer cálculos y comparaciones explicables, manteniendo criterios de evaluación independientes de relaciones comerciales.

## Herramientas disponibles

- Aire acondicionado: dimensionamiento orientativo y comparación de equipos.
- Pintura: cálculo de superficie/material y comparación de alternativas.
- Cerámicos y pisos: cálculo de superficie, desperdicio, cajas y comparación.
- Calefacción: dimensionamiento térmico orientativo y comparación de sistemas.

Los resultados son orientativos y no reemplazan la evaluación de un profesional cuando corresponda.

## Stack

- .NET 10
- ASP.NET Core MVC
- Entity Framework Core 10
- SQL Server
- xUnit
- HTML/CSS/JavaScript
- PWA responsive

## Arquitectura

La solución separa responsabilidades en:

- `ElegiBien.Domain`: entidades, enums y reglas de dominio.
- `ElegiBien.Application`: casos de uso, servicios de cálculo/comparación e interfaces.
- `ElegiBien.Infrastructure`: EF Core, persistencia, configuración y migraciones.
- `ElegiBien.Web`: MVC, endpoints web, UI, PWA, seguridad y configuración.
- `ElegiBien.Tests.Unit`: pruebas de reglas y servicios.
- `ElegiBien.Tests.Integration`: pruebas de integración del host web y flujos críticos.

Más detalle: [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md).

## Seguridad y privacidad

La aplicación incorpora, entre otras medidas:

- HTTPS redirection y HSTS fuera de Development.
- Content Security Policy.
- `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy` y `Permissions-Policy`.
- antiforgery en operaciones POST.
- rate limiting de formularios públicos.
- tokens públicos aleatorios para resultados compartidos.
- expiración y limpieza de tokens compartidos.
- consentimiento separado para analítica anónima.
- sin login obligatorio ni solicitud de DNI, tarjetas, cuentas bancarias o domicilio exacto.

Ver [`SECURITY.md`](SECURITY.md) y la política visible dentro de la aplicación.

## Base de datos

La base se administra con migraciones EF Core. También se mantienen scripts de validación y compatibilidad para el traspaso histórico de comparaciones.

Ver [`docs/DATABASE.md`](docs/DATABASE.md).

## Ejecutar localmente

Requisitos:

- .NET 10 SDK
- SQL Server o SQL Server LocalDB

La configuración de Development incluida utiliza LocalDB. Desde la raíz:

```powershell
dotnet restore
dotnet run --project .\ElegiBien.Web\ElegiBien.Web.csproj
```

Para cargar datos base explícitamente en Development:

```powershell
dotnet run --project .\ElegiBien.Web\ElegiBien.Web.csproj -- --seed
```

## Pruebas

```powershell
dotnet test -c Release
```

Última validación funcional local del cierre técnico previo al release público: **82 pruebas correctas, 0 errores**, más recorrido manual de las cuatro herramientas, comparaciones, enlaces compartidos, páginas legales, `robots.txt`, `sitemap.xml` y vista móvil.

## SEO y PWA

ElegíBien incluye:

- `robots.txt` generado para el host activo.
- `sitemap.xml` con las herramientas indexables.
- metadatos y canonical URLs.
- manifest PWA y service worker.
- diseño responsive.

Los resultados compartidos se excluyen de indexación.

## Estado de publicación

La versión pública actual es **1.4.0**. La configuración de producción, dominio, correo oficial y secretos se suministrarán fuera del repositorio durante el deployment.

## Licencia

El código propio de ElegíBien no se publica como software open source. Ver [`LICENSE.txt`](LICENSE.txt).

Los componentes de terceros conservan sus licencias correspondientes. Ver [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md).

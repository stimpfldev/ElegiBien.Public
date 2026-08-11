# Arquitectura de ElegíBien

## Estructura

```text
ElegiBien.Domain
      ↑
ElegiBien.Application
      ↑
ElegiBien.Infrastructure
      ↑
ElegiBien.Web
```

`ElegiBien.Web` compone la aplicación y registra las implementaciones de Infrastructure y Application.

## Domain

Contiene entidades, enums y conceptos centrales del producto. No depende de Web ni de Infrastructure.

## Application

Contiene:

- casos de uso por categoría;
- calculadoras;
- comparadores;
- interfaces de persistencia/lectura;
- DTOs necesarios para orquestar los flujos.

Los controladores delegan la lógica principal en estos casos de uso.

## Infrastructure

Contiene:

- `ElegiBienDbContext`;
- configuraciones de EF Core;
- migraciones;
- stores/readers de persistencia;
- implementación de comparaciones genéricas;
- seeding explícito.

Las comparaciones de las distintas categorías comparten el modelo genérico `ComparisonAlternatives`, `ComparisonScores` y `ComparisonFactors`.

## Web

ASP.NET Core MVC actúa como capa de presentación y composición. Incluye:

- controladores MVC;
- Razor Views;
- endpoints de resultados compartidos;
- páginas legales/contacto;
- SEO (`robots.txt`, `sitemap.xml`, canonical URLs);
- PWA;
- middleware de seguridad;
- rate limiting;
- manejo de errores de producción.

## Datos compartidos

Cada análisis utiliza un `AnalysisId` GUID interno. Un resultado compartido utiliza un token público aleatorio independiente y con vencimiento. Los resultados compartidos no se indexan.

## Consentimiento

Los consentimientos se registran de forma separada según su finalidad. La analítica funcional se registra solo cuando existe el consentimiento correspondiente.

## Características de la versión actual

- No requiere login para utilizar las herramientas públicas.
- La aplicación se distribuye como Web/PWA.
- Los puntajes se calculan únicamente a partir de las reglas definidas por la aplicación.

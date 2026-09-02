# ElegíBien 1.5.0

## Alcance

ElegíBien 1.5.0 amplía la presentación internacional de la aplicación sin modificar las reglas internas de cálculo ni el modelo de negocio.

La aplicación continúa siendo una web/PWA pública, gratuita y orientada a cálculos y comparaciones explicables.

## Novedades 1.5.0

- Selector de idioma español / inglés (`ES` / `EN`).
- Selector independiente de unidades métricas / imperiales (`MET` / `IMP`).
- Métrico como sistema predeterminado para una instalación nueva.
- Persistencia local de idioma y unidades elegidos por el usuario.
- Conversión de entradas y resultados manteniendo la lógica interna en sistema métrico.
- Longitud: `m` ↔ `ft`.
- Superficie: `m²` ↔ `ft²`.
- Volumen: `m³` ↔ `ft³`.
- Pintura: `L` ↔ `US gal` y `m²/L` ↔ `ft²/US gal`.
- Aire acondicionado: `frig/h` ↔ `BTU/h`.
- Calefacción: `W` / `kcal/h` ↔ `BTU/h`.
- Temperatura: soporte `°C` ↔ `°F` cuando corresponda.
- Traducción de navegación, Home, calculadoras, comparaciones, contacto, legales y textos comunes de resultados.
- Ajuste visual compacto del bloque principal del Home.
- PWA actualizada para cachear los nuevos recursos de idioma/unidades y resolver correctamente assets con `?v=`.

## Validación 1.5.0

- Recorrido manual aprobado en español e inglés.
- Validación manual aprobada de `MET` y `IMP` en Aire acondicionado, Pintura, Cerámicos y pisos y Calefacción.
- Validación aprobada de conversiones en formularios, resultados y comparaciones.
- Validación aprobada de `MET → IMP → MET` sin deriva material ni duplicación de unidades.
- Persistencia de preferencias validada.
- GitHub Actions `Build and Test` en verde sobre la rama de 1.5.0.
- No se modificó el esquema de base de datos; permanece vigente el baseline `database/INSTALL-1.4.0.sql`.

## Deployment

La cadena de conexión, correo oficial, dominio y demás configuración de producción deben suministrarse externamente y no se incluyen en el repositorio ni en el artefacto público.

---

# ElegíBien 1.4.0

## Alcance

Versión 1.4.0 de ElegíBien.

ElegíBien es una aplicación web/PWA que ofrece herramientas de cálculo y comparación explicables.

## Incluye

- Aire acondicionado: dimensionamiento y comparación de alternativas.
- Pintura: cálculo de material y comparación.
- Cerámicos y pisos: cálculo de superficie/desperdicio y comparación.
- Calefacción: dimensionamiento y comparación de sistemas.
- Persistencia con EF Core y SQL Server.
- Modelo genérico de comparaciones.
- Resultados compartidos mediante token aleatorio con vencimiento.
- Limpieza de resultados compartidos vencidos.
- Consentimiento diferenciado para analítica anónima.
- Manejo profesional de errores de producción.
- Headers de seguridad, CSP, antiforgery y rate limiting.
- SEO técnico: robots.txt, sitemap.xml, canonical URLs y metadata.
- PWA responsive.
- Documentación técnica, de seguridad, base de datos y legales.
- GitHub Actions para restore, build y tests.
- Script reproducible de empaquetado con ZIP y SHA-256.

## Validación previa

- 82 pruebas automatizadas correctas, 0 errores.
- Instalación limpia de base validada.
- Migraciones EF Core verificadas.
- Recorrido manual de las cuatro herramientas, comparación, resultados compartidos y vista móvil.
- robots.txt y sitemap.xml validados.

## Plataforma

Aplicación web/PWA.

## Deployment

La cadena de conexión, correo oficial, dominio y demás configuración de producción deben suministrarse externamente y no se incluyen en el repositorio ni en el artefacto público.

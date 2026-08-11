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

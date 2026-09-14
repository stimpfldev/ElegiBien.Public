# Rebranding a OPTIANZA

## Marca pública

La marca pública del producto pasa de **ElegíBien** a **OPTIANZA**.

Los nombres técnicos internos `ElegiBien.*`, namespaces, proyectos, migraciones y `ConnectionStrings:ElegiBienDb` se mantienen por compatibilidad. No deben mostrarse como marca al usuario.

## Aplicación

Actualizar a OPTIANZA en:

- Home, navegación y footer.
- Titles, descriptions y metadata SEO.
- Manifest PWA y página offline.
- Logos y textos alternativos.
- Contacto, términos, privacidad y metodología.
- Resultados compartidos y textos de analítica.

## Dominio y publicación

Dominio objetivo: `optianza.com.ar`.

Antes de cambiar DNS o enlaces públicos:

1. Registrar y confirmar el dominio.
2. Agregar el dominio al hosting.
3. Configurar DNS y HTTPS.
4. Publicar la aplicación con la nueva configuración.
5. Mantener temporalmente una redirección 301 desde el dominio anterior si continúa bajo control.
6. Actualizar canonical URLs, sitemap y Search Console una vez activo el dominio nuevo.

## Correo

Correo público objetivo: `contacto@optianza.com.ar`.

Crear la casilla en el proveedor de hosting y configurar `Contact:Email` en producción. Probar envío y recepción antes de publicarla en el sitio.

## Google

Después de activar el dominio nuevo:

- Google Analytics: conservar la propiedad si se desea continuidad histórica y verificar el nuevo dominio.
- Search Console: agregar y verificar `optianza.com.ar`, enviar sitemap y revisar indexación.
- AdSense: actualizar/agregar el sitio nuevo y completar la revisión requerida por Google.

## Sitio personal

En `PersonalWeb`, reemplazar la tarjeta pública:

- `ElegíBien` por `OPTIANZA`.
- `https://eligebien.com.ar/` por `https://optianza.com.ar/` únicamente después de confirmar que el nuevo dominio está operativo.

## GitHub

Repositorios actuales:

- `stimpfldev/ElegiBien`
- `stimpfldev/ElegiBien.Public`

Actualizar el branding visible de README y documentación a OPTIANZA. El renombrado físico de repositorios puede realizarse después de validar la aplicación y los enlaces externos, evitando romper remotes o integraciones existentes durante el cambio.

## Validación final

Antes de publicar:

- Buscar `ElegíBien`, `Elegi Bien`, `Eligi Bien` y variantes en contenido visible.
- Verificar que no queden textos públicos con la marca anterior.
- Ejecutar build y tests.
- Validar desktop/móvil y PWA.
- Validar contacto por correo.
- Validar HTTPS, sitemap, robots, canonical y Analytics.

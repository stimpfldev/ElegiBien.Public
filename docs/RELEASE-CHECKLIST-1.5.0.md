# Release checklist - OPTIANZA 1.5.0

## Estado funcional

- [x] Versión `1.5.0` en los proyectos y `VERSION.txt`.
- [x] ES / EN validado manualmente.
- [x] MET / IMP validado manualmente en las cuatro herramientas.
- [x] Comparaciones validadas con ambos sistemas de unidades.
- [x] Persistencia de idioma/unidades validada.
- [x] Vuelta `MET → IMP → MET` validada.
- [x] Default de una instalación nueva: `ES + MET`.
- [x] No se modificaron las reglas internas de cálculo.
- [x] No hubo cambios de esquema SQL respecto de 1.4.0.

## PWA 1.5.0

- [x] `presentation-preferences.css` incluido en cache estático.
- [x] `presentation-preferences.js` incluido en cache estático.
- [x] `presentation-language.js` incluido en cache estático.
- [x] Cache actualizado para aceptar assets con query string de versionado ASP.NET.
- [ ] Smoke test offline final.

## Rebranding OPTIANZA

- [x] UI y textos visibles actualizados.
- [x] SEO, metadata, PWA y logos actualizados.
- [x] Legales y documentación pública actualizados.
- [x] Se conservan los identificadores técnicos `ElegiBien.*` por compatibilidad.
- [ ] Validar `optianza.com.ar` luego del deployment.
- [ ] Validar correo `contacto@optianza.com.ar`.

## Antes del merge

- [ ] `git status` limpio.
- [ ] `git diff --check` sin errores reales.
- [ ] `dotnet test -c Release` finaliza correctamente.
- [ ] GitHub Actions `Build and Test` en verde para el SHA final de la rama.

## GitHub Release

- [ ] Crear GitHub Release `OPTIANZA 1.5.0` desde `v1.5.0` cuando corresponda.
- [ ] Usar la sección 1.5.0 de `RELEASE-NOTES.md` como base de descripción.

## Publicación

- [ ] Publicar en el sitio SharkASP `optianza`.
- [ ] Configurar HTTPS para `optianza.com.ar`.
- [ ] Configurar correo oficial.
- [ ] Actualizar PersonalWeb.
- [ ] Validar Search Console, Analytics y AdSense para el dominio nuevo.

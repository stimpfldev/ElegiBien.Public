# Release checklist - ElegíBien 1.5.0

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
- [ ] Smoke test offline final: cargar la aplicación online, pasar a otra página, desconectar red y verificar que la PWA conserva shell/recursos estáticos sin errores de idioma/unidades.

## Antes del merge

- [ ] `git pull` de `feature/i18n-units-1.5.0`.
- [ ] `git status` limpio.
- [ ] `git diff --check` sin errores reales.
- [ ] `dotnet test -c Release` finaliza correctamente.
- [ ] GitHub Actions `Build and Test` en verde para el SHA final de la rama.
- [ ] PR #3 deja de estar en Draft y queda Ready for review.

## Merge a main

Solo después de completar el bloque anterior:

- [ ] Merge de PR #3 a `main`.
- [ ] Confirmar `main` en el SHA esperado.
- [ ] Confirmar GitHub Actions verde sobre el estado integrado.

## Artefactos 1.5.0

Solo después del merge validado:

- [ ] Ejecutar `scripts/Crear-Release-1.5.0.ps1`.
- [ ] Verificar `artifacts/ElegiBien-1.5.0.zip`.
- [ ] Verificar `artifacts/ElegiBien-1.5.0.zip.sha256`.
- [ ] Ejecutar `scripts/Validar-PrePublicacion-1.5.0.ps1`.
- [ ] Confirmar `PRE-PUBLICATION VALIDATION: OK`.
- [ ] Recalcular SHA-256 local y confirmar coincidencia.
- [ ] Confirmar que `appsettings.Development.json` no está en el ZIP.
- [ ] Confirmar que los assets ES/EN y MET/IMP están dentro del ZIP.

## Base de datos

- [x] 1.5.0 no agrega migraciones ni modifica el esquema.
- [x] Baseline de instalación vigente: `database/INSTALL-1.4.0.sql`.
- [ ] Antes de un deployment real, ejecutar/validar el baseline solamente si se instala sobre una base nueva.

## Tag y GitHub Release

Solo después de artefactos y SHA validados:

- [ ] Crear tag `v1.5.0` sobre el SHA validado de `main`.
- [ ] Publicar el tag.
- [ ] Crear GitHub Release `ElegíBien 1.5.0` desde `v1.5.0`.
- [ ] Adjuntar ZIP y `.sha256`.
- [ ] Usar la sección 1.5.0 de `RELEASE-NOTES.md` como base de descripción.
- [ ] Confirmar digest del asset publicado contra SHA local.

## Backup

- [ ] Sincronizar el repositorio privado de backup.
- [ ] Confirmar que público y backup contienen el mismo SHA validado de 1.5.0.

## Fuera de este cierre

Continúan intencionalmente fuera de este bloque hasta decisión explícita:

- hosting/deployment productivo;
- publicación/promoción en sitio personal;
- publicación en LinkedIn.

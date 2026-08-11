# Release checklist - ElegíBien 1.4.0

## Antes del commit

- [ ] `git status` revisado.
- [ ] `git diff --check` sin errores reales.
- [ ] Todos los proyectos informan versión `1.4.0` / `1.4.0.0`.
- [ ] `VERSION.txt` indica `1.4.0`.
- [ ] `dotnet test -c Release` finaliza con 82/82 correctos.
- [ ] No existen secretos ni cadenas de conexión productivas versionadas.

## Publicación del commit

- [ ] Commit final creado.
- [ ] Working tree limpio.
- [ ] Push a `master` realizado.
- [ ] GitHub Actions `Build and Test` finaliza en verde para el SHA del commit.

## Tag y artefactos

Solo después de CI verde:

- [ ] Crear tag `v1.4.0` sobre el SHA validado.
- [ ] Publicar el tag.
- [ ] Ejecutar `scripts/Crear-Release-1.4.0.ps1`.
- [ ] Verificar `artifacts/ElegiBien-1.4.0.zip`.
- [ ] Verificar `artifacts/ElegiBien-1.4.0.zip.sha256`.
- [ ] Recalcular SHA-256 local y confirmar coincidencia.

## GitHub Release

- [ ] Crear release `ElegíBien 1.4.0` desde `v1.4.0`.
- [ ] No marcar prerelease salvo decisión explícita.
- [ ] Adjuntar ZIP y `.sha256`.
- [ ] Usar `RELEASE-NOTES.md` como base de descripción.
- [ ] Confirmar digest del asset publicado contra SHA local.

## Backup

- [ ] Sincronizar repositorio privado de backup si existe.
- [ ] Confirmar que público y backup terminan en el mismo SHA.

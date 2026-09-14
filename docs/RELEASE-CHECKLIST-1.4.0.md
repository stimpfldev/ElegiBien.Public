# Release checklist - OPTIANZA 1.4.0

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
- [ ] Push realizado.
- [ ] GitHub Actions `Build and Test` finaliza en verde para el SHA del commit.

## GitHub Release

- [ ] Crear release `OPTIANZA 1.4.0` desde `v1.4.0` si se conserva esta versión histórica.
- [ ] No marcar prerelease salvo decisión explícita.
- [ ] Usar `RELEASE-NOTES.md` como base de descripción.

## Compatibilidad técnica

Los identificadores internos `ElegiBien.*` se mantienen sin cambios. La marca pública es OPTIANZA.

## Backup

- [ ] Sincronizar repositorio privado de backup si existe.
- [ ] Confirmar que público y backup terminan en el mismo SHA.

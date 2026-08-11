# Base de datos y migraciones

## Base local de Development

Configuración predeterminada de Development:

```text
Server=(localdb)\MSSQLLocalDB;Database=ElegiBien_Dev;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

La cadena de producción no se guarda en `appsettings.json`; debe suministrarse mediante configuración externa.

## Aplicar migraciones

Desde la raíz de la solución:

```powershell
dotnet ef database update --project .\ElegiBien.Infrastructure\ElegiBien.Infrastructure.csproj --startup-project .\ElegiBien.Web\ElegiBien.Web.csproj
```

Para una base específica:

```powershell
dotnet ef database update --project .\ElegiBien.Infrastructure\ElegiBien.Infrastructure.csproj --startup-project .\ElegiBien.Web\ElegiBien.Web.csproj --connection "<CONNECTION_STRING>"
```

## Seed

El seed no se ejecuta automáticamente al iniciar la aplicación. En Development debe solicitarse explícitamente:

```powershell
dotnet run --project .\ElegiBien.Web\ElegiBien.Web.csproj -- --seed
```

## Validación

Después de instalar/migrar una base, ejecutar en SSMS:

```text
database/VALIDAR_ESTADO_BASE.sql
```

La salida esperada para el esquema actual es:

```text
OK - base alineada con ElegiBien 1.4.0
```

La validación comprueba también el historial de migraciones y la presencia del modelo genérico de comparaciones.

## Actualización desde el esquema legacy de comparaciones

El repositorio contiene:

```text
database/MIGRAR_COMPARACIONES_HISTORICAS.sql
```

Ese script existe para preservar datos de instalaciones antiguas que todavía posean tablas de comparación específicas por categoría. Es re-ejecutable y verifica el estado del esquema antes de insertar datos.

No debe ejecutarse a ciegas sobre una base desconocida. Primero ejecutar `VALIDAR_ESTADO_BASE.sql` y conservar un backup de la base antes de una actualización productiva.

## Instalación limpia validada

El procedimiento de instalación desde una base vacía fue verificado ejecutando todas las migraciones EF Core, seed explícito y el script de validación final sobre una base independiente.

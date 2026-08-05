# Revisión de arquitectura — ElegíBien 1.3.0

## Corregido en este paquete

- Navegación de Calefacción y Contacto.
- Accesos desde portada y pie.
- Configuración `Contact:Email`.
- Desplazamiento automático a resultados y errores.
- Botones visibles y formularios adaptados a móviles.
- Cerámicos calcula el material adicional sin pedir edición manual.
- Versiones de proyectos en 1.3.0.
- Paquetes de integración alineados con EF Core 10.0.10.
- Versión legal de consentimientos alineada a 1.0.0.
- Pruebas de navegación básica.

## Pendiente para una versión posterior

No se incluye en este fix porque implica un refactor transversal con riesgo sobre categorías cerradas:

1. Unificar las entidades de comparación genéricas y específicas.
2. Mover la orquestación de los controladores a casos de uso de Application.
3. Separar el registro de dependencias de `Program.cs` en métodos `AddApplication` y `AddInfrastructure`.
4. Separar migración/seed del arranque normal para producción.
5. Definir y probar una Content Security Policy antes de publicar.
6. Ampliar pruebas de consentimiento y enlaces compartidos válidos.

Los archivos `bin`, `obj`, `.vs` y `*.csproj.user` no forman parte del paquete fix.

# Google SEO, Search Console, GA4 y AdSense

Estado técnico preparado el 30-08-2026.

## Principio
Las integraciones externas permanecen deshabilitadas por defecto. No se versionan identificadores reales ni credenciales. La activación se realiza mediante configuración de producción.

## SEO técnico
Implementado: title y meta description por página, canonical, Open Graph/Twitter, `/robots.txt`, `/sitemap.xml`, exclusión de `/Shared/` y no carga de GA4/AdSense en páginas no indexables.

Validación en producción: comprobar HTTP 200 de `/robots.txt` y `/sitemap.xml`, dominio HTTPS correcto en sitemap/canonical y ausencia de resultados compartidos en sitemap.

## Search Console
Configurar preferentemente por DNS. Si se usa meta HTML, colocar sólo el token en `GoogleSearchConsole:VerificationToken`; el layout genera la etiqueta automáticamente.

Cierre: propiedad verificada, sitemap enviado y leído por Google.

## Google Analytics 4
Configurar `GoogleAnalytics:MeasurementId` con `G-XXXXXXXXXX` y `GoogleAnalytics:Enabled=true`. La implementación carga una sola instancia de gtag.js y usa Consent Mode v2. La publicidad queda denegada y Analytics queda restringido en EEE/Reino Unido/Suiza hasta una señal válida.

Cierre: `page_view` visible en Realtime/DebugView y sin errores CSP.

## Google AdSense
Configurar `GoogleAdSense:PublisherId` con `ca-pub-XXXXXXXXXXXXXXXX` y `GoogleAdSense:Enabled=true` sólo después de agregar el sitio a AdSense. `/ads.txt` se genera automáticamente con el publisher configurado. La CSP usa nonce por request.

En regiones donde Google exige una CMP certificada, configurar el mecanismo de consentimiento desde AdSense antes de servir anuncios que lo requieran.

Cierre: `/ads.txt` correcto, AdSense detecta el código, sitio aprobado y anuncios servidos sin errores CSP/política.

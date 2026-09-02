"use strict";

(() => {
    const LANGUAGE_KEY = "elegibien.language";

    const translations = new Map([
        ["Métrico", "Metric"],
        ["Imperial", "Imperial"],
        ["Idioma", "Language"],
        ["Sistema de unidades", "Unit system"],
        ["ElegíBien, inicio", "ElegíBien, home"],
        ["Enlaces legales", "Legal links"],

        ["Decisiones de compra, sin adivinar", "Purchase decisions without guessing"],
        ["Calculá lo que necesitás. Compará con criterios claros.", "Calculate what you need. Compare with clear criteria."],
        ["ElegíBien transforma medidas y datos simples en recomendaciones orientativas para ayudarte a comprar con más información.", "ElegíBien turns measurements and simple data into practical estimates to help you make more informed purchases."],
        ["Herramientas principales de ElegíBien", "Main ElegíBien tools"],
        ["Ver detalles de todas las herramientas", "See details for all tools"],
        ["Características de ElegíBien", "ElegíBien features"],
        ["Cálculos explicables", "Explainable calculations"],
        ["Sin registro", "No registration"],
        ["Privacidad por diseño", "Privacy by design"],
        ["Ilustración de una decisión de compra informada", "Illustration of an informed purchase decision"],
        ["Herramientas disponibles", "Available tools"],
        ["Elegí qué necesitás resolver", "Choose what you need to solve"],
        ["Cada categoría usa reglas específicas y muestra cómo se obtiene el resultado.", "Each category uses specific rules and shows how the result is obtained."],
        ["Disponible", "Available"],
        ["Calculá un rango de frigorías y compará equipos según capacidad, eficiencia, precio y garantía.", "Calculate a cooling-capacity range and compare units by capacity, efficiency, price, and warranty."],
        ["Estimá litros, manos y envases, y compará productos por costo, rendimiento y prestaciones.", "Estimate paint quantity, coats, and containers, and compare products by cost, coverage, and features."],
        ["Calculá superficie, material adicional y cajas necesarias. Compará costo, cobertura y resistencia.", "Calculate area, extra material, and boxes required. Compare cost, coverage, and durability."],
        ["Estimá la potencia necesaria y compará equipos por consumo, eficiencia, instalación y precio.", "Estimate required heating power and compare units by consumption, efficiency, installation, and price."],
        ["Empezar cálculo", "Start calculation"],
        ["¿Detectaste un error o querés proponer una mejora?", "Did you find an error or want to suggest an improvement?"],
        ["El canal está preparado para consultas, sugerencias y oportunidades de colaboración.", "This channel is available for questions, suggestions, and collaboration opportunities."],
        ["Contactanos", "Contact us"],

        ["Estamos para escucharte", "We are listening"],
        ["Podés escribirnos por consultas, sugerencias, errores detectados u oportunidades de colaboración.", "You can contact us with questions, suggestions, reported errors, or collaboration opportunities."],
        ["Correo electrónico", "Email"],
        ["Respondemos desde la dirección oficial de ElegíBien.", "We reply from the official ElegíBien address."],
        ["El canal de contacto está preparado. La dirección oficial se publicará próximamente.", "The contact channel is ready. The official address will be published soon."],

        ["Política de privacidad", "Privacy Policy"],
        ["Última actualización: agosto de 2026.", "Last updated: August 2026."],
        ["Datos utilizados", "Data we use"],
        ["ElegíBien utiliza los datos técnicos ingresados para realizar cálculos y comparar alternativas. Según la herramienta, pueden incluir dimensiones, características del ambiente, capacidades, precios, consumo, garantía u otros datos técnicos necesarios para producir el resultado.", "ElegíBien uses the technical data you enter to perform calculations and compare alternatives. Depending on the tool, this may include dimensions, room characteristics, capacities, prices, consumption, warranty, or other technical data required to produce the result."],
        ["Datos que no solicitamos", "Data we do not request"],
        ["No solicitamos nombre, DNI, teléfono, domicilio exacto, tarjetas, cuentas bancarias ni documentación personal para utilizar las herramientas.", "We do not request your name, national ID, phone number, exact address, cards, bank accounts, or personal documentation to use the tools."],
        ["Seguridad y prevención de abuso", "Security and abuse prevention"],
        ["La dirección IP puede procesarse temporalmente para seguridad y limitación de solicitudes. No se incorpora como dato analítico del análisis.", "The IP address may be processed temporarily for security and request limiting. It is not stored as analytical data for the calculation."],
        ["Analítica web", "Web analytics"],
        ["ElegíBien puede utilizar Google Analytics para obtener métricas agregadas sobre el uso del sitio, como visitas, páginas consultadas y funcionamiento general de las herramientas.", "ElegíBien may use Google Analytics to obtain aggregated site-usage metrics such as visits, viewed pages, and general tool performance."],
        ["Cuando Google Analytics está habilitado, su funcionamiento respeta las preferencias de privacidad aplicables según la región del visitante. En las regiones configuradas para requerir una decisión previa, la medición permanece restringida hasta que el usuario establezca sus preferencias mediante el mecanismo de consentimiento correspondiente.", "When Google Analytics is enabled, it follows the privacy preferences applicable to the visitor's region. In regions configured to require a prior choice, measurement remains restricted until the user sets the applicable consent preferences."],
        ["Métricas internas y Radar ElegíBien", "Internal metrics and ElegíBien Radar"],
        ["La incorporación de eventos anónimos a métricas internas requiere consentimiento. La autorización para utilizar datos técnicos en futuras estadísticas agregadas de Radar ElegíBien se solicita por separado.", "Adding anonymous events to internal metrics requires consent. Permission to use technical data in future aggregated ElegíBien Radar statistics is requested separately."],
        ["Resultados compartidos y retención", "Shared results and retention"],
        ["Cuando se genera un enlace compartible, el resultado se identifica mediante un token aleatorio. El enlace tiene una vigencia máxima de doce meses desde su creación.", "When a shareable link is created, the result is identified by a random token. The link remains valid for up to twelve months from creation."],
        ["Una vez vencido, el registro que contiene el token público se elimina automáticamente y el resultado deja de ser accesible mediante ese enlace. Los datos técnicos internos asociados al análisis pueden permanecer en la base operativa. No contienen los datos identificatorios enumerados anteriormente y no se incorporan a Radar o analítica agregada sin el consentimiento correspondiente.", "After expiration, the record containing the public token is automatically deleted and the result is no longer accessible through that link. Internal technical data associated with the analysis may remain in the operational database. It does not contain the identifying information listed above and is not included in Radar or aggregated analytics without the applicable consent."],
        ["Protección", "Protection"],
        ["Aplicamos HTTPS, validaciones, protección antiforgery, políticas de seguridad del navegador, limitación de solicitudes y controles para reducir exposición accidental de información técnica.", "We use HTTPS, validation, antiforgery protection, browser security policies, request limiting, and controls designed to reduce accidental exposure of technical information."],
        ["Consultas y derechos sobre datos", "Questions and data rights"],
        ["Si considerás que ElegíBien almacena información que te identifica o necesitás realizar una consulta sobre acceso, actualización, rectificación o supresión, utilizá el canal publicado en la página de", "If you believe ElegíBien stores information that identifies you, or you need to ask about access, updating, correction, or deletion, use the channel published on the"],
        ["La dirección oficial de correo se incorporará en la configuración de producción antes de la publicación definitiva.", "The official email address will be added to the production configuration before the final public launch."],

        ["Términos de uso", "Terms of Use"],
        ["Resultados orientativos", "Estimated results"],
        ["Los cálculos, comparaciones y puntajes proporcionados por ElegíBien son orientativos y no reemplazan la evaluación de un técnico, instalador, fabricante o profesional matriculado cuando corresponda.", "Calculations, comparisons, and scores provided by ElegíBien are estimates and do not replace an assessment by a technician, installer, manufacturer, or qualified professional when applicable."],
        ["Datos ingresados", "Entered data"],
        ["La precisión del resultado depende de la calidad y exactitud de los datos proporcionados por el usuario.", "Result accuracy depends on the quality and accuracy of the data provided by the user."],
        ["Precios, productos y disponibilidad", "Prices, products, and availability"],
        ["Los precios, características y disponibilidad de productos ingresados por usuarios o provenientes de fuentes externas pueden cambiar y no son garantizados por ElegíBien.", "Prices, product characteristics, and availability entered by users or obtained from external sources may change and are not guaranteed by ElegíBien."],
        ["Puntaje ElegíBien", "ElegíBien Score"],
        ["El puntaje se calcula mediante reglas de adecuación y otros criterios técnicos publicados en la sección de metodología de cada herramienta.", "The score is calculated using suitability rules and other technical criteria published in each tool's methodology section."],
        ["Afiliados, anunciantes, comercios y patrocinadores no pueden modificar las reglas de cálculo ni el puntaje ElegíBien.", "Affiliates, advertisers, retailers, and sponsors cannot modify the calculation rules or the ElegíBien Score."],
        ["Publicidad y enlaces de afiliados", "Advertising and affiliate links"],
        ["ElegíBien puede incorporar publicidad, enlaces de afiliados o referencias a comercios. Algunas interacciones comerciales pueden generar una compensación para el proyecto. Cuando corresponda, esa relación debe identificarse de forma clara y no altera el resultado técnico ni el orden determinado por el puntaje.", "ElegíBien may include advertising, affiliate links, or retailer references. Some commercial interactions may generate compensation for the project. When applicable, that relationship must be clearly identified and does not alter the technical result or the ranking determined by the score."],
        ["Enlaces externos", "External links"],
        ["Los sitios, comercios o servicios externos son responsables de sus propios contenidos, precios, disponibilidad, políticas y condiciones de contratación.", "External sites, retailers, or services are responsible for their own content, prices, availability, policies, and contractual terms."],
        ["Limitación de responsabilidad", "Limitation of liability"],
        ["Las condiciones reales del ambiente, la construcción, la instalación y el producto pueden modificar el resultado final. La decisión de compra, contratación e instalación corresponde al usuario.", "Actual room, construction, installation, and product conditions may change the final result. The user is responsible for purchasing, contracting, and installation decisions."],

        ["Metodología ElegíBien", "ElegíBien Methodology"],
        ["Aire acondicionado — versión 1.0.0", "Air conditioning — version 1.0.0"],
        ["El dimensionamiento utiliza las medidas del ambiente y ajustes simplificados. El modo rápido asume una altura estándar de 2,60 metros.", "Sizing uses room dimensions and simplified adjustments. Quick mode assumes a standard height of 2.60 meters."],
        ["Adecuación de capacidad: hasta 55 puntos.", "Capacity suitability: up to 55 points."],
        ["Eficiencia y consumo: hasta 20 puntos.", "Efficiency and consumption: up to 20 points."],
        ["Precio relativo: hasta 15 puntos.", "Relative price: up to 15 points."],
        ["Garantía: hasta 10 puntos.", "Warranty: up to 10 points."],
        ["Un producto insuficiente no puede ganar solamente por tener menor precio.", "An undersized product cannot win solely because it has a lower price."],
        ["Pintura — versión 1.0.0", "Paint — version 1.0.0"],
        ["La superficie de paredes se calcula con el perímetro y la altura. Cuando corresponde, se suma el techo.", "Wall area is calculated from perimeter and height. The ceiling is added when applicable."],
        ["Se descuentan 1,80 m² por puerta y 1,50 m² por ventana como referencias estándar.", "Standard deductions of 1.80 m² per door and 1.50 m² per window are applied."],
        ["La cantidad orientativa incorpora las manos seleccionadas, el estado de la superficie y un margen de desperdicio del 10 %.", "The estimated quantity includes the selected number of coats, surface condition, and a 10% waste allowance."],
        ["La referencia inicial utiliza un rendimiento de 10 m² por litro y por mano.", "The initial reference uses coverage of 10 m² per liter per coat."],
        ["Aprovechamiento de los envases: hasta 45 puntos.", "Container utilization: up to 45 points."],
        ["Costo total necesario: hasta 30 puntos.", "Required total cost: up to 30 points."],
        ["Lavabilidad: hasta 15 puntos.", "Washability: up to 15 points."],
        ["Tiempo de secado: hasta 10 puntos.", "Drying time: up to 10 points."],
        ["En la comparación se utiliza el rendimiento informado para cada pintura. El precio se evalúa sobre la cantidad completa de envases necesarios, no sobre el precio aislado de un solo envase.", "The comparison uses the stated coverage for each paint. Price is evaluated using the full number of containers required, not the isolated price of a single container."],
        ["Cerámicos y pisos — versión 1.0.0", "Flooring and tiles — version 1.0.0"],
        ["La superficie se calcula multiplicando el largo por el ancho de un piso rectangular. Luego se agrega material adicional para cortes, roturas y futuras reposiciones.", "Area is calculated by multiplying the length by the width of a rectangular floor. Extra material is then added for cuts, breakage, and future replacements."],
        ["Los valores iniciales recomendados son 10 % para colocación recta, 12 % para colocación trabada y 15 % para colocación diagonal. El usuario puede ajustar el porcentaje según su situación.", "Recommended initial values are 10% for straight installation, 12% for staggered installation, and 15% for diagonal installation. The user can adjust the percentage to suit the situation."],
        ["La cantidad de cajas se redondea siempre hacia arriba porque no es posible comprar una fracción de caja. El costo se calcula usando la cantidad completa de cajas necesarias.", "The number of boxes is always rounded up because a fraction of a box cannot be purchased. Cost is calculated using the complete number of boxes required."],
        ["Cobertura real: hasta 35 puntos.", "Actual coverage: up to 35 points."],
        ["Costo total: hasta 30 puntos.", "Total cost: up to 30 points."],
        ["Material excedente: hasta 10 puntos.", "Excess material: up to 10 points."],
        ["Resistencia de uso: hasta 15 puntos.", "Durability: up to 15 points."],
        ["Facilidad de reposición: hasta 10 puntos.", "Replacement availability: up to 10 points."],
        ["La comparación prioriza la cobertura suficiente y el costo total de compra, sin elegir automáticamente un producto solo porque una caja individual tenga menor precio.", "The comparison prioritizes sufficient coverage and total purchase cost rather than automatically choosing a product simply because one individual box is cheaper."],
        ["Calefacción — versión 1.0.0", "Heating — version 1.0.0"],
        ["La potencia térmica orientativa se calcula a partir de la superficie, el volumen del ambiente, la zona climática, el aislamiento, las paredes exteriores, la exposición de ventanas y la conexión con otros espacios.", "Estimated heating power is calculated from area, room volume, climate zone, insulation, exterior walls, window exposure, and connection to other spaces."],
        ["El resultado muestra un rango recomendado y una potencia ideal aproximada en watts y kilocalorías por hora. Los valores no reemplazan el cálculo de cargas térmicas ni la revisión de un instalador matriculado.", "The result shows a recommended range and an approximate ideal power in watts and kilocalories per hour. These values do not replace a heat-load calculation or review by a qualified installer."],
        ["Adecuación de potencia: hasta 35 puntos.", "Power suitability: up to 35 points."],
        ["Costo estimado de uso: hasta 25 puntos.", "Estimated operating cost: up to 25 points."],
        ["Eficiencia: hasta 15 puntos.", "Efficiency: up to 15 points."],
        ["Seguridad e instalación: hasta 15 puntos.", "Safety and installation: up to 15 points."],
        ["Precio del equipo: hasta 10 puntos.", "Equipment price: up to 10 points."],
        ["Un equipo cuya potencia no cubra el rango mínimo recomendado no puede ser elegido solamente por su menor precio.", "A unit whose power does not cover the recommended minimum range cannot be selected solely because of a lower price."],
        ["Alcance", "Scope"],
        ["Los resultados son orientativos y no reemplazan la inspección de la superficie, la ficha técnica del fabricante ni la evaluación de un profesional.", "Results are estimates and do not replace surface inspection, manufacturer specifications, or professional evaluation."],

        ["Compará dos equipos", "Compare two units"],
        ["Equipo A", "Unit A"], ["Equipo B", "Unit B"],
        ["Nombre o modelo", "Name or model"],
        ["Capacidad en frigorías", "Cooling capacity"],
        ["Precio total", "Total price"],
        ["Tecnología", "Technology"],
        ["No lo sé", "I don't know"],
        ["Convencional", "Conventional"],
        ["Consumo nominal en watts", "Nominal power consumption"],
        ["Opcional.", "Optional."],
        ["Garantía en meses", "Warranty in months"],
        ["Comparar equipos", "Compare units"],
        ["Resultado ElegíBien", "ElegíBien result"],
        ["Capacidad:", "Capacity:"],
        ["Confianza:", "Confidence:"],
        ["Insuficiente", "Insufficient"], ["Correcta", "Correct"],
        ["Sobredimensionada aceptable", "Acceptably oversized"],
        ["Sobredimensionada relevante", "Significantly oversized"],
        ["Desconocida", "Unknown"],
        ["Adecuación de capacidad", "Capacity suitability"],
        ["Eficiencia", "Efficiency"],
        ["Precio relativo", "Relative price"],
        ["Factor", "Factor"]
    ]);

    const originals = new WeakMap();
    const attributeOriginals = new WeakMap();
    const compact = value => value.replace(/\s+/g, " ").trim();

    function getLanguage() {
        return localStorage.getItem(LANGUAGE_KEY) === "en" ? "en" : "es";
    }

    function apply() {
        const language = getLanguage();
        const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
        const nodes = [];
        while (walker.nextNode()) nodes.push(walker.currentNode);

        for (const node of nodes) {
            const parent = node.parentElement;
            if (!parent || ["SCRIPT", "STYLE", "CODE", "PRE"].includes(parent.tagName)) continue;
            if (!originals.has(node)) originals.set(node, node.nodeValue);
            const source = originals.get(node);
            if (language === "es") {
                node.nodeValue = source;
                continue;
            }
            const translated = translations.get(compact(source));
            if (!translated) continue;
            const leading = source.match(/^\s*/)?.[0] ?? "";
            const trailing = source.match(/\s*$/)?.[0] ?? "";
            node.nodeValue = `${leading}${translated}${trailing}`;
        }

        document.querySelectorAll("[aria-label], [alt]").forEach(element => {
            let original = attributeOriginals.get(element);
            if (!original) {
                original = {
                    ariaLabel: element.getAttribute("aria-label"),
                    alt: element.getAttribute("alt")
                };
                attributeOriginals.set(element, original);
            }
            for (const [attribute, value] of [["aria-label", original.ariaLabel], ["alt", original.alt]]) {
                if (value === null) continue;
                element.setAttribute(attribute, language === "en" ? (translations.get(compact(value)) ?? value) : value);
            }
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.addEventListener("click", () => window.setTimeout(apply, 0));
        });
        apply();
    });
})();

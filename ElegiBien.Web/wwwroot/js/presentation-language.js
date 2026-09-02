"use strict";

(() => {
    const LANGUAGE_KEY = "elegibien.language";

    const pairs = [
        ["Inicio", "Home"],
        ["Aire acondicionado", "Air conditioning"],
        ["Pintura", "Paint"],
        ["Cerámicos y pisos", "Flooring and tiles"],
        ["Calefacción", "Heating"],
        ["Contacto", "Contact"],
        ["Cómo decidimos", "How we decide"],
        ["Privacidad", "Privacy"],
        ["Términos", "Terms"],
        ["Metodología", "Methodology"],
        ["Idioma", "Language"],
        ["Sistema de unidades", "Unit system"],
        ["ElegíBien, inicio", "ElegíBien, home"],
        ["Enlaces legales", "Legal links"],
        ["Abrir navegación", "Open navigation"],
        ["Hecho en Argentina", "Made in Argentina"],
        ["Decisiones de compra más claras, con cálculos y criterios explicables.", "Clearer purchase decisions with transparent calculations and criteria."],

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

        ["Qué aire acondicionado necesitás", "What air conditioner do you need?"],
        ["Ingresá cuatro datos y obtené un rango orientativo de frigorías para tu ambiente.", "Enter four values to get an estimated cooling-capacity range for your room."],
        ["Largo del ambiente", "Room length"],
        ["Ancho del ambiente", "Room width"],
        ["Altura del ambiente", "Room height"],
        ["Cantidad habitual de personas", "Usual number of people"],
        ["Exposición al sol", "Sun exposure"],
        ["Baja", "Low"],
        ["Media", "Medium"],
        ["Alta", "High"],
        ["Muy alta", "Very high"],
        ["Permitir estadísticas anónimas para mejorar ElegíBien.", "Allow anonymous statistics to improve ElegíBien."],
        ["Permitir que los datos anónimos de esta consulta se incorporen en futuras estadísticas del Radar ElegíBien.", "Allow anonymous data from this query to be included in future ElegíBien Radar statistics."],
        ["No solicitamos nombre, documento, teléfono, domicilio ni información bancaria.", "We do not request your name, ID, phone number, address, or banking information."],
        ["Calcular frigorías", "Calculate cooling capacity"],
        ["Resultado orientativo", "Estimated result"],
        ["Necesitás aproximadamente entre", "You need approximately between"],
        ["Para este ambiente necesitás entre", "For this room you need between"],
        ["y", "and"],
        ["frigorías.", "cooling units."],
        ["frigorías", "cooling units"],
        ["Capacidad ideal aproximada:", "Approximate ideal capacity:"],
        ["Superficie:", "Area:"],
        ["Nivel de confianza:", "Confidence level:"],
        ["Comparar dos equipos", "Compare two units"],
        ["Este ambiente presenta condiciones que requieren revisión profesional antes de comprar o instalar el equipo.", "This room has conditions that should be reviewed by a professional before purchasing or installing the unit."],
        ["El resultado es orientativo y no reemplaza la evaluación de un técnico matriculado.", "This result is an estimate and does not replace an assessment by a qualified technician."],
        ["El cálculo utiliza las dimensiones del ambiente, una altura estándar de 2,60 metros, la cantidad de personas y la exposición solar.", "The calculation uses the room dimensions, a standard height of 2.60 meters, the number of people, and sun exposure."],

        ["Cuánta pintura necesitás", "How much paint do you need?"],
        ["Calculá la superficie y una cantidad orientativa de litros.", "Calculate the area and an estimated amount of paint."],
        ["Cantidad de puertas", "Number of doors"],
        ["Cantidad de ventanas", "Number of windows"],
        ["Cantidad de manos", "Number of coats"],
        ["Estado de la superficie", "Surface condition"],
        ["Incluir el techo", "Include ceiling"],
        ["Incluir cielorraso", "Include ceiling"],
        ["Buena", "Good"],
        ["Nueva o porosa", "New or porous"],
        ["Dañada", "Damaged"],
        ["Permitir incorporar esta consulta anónima al futuro Radar ElegíBien.", "Allow this anonymous query to be included in the future ElegíBien Radar."],
        ["Calcular pintura", "Calculate paint"],
        ["Superficie neta:", "Net area:"],
        ["Superficie ajustada:", "Adjusted area:"],
        ["Referencia:", "Reference:"],
        ["Referencia inicial:", "Initial estimate:"],
        ["para", "for"],
        ["manos.", "coats."],
        ["La superficie requiere revisión o preparación profesional antes de pintar.", "The surface requires professional review or preparation before painting."],
        ["El resultado es orientativo. Revisá el rendimiento indicado por el fabricante.", "This result is an estimate. Check the coverage specified by the manufacturer."],
        ["Comparar dos pinturas", "Compare two paints"],
        ["El cálculo descuenta puertas y ventanas estándar, aplica el estado de la superficie, las manos elegidas y un margen de desperdicio del 10 %.", "The calculation deducts standard doors and windows and applies the surface condition, selected coats, and a 10% waste allowance."],

        ["Cuántos cerámicos o pisos necesitás", "How much flooring or tile do you need?"],
        ["Calculá la superficie y el material adicional recomendado para cortes, roturas y futuras reposiciones.", "Calculate the area and the recommended extra material for cuts, breakage, and future replacements."],
        ["Largo del piso", "Floor length"],
        ["Ancho del piso", "Floor width"],
        ["Tipo de colocación", "Installation pattern"],
        ["Recta", "Straight"],
        ["Trabada", "Staggered"],
        ["Diagonal", "Diagonal"],
        ["Material adicional calculado", "Calculated extra material"],
        ["para cortes, roturas y futuras reposiciones.", "for cuts, breakage, and future replacements."],
        ["porque la colocación trabada suele requerir más cortes.", "because staggered installation usually requires more cuts."],
        ["porque la colocación diagonal requiere más cortes.", "because diagonal installation requires more cuts."],
        ["Permitir analítica anónima para mejorar ElegíBien.", "Allow anonymous analytics to improve ElegíBien."],
        ["Permitir analítica anónima para mejorar ElegíBien", "Allow anonymous analytics to improve ElegíBien"],
        ["Permitir el uso anónimo de este resultado en estadísticas agregadas.", "Allow anonymous use of this result in aggregated statistics."],
        ["Permitir el uso anónimo de este resultado en estadísticas agregadas", "Allow anonymous use of this result in aggregated statistics"],
        ["Calcular superficie", "Calculate area"],
        ["Superficie del piso", "Floor area"],
        ["Material adicional aplicado", "Extra material applied"],
        ["Superficie adicional", "Extra area"],
        ["Superficie total necesaria", "Total required area"],
        ["Conviene revisar medidas, cortes y disposición antes de comprar.", "Review measurements, cuts, and layout before purchasing."],
        ["Comparar dos productos", "Compare two products"],
        ["La estimación calcula la superficie rectangular y agrega el porcentaje de desperdicio elegido según el tipo de colocación.", "The estimate calculates the rectangular area and adds the selected waste percentage according to the installation pattern."],

        ["Qué potencia de calefacción necesitás", "How much heating power do you need?"],
        ["Estimá la potencia necesaria para calefaccionar el ambiente sin elegir un equipo demasiado chico o excesivo.", "Estimate the power required to heat the room without choosing a unit that is too small or unnecessarily large."],
        ["Zona climática", "Climate zone"],
        ["Nivel de aislamiento", "Insulation level"],
        ["Nivel de aislación", "Insulation level"],
        ["Paredes que dan al exterior", "Exterior walls"],
        ["Paredes exteriores", "Exterior walls"],
        ["Ambiente abierto hacia otro espacio", "Open to another space"],
        ["Abierto a otro ambiente", "Open to another space"],
        ["Templada", "Mild"],
        ["Templada fría", "Cool temperate"],
        ["Fría", "Cold"],
        ["Muy fría", "Very cold"],
        ["Bueno", "Good"],
        ["Normal", "Normal"],
        ["Deficiente", "Poor"],
        ["Ninguna", "None"],
        ["Una", "One"],
        ["Dos", "Two"],
        ["Tres", "Three"],
        ["Cuatro", "Four"],
        ["Pocas o normales", "Few or average"],
        ["Varias o grandes", "Several or large"],
        ["Ventanal amplio", "Large glazing"],
        ["Calcular potencia", "Calculate heating power"],
        ["Superficie del ambiente", "Room area"],
        ["Volumen calculado", "Calculated volume"],
        ["Potencia mínima recomendada", "Recommended minimum power"],
        ["Potencia máxima recomendada", "Recommended maximum power"],
        ["Potencia ideal orientativa", "Estimated ideal power"],
        ["Equivalencia aproximada", "Approximate equivalent"],
        ["Por las características del ambiente, conviene validar la instalación y la potencia con un profesional.", "Because of the room characteristics, the installation and required power should be validated by a professional."],

        ["Compará dos equipos", "Compare two units"],
        ["Equipo A", "Unit A"],
        ["Equipo B", "Unit B"],
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
        ["Insuficiente", "Insufficient"],
        ["Correcta", "Correct"],
        ["Adecuada", "Adequate"],
        ["Excesiva", "Excessive"],
        ["Exceso de compra", "Excess purchase"],
        ["Sobredimensionada aceptable", "Acceptably oversized"],
        ["Sobredimensionada relevante", "Significantly oversized"],
        ["Sobredimensionada", "Oversized"],
        ["Desconocida", "Unknown"],
        ["Sin datos", "No data"],
        ["Adecuación de capacidad", "Capacity suitability"],
        ["Adecuación de potencia", "Power suitability"],
        ["Eficiencia", "Efficiency"],
        ["Precio relativo", "Relative price"],
        ["Garantía", "Warranty"],
        ["Factor", "Factor"],
        ["Enlace para compartir", "Share link"],
        ["Enlace para compartir este resultado", "Share link for this result"],
        ["Copiar enlace", "Copy link"],

        ["Compará dos pinturas", "Compare two paints"],
        ["Comparar pinturas", "Compare paints"],
        ["Pintura A", "Paint A"],
        ["Pintura B", "Paint B"],
        ["Litros por envase", "Liters per container"],
        ["Precio por envase", "Price per container"],
        ["Rendimiento por litro y por mano", "Coverage per liter per coat"],
        ["Lavabilidad", "Washability"],
        ["Secado en horas", "Drying time in hours"],
        ["Aprovechamiento", "Utilization"],
        ["Costo total", "Total cost"],
        ["Secado", "Drying"],
        ["Resultado ElegíBien: Pintura", "ElegíBien result: Paint"],
        ["Resultado orientativo. Revisá la ficha técnica y prepará correctamente la superficie.", "Estimated result. Check the technical data sheet and prepare the surface correctly."],
        ["Calcular otra superficie", "Calculate another area"],

        ["Comparar cerámicos y pisos", "Compare flooring and tiles"],
        ["Necesitás cubrir aproximadamente", "You need to cover approximately"],
        ["Producto A", "Product A"],
        ["Producto B", "Product B"],
        ["Comparar productos", "Compare products"],
        ["Cobertura por caja", "Coverage per box"],
        ["Precio por caja", "Price per box"],
        ["Resistencia de uso", "Use resistance"],
        ["Facilidad de reposición", "Replacement availability"],
        ["Uso liviano", "Light use"],
        ["Uso medio", "Medium use"],
        ["Uso intenso", "Heavy use"],
        ["Uso muy intenso", "Very heavy use"],
        ["Excedente estimado:", "Estimated excess:"],
        ["Cobertura real", "Actual coverage"],
        ["Desperdicio estimado", "Estimated waste"],
        ["Material excedente", "Excess material"],
        ["Resultado ElegíBien: Cerámicos y pisos", "ElegíBien result: Flooring and tiles"],
        ["Superficie original:", "Original area:"],
        ["Material adicional considerado:", "Extra material considered:"],
        ["Resultado orientativo. Verificá la cobertura informada en la caja, el lote disponible y las recomendaciones del fabricante o colocador.", "Estimated result. Check the coverage stated on the box, the available batch, and the manufacturer's or installer's recommendations."],

        ["Compará dos equipos de calefacción", "Compare two heating units"],
        ["Rango recomendado:", "Recommended range:"],
        ["Primer equipo", "First unit"],
        ["Segundo equipo", "Second unit"],
        ["Nombre del equipo", "Unit name"],
        ["Tipo de sistema", "System type"],
        ["Calefactor a gas de tiro balanceado", "Balanced-flue gas heater"],
        ["Estufa eléctrica", "Electric heater"],
        ["Panel eléctrico", "Electric panel heater"],
        ["Caloventor", "Fan heater"],
        ["Aire acondicionado frío/calor", "Heat-pump air conditioner"],
        ["Potencia térmica", "Heating capacity"],
        ["Precio del equipo", "Unit price"],
        ["Costo estimado por hora", "Estimated hourly cost"],
        ["Instalación y seguridad", "Installation and safety"],
        ["Requiere instalación profesional", "Requires professional installation"],
        ["Requiere revisión eléctrica dedicada", "Requires dedicated electrical review"],
        ["Instalación estándar", "Standard installation"],
        ["Instalación simple", "Simple installation"],
        ["Empate técnico", "Technical tie"],
        ["Adecuación de potencia", "Power suitability"],
        ["Costo estimado de uso", "Estimated operating cost"],
        ["Seguridad e instalación", "Safety and installation"],
        ["Resultado ElegíBien: Calefacción", "ElegíBien result: Heating"],
        ["Ambiente:", "Room:"],
        ["Potencia ideal:", "Ideal power:"],
        ["Resultado orientativo. La instalación de gas o una instalación eléctrica dedicada debe ser revisada por un profesional habilitado.", "Estimated result. Gas installation or a dedicated electrical installation must be reviewed by a qualified professional."],
        ["Calcular otro ambiente", "Calculate another room"],

        ["Capacidad recomendada:", "Recommended capacity:"],
        ["Estado de capacidad:", "Capacity status:"],
        ["Analizar otra opción", "Analyze another option"],
        ["Resultado compartido", "Shared result"],
        ["Resultado compartido de pintura", "Shared paint result"],
        ["Resultado compartido de cerámicos y pisos", "Shared flooring and tiles result"],
        ["Resultado compartido de calefacción", "Shared heating result"],

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

        ["Ocurrió un problema", "Something went wrong"],
        ["No pudimos completar la operación. Podés volver al inicio e intentarlo nuevamente.", "We could not complete the operation. You can return home and try again."],
        ["Identificador de solicitud:", "Request ID:"],
        ["Volver al inicio", "Back to home"]
    ];

    const toEnglish = new Map(pairs);
    const toSpanish = new Map(pairs.map(([es, en]) => [en, es]));
    const state = new WeakMap();
    const attributeState = new WeakMap();
    let applying = false;

    const titlePairs = new Map([
        ["ElegíBien - ElegíBien", "ElegíBien"],
        ["Qué aire acondicionado necesito - ElegíBien", "What air conditioner do I need? - ElegíBien"],
        ["Cuánta pintura necesito - ElegíBien", "How much paint do I need? - ElegíBien"],
        ["Cuántos cerámicos o pisos necesito - ElegíBien", "How much flooring or tile do I need? - ElegíBien"],
        ["Qué potencia de calefacción necesito - ElegíBien", "How much heating power do I need? - ElegíBien"],
        ["Comparar aires acondicionados - ElegíBien", "Compare air conditioners - ElegíBien"],
        ["Comparar pinturas - ElegíBien", "Compare paints - ElegíBien"],
        ["Comparar cerámicos y pisos - ElegíBien", "Compare flooring and tiles - ElegíBien"],
        ["Comparar equipos de calefacción - ElegíBien", "Compare heating units - ElegíBien"],
        ["Contacto - ElegíBien", "Contact - ElegíBien"],
        ["Política de privacidad - ElegíBien", "Privacy Policy - ElegíBien"],
        ["Términos de uso - ElegíBien", "Terms of Use - ElegíBien"],
        ["Metodología - ElegíBien", "Methodology - ElegíBien"],
        ["Resultado compartido - ElegíBien", "Shared result - ElegíBien"],
        ["Resultado compartido de pintura - ElegíBien", "Shared paint result - ElegíBien"],
        ["Resultado compartido de cerámicos y pisos - ElegíBien", "Shared flooring and tiles result - ElegíBien"],
        ["Resultado compartido de calefacción - ElegíBien", "Shared heating result - ElegíBien"],
        ["Ocurrió un problema - ElegíBien", "Something went wrong - ElegíBien"]
    ]);

    const descriptionPairs = new Map([
        ["ElegíBien ayuda a calcular qué producto necesitás y comparar alternativas mediante criterios claros y explicables.", "ElegíBien helps you estimate what you need and compare alternatives using clear, explainable criteria."],
        ["Calculá cuántas frigorías necesitás para tu ambiente y compará equipos por capacidad, eficiencia, precio y garantía.", "Estimate the cooling capacity required for your room and compare units by capacity, efficiency, price, and warranty."],
        ["Calculá cuántos litros de pintura necesitás según la superficie, las manos y el estado de las paredes, y compará alternativas.", "Estimate how much paint you need based on area, coats, and wall condition, and compare alternatives."],
        ["Calculá cuántos cerámicos o pisos necesitás según la superficie, el tipo de colocación y el material adicional recomendado.", "Estimate how much flooring or tile you need based on area, installation pattern, and recommended extra material."],
        ["Calculá una potencia orientativa de calefacción según el tamaño, el clima y las características del ambiente.", "Estimate heating power based on room size, climate, and room characteristics."],
        ["Compará dos opciones de cerámicos o pisos por cajas necesarias, costo total, excedente, resistencia y facilidad de reposición.", "Compare two flooring or tile options by required boxes, total cost, excess material, durability, and replacement availability."]
    ]);

    const compact = value => (value ?? "").replace(/\s+/g, " ").trim();
    const getLanguage = () => localStorage.getItem(LANGUAGE_KEY) === "en" ? "en" : "es";

    function dynamicEnglish(source) {
        const value = compact(source);
        let match;

        match = /^ElegíBien recomienda (.+) porque presenta la mejor adecuación general entre las alternativas analizadas\.$/.exec(value);
        if (match) return `ElegíBien recommends ${match[1]} because it provides the best overall fit among the alternatives analyzed.`;

        match = /^ElegíBien recomienda (.+) porque ofrece la mejor relación entre cobertura necesaria, costo total, lavabilidad y tiempo de secado\.$/.exec(value);
        if (match) return `ElegíBien recommends ${match[1]} because it offers the best balance of required coverage, total cost, washability, and drying time.`;

        match = /^ElegíBien recomienda (.+) porque ofrece la mejor relación entre cobertura comprada, costo total, excedente, resistencia y facilidad de reposición\.$/.exec(value);
        if (match) return `ElegíBien recommends ${match[1]} because it offers the best balance of purchased coverage, total cost, excess material, durability, and replacement availability.`;

        match = /^Recomendación: (.+)$/.exec(value);
        if (match) return `Recommendation: ${match[1]}`;

        match = /^Garantía informada: (\d+) meses\.$/.exec(value);
        if (match) return `Reported warranty: ${match[1]} months.`;

        match = /^Se necesitan ([\d.,]+) litros y se comprarían ([\d.,]+) litros\.$/.exec(value);
        if (match) return `${match[1]} liters are required and ${match[2]} liters would be purchased.`;

        match = /^Costo total estimado: \$([\d.,]+)\.$/.exec(value);
        if (match) return `Estimated total cost: $${match[1]}.`;

        match = /^Lavabilidad informada: (.+)\.$/.exec(value);
        if (match) return `Reported washability: ${toEnglish.get(match[1]) ?? match[1]}.`;

        match = /^Secado informado: ([\d.,]+) horas\.$/.exec(value);
        if (match) return `Reported drying time: ${match[1]} hours.`;

        if (value === "No se informó el tiempo de secado.") return "Drying time was not provided.";

        match = /^Se necesitan ([\d.,]+) m² y se comprarían ([\d.,]+) m²\.$/.exec(value);
        if (match) return `${match[1]} m² are required and ${match[2]} m² would be purchased.`;

        match = /^Excedente por cajas completas: ([\d.,]+) m² \(([\d.,]+) %\)\.$/.exec(value);
        if (match) return `Excess from full boxes: ${match[1]} m² (${match[2]}%).`;

        match = /^Resistencia de uso informada: (.+)\.$/.exec(value);
        if (match) return `Reported use resistance: ${toEnglish.get(match[1]) ?? match[1]}.`;

        match = /^Facilidad de reposición informada: (.+)\.$/.exec(value);
        if (match) return `Reported replacement availability: ${toEnglish.get(match[1]) ?? match[1]}.`;

        match = /^Nivel de eficiencia informado: (.+)\.$/.exec(value);
        if (match) return `Reported efficiency level: ${match[1]}.`;

        match = /^Condición de instalación informada: (.+)\.$/.exec(value);
        if (match) return `Reported installation condition: ${match[1]}.`;

        const exactDynamic = new Map([
            ["Las dos alternativas presentan un empate técnico. Revisá precio, consumo y garantía según tus prioridades.", "The two alternatives are technically tied. Review price, consumption, and warranty according to your priorities."],
            ["Ninguna de las alternativas analizadas se adapta correctamente a la capacidad necesaria.", "None of the analyzed alternatives correctly matches the required capacity."],
            ["Las dos pinturas presentan un empate técnico. Revisá terminación, disponibilidad y preferencia de marca.", "The two paints are technically tied. Review finish, availability, and brand preference."],
            ["Los dos productos presentan un empate técnico. Revisá disponibilidad, terminación y posibilidad de conseguir cajas del mismo lote en el futuro.", "The two products are technically tied. Review availability, finish, and the possibility of obtaining boxes from the same batch in the future."],
            ["Se comparó el consumo nominal por cada 1.000 frigorías.", "Nominal consumption was compared per 1,000 cooling units."],
            ["El consumo exacto no estaba disponible; se utilizó la tecnología como aproximación.", "Exact consumption was not available; technology was used as an approximation."],
            ["El precio se comparó únicamente entre alternativas técnicamente elegibles.", "Price was compared only among technically eligible alternatives."],
            ["No recibió puntos de precio porque la capacidad no es elegible.", "No price points were awarded because the capacity is not eligible."],
            ["La capacidad está dentro del rango recomendado.", "Capacity is within the recommended range."],
            ["La capacidad está hasta un 10 % por debajo del mínimo recomendado.", "Capacity is up to 10% below the recommended minimum."],
            ["La capacidad está entre un 10 % y un 20 % por debajo del mínimo recomendado.", "Capacity is between 10% and 20% below the recommended minimum."],
            ["La capacidad está más de un 20 % por debajo del mínimo recomendado.", "Capacity is more than 20% below the recommended minimum."],
            ["La capacidad supera hasta un 10 % el máximo recomendado.", "Capacity is up to 10% above the recommended maximum."],
            ["La capacidad supera entre un 10 % y un 20 % el máximo recomendado.", "Capacity is between 10% and 20% above the recommended maximum."],
            ["La capacidad supera más de un 20 % el máximo recomendado.", "Capacity is more than 20% above the recommended maximum."],
            ["El costo por hora se comparó entre alternativas con capacidad suficiente.", "Hourly cost was compared among alternatives with sufficient capacity."],
            ["No recibió puntos porque la capacidad informada no resulta elegible.", "No points were awarded because the reported capacity is not eligible."],
            ["El precio se comparó entre alternativas con capacidad suficiente.", "Price was compared among alternatives with sufficient capacity."],
            ["La potencia está dentro del rango recomendado.", "Power is within the recommended range."],
            ["La potencia está hasta un 10 % por debajo del mínimo recomendado.", "Power is up to 10% below the recommended minimum."],
            ["La potencia está entre un 10 % y un 20 % por debajo del mínimo recomendado.", "Power is between 10% and 20% below the recommended minimum."],
            ["La potencia está más de un 20 % por debajo del mínimo recomendado.", "Power is more than 20% below the recommended minimum."],
            ["La potencia supera hasta un 10 % el máximo recomendado.", "Power is up to 10% above the recommended maximum."],
            ["La potencia supera entre un 10 % y un 20 % el máximo recomendado.", "Power is between 10% and 20% above the recommended maximum."],
            ["La potencia supera más de un 20 % el máximo recomendado.", "Power is more than 20% above the recommended maximum."]
        ]);

        const exact = exactDynamic.get(value);
        if (exact) return exact;

        if (/\benvases\b|\bcajas\b|\blitros\b|\bcomprados\b/.test(value)) {
            return value
                .replace(/\benvases\b/g, "containers")
                .replace(/\bcajas\b/g, "boxes")
                .replace(/\blitros\b/g, "liters")
                .replace(/\bcomprados\b/g, "purchased");
        }

        return null;
    }

    function canonicalSpanish(value) {
        const normalized = compact(value);
        return toSpanish.get(normalized) ?? normalized;
    }

    function renderTextNode(node, language) {
        const parent = node.parentElement;
        if (!parent || ["SCRIPT", "STYLE", "CODE", "PRE", "TEXTAREA"].includes(parent.tagName)) return;

        let item = state.get(node);
        const current = node.nodeValue ?? "";

        if (!item || current !== item.rendered) {
            const leading = current.match(/^\s*/)?.[0] ?? "";
            const trailing = current.match(/\s*$/)?.[0] ?? "";
            const normalized = compact(current);
            const source = canonicalSpanish(normalized);
            item = { source, leading, trailing, rendered: current };
            state.set(node, item);
        }

        let renderedCore = item.source;
        if (language === "en") {
            renderedCore = toEnglish.get(item.source) ?? dynamicEnglish(item.source) ?? item.source;
        }

        const rendered = `${item.leading}${renderedCore}${item.trailing}`;
        if (node.nodeValue !== rendered) node.nodeValue = rendered;
        item.rendered = rendered;
    }

    function renderAttributes(language) {
        const attributes = ["placeholder", "title", "aria-label", "alt"];
        document.querySelectorAll("[placeholder], [title], [aria-label], [alt]").forEach(element => {
            let originals = attributeState.get(element);
            if (!originals) {
                originals = {};
                attributeState.set(element, originals);
            }

            for (const attribute of attributes) {
                if (!element.hasAttribute(attribute)) continue;
                if (!(attribute in originals)) {
                    originals[attribute] = canonicalSpanish(element.getAttribute(attribute) ?? "");
                }
                const source = originals[attribute];
                const rendered = language === "en" ? (toEnglish.get(source) ?? source) : source;
                if (element.getAttribute(attribute) !== rendered) element.setAttribute(attribute, rendered);
            }
        });
    }

    function renderHead(language) {
        const html = document.documentElement;
        if (!html.dataset.presentationOriginalTitle) {
            html.dataset.presentationOriginalTitle = document.title;
        }
        const originalTitle = html.dataset.presentationOriginalTitle;
        document.title = language === "en" ? (titlePairs.get(originalTitle) ?? originalTitle) : originalTitle;

        const meta = document.querySelector('meta[name="description"]');
        if (meta) {
            if (!meta.dataset.presentationOriginalDescription) {
                meta.dataset.presentationOriginalDescription = meta.content;
            }
            const original = meta.dataset.presentationOriginalDescription;
            meta.content = language === "en" ? (descriptionPairs.get(original) ?? original) : original;
        }
    }

    function applyLanguage() {
        if (applying || !document.body) return;
        applying = true;
        try {
            const language = getLanguage();
            document.documentElement.lang = language === "en" ? "en" : "es-AR";

            const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
            const nodes = [];
            while (walker.nextNode()) nodes.push(walker.currentNode);
            nodes.forEach(node => renderTextNode(node, language));

            renderAttributes(language);
            renderHead(language);

            document.querySelectorAll("[data-pref-language]").forEach(button => {
                const active = button.dataset.prefLanguage === language;
                button.classList.toggle("active", active);
                button.setAttribute("aria-pressed", active ? "true" : "false");
            });
        }
        finally {
            applying = false;
        }
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.addEventListener("click", () => {
                localStorage.setItem(LANGUAGE_KEY, button.dataset.prefLanguage === "en" ? "en" : "es");
                window.setTimeout(applyLanguage, 0);
            });
        });

        applyLanguage();

        const observer = new MutationObserver(() => {
            if (!applying) window.requestAnimationFrame(applyLanguage);
        });
        observer.observe(document.body, { subtree: true, childList: true, characterData: true });
    });
})();

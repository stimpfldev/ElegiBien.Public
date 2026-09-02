"use strict";

(() => {
    const LANGUAGE_KEY = "elegibien.language";
    const originals = new WeakMap();

    const exactTranslations = new Map([
        ["Necesitás aproximadamente entre", "You need approximately between"],
        ["y", "and"],
        ["frigorías.", "frigories."],
        ["frigorías", "frigories"],
        ["para", "for"],
        ["manos.", "coats."],
        ["El cálculo utiliza las dimensiones del ambiente, una altura estándar de 2,60 metros, la cantidad de personas y la exposición solar.", "The calculation uses the room dimensions, a standard height of 2.60 meters, the number of people, and sun exposure."],
        ["El cálculo descuenta puertas y ventanas estándar, aplica el estado de la superficie, las manos elegidas y un margen de desperdicio del 10 %.", "The calculation deducts standard doors and windows and applies the surface condition, selected coats, and a 10% waste allowance."],
        ["La estimación calcula la superficie rectangular y agrega el porcentaje de desperdicio elegido según el tipo de colocación.", "The estimate calculates the rectangular area and adds the selected waste percentage according to the installation pattern."],
        ["Necesitás", "You need"],
        ["aproximadamente", "approximately"],
        ["Cantidad necesaria", "Required quantity"],
        ["Cantidad de cajas", "Number of boxes"],
        ["Costo total", "Total cost"],
        ["Costo estimado", "Estimated cost"],
        ["Producto recomendado", "Recommended product"],
        ["Equipo recomendado", "Recommended unit"],
        ["Pintura recomendada", "Recommended paint"],
        ["Mejor opción", "Best option"],
        ["Opción recomendada", "Recommended option"],
        ["Puntaje", "Score"],
        ["Capacidad", "Capacity"],
        ["Eficiencia", "Efficiency"],
        ["Consumo", "Consumption"],
        ["Instalación", "Installation"],
        ["Cobertura", "Coverage"],
        ["Resistencia", "Durability"],
        ["Lavabilidad", "Washability"],
        ["Secado", "Drying"],
        ["Envases necesarios", "Containers required"],
        ["Cajas necesarias", "Boxes required"],
        ["Compartir resultado", "Share result"],
        ["Copiar enlace", "Copy link"],
        ["Resultado compartido", "Shared result"],
        ["Enlace vencido", "Expired link"],
        ["Enlace inválido", "Invalid link"]
    ]);

    const titleTranslations = new Map([
        ["ElegíBien - ElegíBien", "ElegíBien"],
        ["Qué aire acondicionado necesito - ElegíBien", "What air conditioner do I need? - ElegíBien"],
        ["Cuánta pintura necesito - ElegíBien", "How much paint do I need? - ElegíBien"],
        ["Cuántos cerámicos o pisos necesito - ElegíBien", "How much flooring or tile do I need? - ElegíBien"],
        ["Qué potencia de calefacción necesito - ElegíBien", "How much heating power do I need? - ElegíBien"],
        ["Contacto - ElegíBien", "Contact - ElegíBien"],
        ["Política de privacidad - ElegíBien", "Privacy Policy - ElegíBien"],
        ["Términos de uso - ElegíBien", "Terms of Use - ElegíBien"],
        ["Metodología - ElegíBien", "Methodology - ElegíBien"]
    ]);

    const descriptionTranslations = new Map([
        ["Calculá cuántas frigorías necesitás para tu ambiente y compará equipos por capacidad, eficiencia, precio y garantía.", "Estimate the cooling capacity required for your room and compare units by capacity, efficiency, price, and warranty."],
        ["Calculá cuántos litros de pintura necesitás según la superficie, las manos y el estado de las paredes, y compará alternativas.", "Estimate how much paint you need based on area, coats, and wall condition, and compare alternatives."],
        ["Calculá cuántos cerámicos o pisos necesitás según la superficie, el tipo de colocación y el material adicional recomendado.", "Estimate how much flooring or tile you need based on area, installation pattern, and recommended extra material."],
        ["Calculá una potencia orientativa de calefacción según el tamaño, el clima y las características del ambiente.", "Estimate heating power based on room size, climate, and room characteristics."],
        ["ElegíBien ayuda a calcular qué producto necesitás y comparar alternativas mediante criterios claros y explicables.", "ElegíBien helps you estimate what you need and compare alternatives using clear, explainable criteria."]
    ]);

    const compact = value => (value ?? "").replace(/\s+/g, " ").trim();

    function translateRemainingText(language) {
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

            const normalized = compact(source);
            const translated = exactTranslations.get(normalized);
            if (!translated) continue;

            const leading = source.match(/^\s*/)?.[0] ?? "";
            const trailing = source.match(/\s*$/)?.[0] ?? "";
            node.nodeValue = `${leading}${translated}${trailing}`;
        }
    }

    function translateHead(language) {
        if (!document.documentElement.dataset.originalTitle) {
            document.documentElement.dataset.originalTitle = document.title;
        }

        const originalTitle = document.documentElement.dataset.originalTitle;
        document.title = language === "en"
            ? (titleTranslations.get(originalTitle) ?? originalTitle)
            : originalTitle;

        const meta = document.querySelector('meta[name="description"]');
        if (!meta) return;

        if (!meta.dataset.originalDescription) {
            meta.dataset.originalDescription = meta.content;
        }

        const originalDescription = meta.dataset.originalDescription;
        meta.content = language === "en"
            ? (descriptionTranslations.get(originalDescription) ?? originalDescription)
            : originalDescription;
    }

    function apply() {
        const language = localStorage.getItem(LANGUAGE_KEY) === "en" ? "en" : "es";
        translateRemainingText(language);
        translateHead(language);
    }

    document.addEventListener("DOMContentLoaded", () => {
        apply();

        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.addEventListener("click", () => window.setTimeout(apply, 0));
        });
    });
})();

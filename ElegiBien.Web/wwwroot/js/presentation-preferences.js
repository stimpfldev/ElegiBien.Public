"use strict";

(() => {
    const LANGUAGE_KEY = "elegibien.language";
    const UNITS_KEY = "elegibien.units";

    const translations = new Map([
        ["Aire acondicionado", "Air conditioning"],
        ["Pintura", "Paint"],
        ["Cerámicos y pisos", "Flooring and tiles"],
        ["Calefacción", "Heating"],
        ["Contacto", "Contact"],
        ["Cómo decidimos", "How we decide"],
        ["Privacidad", "Privacy"],
        ["Términos", "Terms"],
        ["Metodología", "Methodology"],
        ["Decisiones de compra más claras, con cálculos y criterios explicables.", "Clearer purchase decisions with transparent calculations and criteria."],
        ["Hecho en Argentina", "Made in Argentina"],
        ["Abrir navegación", "Open navigation"],

        ["Qué aire acondicionado necesitás", "What air conditioner do you need?"],
        ["Ingresá cuatro datos y obtené un rango orientativo de frigorías para tu ambiente.", "Enter four values to get an estimated cooling-capacity range for your room."],
        ["Baja", "Low"], ["Media", "Medium"], ["Alta", "High"],
        ["Permitir estadísticas anónimas para mejorar ElegíBien.", "Allow anonymous statistics to improve ElegíBien."],
        ["Permitir que los datos anónimos de esta consulta se incorporen en futuras estadísticas del Radar ElegíBien.", "Allow anonymous data from this query to be included in future ElegíBien Radar statistics."],
        ["No solicitamos nombre, documento, teléfono, domicilio ni información bancaria.", "We do not request your name, ID, phone number, address, or banking information."],
        ["Calcular frigorías", "Calculate cooling capacity"],
        ["Resultado orientativo", "Estimated result"],
        ["Capacidad ideal aproximada:", "Approximate ideal capacity:"],
        ["Superficie:", "Area:"],
        ["Nivel de confianza:", "Confidence level:"],
        ["Comparar dos equipos", "Compare two units"],
        ["Este ambiente presenta condiciones que requieren revisión profesional antes de comprar o instalar el equipo.", "This room has conditions that should be reviewed by a professional before purchasing or installing the unit."],
        ["El resultado es orientativo y no reemplaza la evaluación de un técnico matriculado.", "This result is an estimate and does not replace an assessment by a qualified technician."],

        ["Cuánta pintura necesitás", "How much paint do you need?"],
        ["Calculá la superficie y una cantidad orientativa de litros.", "Calculate the area and an estimated amount of paint."],
        ["Buena", "Good"], ["Nueva o porosa", "New or porous"], ["Dañada", "Damaged"],
        ["Permitir incorporar esta consulta anónima al futuro Radar ElegíBien.", "Allow this anonymous query to be included in the future ElegíBien Radar."],
        ["Calcular pintura", "Calculate paint"],
        ["Superficie neta:", "Net area:"],
        ["Superficie ajustada:", "Adjusted area:"],
        ["Referencia:", "Reference:"],
        ["La superficie requiere revisión o preparación profesional antes de pintar.", "The surface requires professional review or preparation before painting."],
        ["El resultado es orientativo. Revisá el rendimiento indicado por el fabricante.", "This result is an estimate. Check the coverage specified by the manufacturer."],
        ["Comparar dos pinturas", "Compare two paints"],

        ["Cuántos cerámicos o pisos necesitás", "How much flooring or tile do you need?"],
        ["Calculá la superficie y el material adicional recomendado para cortes, roturas y futuras reposiciones.", "Calculate the area and the recommended extra material for cuts, breakage, and future replacements."],
        ["Recta", "Straight"], ["Trabada", "Staggered"], ["Diagonal", "Diagonal"],
        ["Material adicional calculado", "Calculated extra material"],
        ["para cortes, roturas y futuras reposiciones.", "for cuts, breakage, and future replacements."],
        ["porque la colocación trabada suele requerir más cortes.", "because staggered installation usually requires more cuts."],
        ["porque la colocación diagonal requiere más cortes.", "because diagonal installation requires more cuts."],
        ["Permitir analítica anónima para mejorar ElegíBien.", "Allow anonymous analytics to improve ElegíBien."],
        ["Permitir el uso anónimo de este resultado en estadísticas agregadas.", "Allow anonymous use of this result in aggregated statistics."],
        ["Calcular superficie", "Calculate area"],
        ["Superficie del piso", "Floor area"],
        ["Material adicional aplicado", "Extra material applied"],
        ["Superficie adicional", "Extra area"],
        ["Superficie total necesaria", "Total required area"],
        ["Conviene revisar medidas, cortes y disposición antes de comprar.", "Review measurements, cuts, and layout before purchasing."],
        ["Comparar dos productos", "Compare two products"],

        ["Qué potencia de calefacción necesitás", "How much heating power do you need?"],
        ["Estimá la potencia necesaria para calefaccionar el ambiente sin elegir un equipo demasiado chico o excesivo.", "Estimate the power required to heat the room without choosing a unit that is too small or unnecessarily large."],
        ["Templada", "Mild"], ["Templada fría", "Cool temperate"], ["Fría", "Cold"], ["Muy fría", "Very cold"],
        ["Bueno", "Good"], ["Normal", "Normal"], ["Deficiente", "Poor"],
        ["Ninguna", "None"], ["Una", "One"], ["Dos", "Two"], ["Tres", "Three"], ["Cuatro", "Four"],
        ["Pocas o normales", "Few or average"], ["Varias o grandes", "Several or large"], ["Ventanal amplio", "Large glazing"],
        ["Calcular potencia", "Calculate heating power"],
        ["Superficie del ambiente", "Room area"],
        ["Volumen calculado", "Calculated volume"],
        ["Potencia mínima recomendada", "Recommended minimum power"],
        ["Potencia máxima recomendada", "Recommended maximum power"],
        ["Potencia ideal orientativa", "Estimated ideal power"],
        ["Equivalencia aproximada", "Approximate equivalent"],
        ["Por las características del ambiente, conviene validar la instalación y la potencia con un profesional.", "Because of the room characteristics, the installation and required power should be validated by a professional."],

        ["Largo", "Length"], ["Ancho", "Width"], ["Alto", "Height"],
        ["Personas", "People"], ["Exposición solar", "Sun exposure"],
        ["Cantidad de puertas", "Number of doors"], ["Cantidad de ventanas", "Number of windows"],
        ["Cantidad de manos", "Number of coats"], ["Estado de la superficie", "Surface condition"],
        ["Incluir cielorraso", "Include ceiling"], ["Tipo de colocación", "Installation pattern"],
        ["Zona climática", "Climate zone"], ["Nivel de aislación", "Insulation level"],
        ["Paredes exteriores", "Exterior walls"], ["Ventanas", "Windows"],
        ["Abierto a otro ambiente", "Open to another space"],
        ["Permitir analítica anónima", "Allow anonymous analytics"],
        ["Permitir datos para Radar", "Allow Radar data"],

        ["Copiado", "Copied"], ["Copiar", "Copy"], ["Volver", "Back"], ["Inicio", "Home"],
        ["Guardar", "Save"], ["Comparar", "Compare"], ["Precio", "Price"],
        ["Nombre", "Name"], ["Marca", "Brand"], ["Garantía", "Warranty"],
        ["Resultado", "Result"], ["Recomendación", "Recommendation"]
    ]);

    const originalText = new WeakMap();
    const originalAttributes = new WeakMap();
    const managedInputs = [];

    const getLanguage = () => localStorage.getItem(LANGUAGE_KEY) === "en" ? "en" : "es";
    const getUnits = () => localStorage.getItem(UNITS_KEY) === "imperial" ? "imperial" : "metric";

    const compact = value => value.replace(/\s+/g, " ").trim();

    function translateTextNode(node, language) {
        if (!originalText.has(node)) originalText.set(node, node.nodeValue);
        const source = originalText.get(node);
        if (language === "es") {
            node.nodeValue = source;
            return;
        }

        const normalized = compact(source);
        if (!normalized) return;
        const translated = translations.get(normalized);
        if (!translated) return;

        const leading = source.match(/^\s*/)?.[0] ?? "";
        const trailing = source.match(/\s*$/)?.[0] ?? "";
        node.nodeValue = `${leading}${translated}${trailing}`;
    }

    function translateElementAttributes(element, language) {
        const attributes = ["placeholder", "title", "aria-label"];
        let originals = originalAttributes.get(element);
        if (!originals) {
            originals = {};
            originalAttributes.set(element, originals);
        }

        for (const attribute of attributes) {
            if (!element.hasAttribute(attribute)) continue;
            if (!(attribute in originals)) originals[attribute] = element.getAttribute(attribute);
            const source = originals[attribute] ?? "";
            element.setAttribute(attribute, language === "en" ? (translations.get(compact(source)) ?? source) : source);
        }
    }

    function applyLanguage(language) {
        document.documentElement.lang = language === "en" ? "en" : "es-AR";

        const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
        const nodes = [];
        while (walker.nextNode()) nodes.push(walker.currentNode);

        for (const node of nodes) {
            const parent = node.parentElement;
            if (!parent || ["SCRIPT", "STYLE", "CODE", "PRE"].includes(parent.tagName)) continue;
            translateTextNode(node, language);
        }

        document.querySelectorAll("[placeholder], [title], [aria-label]")
            .forEach(element => translateElementAttributes(element, language));

        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.classList.toggle("active", button.dataset.prefLanguage === language);
            button.setAttribute("aria-pressed", button.dataset.prefLanguage === language ? "true" : "false");
        });
    }

    const converters = {
        length: {
            toImperial: value => value * 3.280839895,
            toMetric: value => value / 3.280839895,
            metricUnit: "m",
            imperialUnit: "ft"
        },
        area: {
            toImperial: value => value * 10.763910417,
            toMetric: value => value / 10.763910417,
            metricUnit: "m²",
            imperialUnit: "ft²"
        },
        volume: {
            toImperial: value => value * 35.314666721,
            toMetric: value => value / 35.314666721,
            metricUnit: "m³",
            imperialUnit: "ft³"
        },
        liters: {
            toImperial: value => value * 0.264172052,
            toMetric: value => value / 0.264172052,
            metricUnit: "L",
            imperialUnit: "US gal"
        },
        power: {
            toImperial: value => value * 3.412141633,
            toMetric: value => value / 3.412141633,
            metricUnit: "W",
            imperialUnit: "BTU/h"
        },
        temperature: {
            toImperial: value => (value * 9 / 5) + 32,
            toMetric: value => (value - 32) * 5 / 9,
            metricUnit: "°C",
            imperialUnit: "°F"
        }
    };

    function inputKind(input) {
        const name = input.name ?? "";
        if (/(LengthMeters|WidthMeters|HeightMeters)$/i.test(name)) return "length";
        if (/(AreaSquareMeters|SquareMeters)$/i.test(name)) return "area";
        if (/Liters$/i.test(name)) return "liters";
        if (/Watts$/i.test(name)) return "power";
        if (/(Celsius|TemperatureCelsius)$/i.test(name)) return "temperature";
        return null;
    }

    function prepareManagedInputs() {
        document.querySelectorAll('input[type="number"], input:not([type])').forEach(input => {
            const kind = inputKind(input);
            if (!kind || managedInputs.some(item => item.input === input)) return;
            managedInputs.push({ input, kind, metricStep: input.step });
        });
    }

    function convertInput(item, targetUnits) {
        const { input, kind, metricStep } = item;
        const converter = converters[kind];
        const currentSystem = input.dataset.presentationUnits ?? "metric";
        if (currentSystem === targetUnits) return;

        const value = Number.parseFloat(input.value);
        if (Number.isFinite(value)) {
            const converted = targetUnits === "imperial" ? converter.toImperial(value) : converter.toMetric(value);
            input.value = Number(converted.toFixed(kind === "power" ? 0 : 2)).toString();
        }

        input.dataset.presentationUnits = targetUnits;
        input.step = targetUnits === "imperial" && kind !== "power" ? "0.01" : metricStep;

        const suffix = input.closest(".input-group")?.querySelector(".input-group-text");
        if (suffix && [converter.metricUnit, converter.imperialUnit].includes(compact(suffix.textContent))) {
            suffix.textContent = targetUnits === "imperial" ? converter.imperialUnit : converter.metricUnit;
        }
    }

    function restoreMetricInputsBeforeSubmit(form) {
        for (const item of managedInputs) {
            if (item.input.form !== form || item.input.dataset.presentationUnits !== "imperial") continue;
            const value = Number.parseFloat(item.input.value);
            if (Number.isFinite(value)) {
                item.input.value = Number(converters[item.kind].toMetric(value).toFixed(4)).toString();
            }
            item.input.dataset.presentationUnits = "metric";
        }
    }

    function convertRenderedMeasurements(targetUnits) {
        document.querySelectorAll("[data-presentation-converted]").forEach(element => {
            element.textContent = element.dataset.presentationOriginal;
            element.removeAttribute("data-presentation-converted");
        });

        if (targetUnits !== "imperial") return;

        const patterns = [
            { regex: /(-?[\d.,]+)\s*m²\b/g, kind: "area" },
            { regex: /(-?[\d.,]+)\s*m³\b/g, kind: "volume" },
            { regex: /(-?[\d.,]+)\s+litros?\b/gi, kind: "liters" },
            { regex: /(-?[\d.,]+)\s+liters?\b/gi, kind: "liters" },
            { regex: /(-?[\d.,]+)\s*W\b/g, kind: "power" }
        ];

        const parseDisplayNumber = text => {
            const lastComma = text.lastIndexOf(",");
            const lastDot = text.lastIndexOf(".");
            let normalized = text;
            if (lastComma > lastDot) normalized = text.replace(/\./g, "").replace(",", ".");
            else if (lastDot > lastComma && lastComma >= 0) normalized = text.replace(/,/g, "");
            else if (lastComma >= 0) normalized = text.replace(",", ".");
            return Number.parseFloat(normalized);
        };

        const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
        const nodes = [];
        while (walker.nextNode()) nodes.push(walker.currentNode);

        for (const node of nodes) {
            const parent = node.parentElement;
            if (!parent || ["SCRIPT", "STYLE", "CODE", "PRE", "INPUT", "OPTION"].includes(parent.tagName)) continue;
            let text = node.nodeValue;
            let changed = false;

            for (const pattern of patterns) {
                text = text.replace(pattern.regex, (match, raw) => {
                    const value = parseDisplayNumber(raw);
                    if (!Number.isFinite(value)) return match;
                    const converted = converters[pattern.kind].toImperial(value);
                    changed = true;
                    return `${converted.toFixed(pattern.kind === "power" ? 0 : 2)} ${converters[pattern.kind].imperialUnit}`;
                });
            }

            if (changed) {
                if (!parent.dataset.presentationOriginal) parent.dataset.presentationOriginal = parent.textContent;
                node.nodeValue = text;
                parent.dataset.presentationConverted = "true";
            }
        }
    }

    function applyUnits(units) {
        prepareManagedInputs();
        managedInputs.forEach(item => convertInput(item, units));
        convertRenderedMeasurements(units);

        document.querySelectorAll("[data-pref-units]").forEach(button => {
            button.classList.toggle("active", button.dataset.prefUnits === units);
            button.setAttribute("aria-pressed", button.dataset.prefUnits === units ? "true" : "false");
        });
    }

    function refresh() {
        applyLanguage(getLanguage());
        applyUnits(getUnits());
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.addEventListener("click", () => {
                localStorage.setItem(LANGUAGE_KEY, button.dataset.prefLanguage);
                refresh();
            });
        });

        document.querySelectorAll("[data-pref-units]").forEach(button => {
            button.addEventListener("click", () => {
                const current = getUnits();
                const next = button.dataset.prefUnits;
                if (current === next) return;
                localStorage.setItem(UNITS_KEY, next);
                applyUnits(next);
            });
        });

        document.querySelectorAll("form").forEach(form => {
            form.addEventListener("submit", () => restoreMetricInputsBeforeSubmit(form), true);
        });

        refresh();
    });
})();

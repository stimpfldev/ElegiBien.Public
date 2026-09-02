"use strict";

(() => {
    const LANGUAGE_KEY = "elegibien.language";
    const UNITS_KEY = "elegibien.units";
    const managedInputs = [];
    const renderedNodes = new Map();

    const getLanguage = () => localStorage.getItem(LANGUAGE_KEY) === "en" ? "en" : "es";
    const getUnits = () => localStorage.getItem(UNITS_KEY) === "imperial" ? "imperial" : "metric";
    const compact = value => (value ?? "").replace(/\s+/g, " ").trim();

    const converters = {
        length: {
            toImperial: value => value * 3.28083989501312,
            toMetric: value => value / 3.28083989501312,
            metricUnit: "m",
            imperialUnit: "ft",
            decimals: 2
        },
        area: {
            toImperial: value => value * 10.7639104167097,
            toMetric: value => value / 10.7639104167097,
            metricUnit: "m²",
            imperialUnit: "ft²",
            decimals: 2
        },
        volume: {
            toImperial: value => value * 35.3146667214886,
            toMetric: value => value / 35.3146667214886,
            metricUnit: "m³",
            imperialUnit: "ft³",
            decimals: 2
        },
        liters: {
            toImperial: value => value * 0.264172052358148,
            toMetric: value => value / 0.264172052358148,
            metricUnit: "L",
            imperialUnit: "US gal",
            decimals: 2
        },
        paintCoverage: {
            // 1 m²/L = 10.7639104167 ft² / 0.2641720524 US gal
            toImperial: value => value * 40.7458333932783,
            toMetric: value => value / 40.7458333932783,
            metricUnit: "m²/L",
            imperialUnit: "ft²/US gal",
            decimals: 1
        },
        heatingPower: {
            toImperial: value => value * 3.41214163312794,
            toMetric: value => value / 3.41214163312794,
            metricUnit: "W",
            imperialUnit: "BTU/h",
            decimals: 0
        },
        coolingCapacity: {
            // 1 frigoría/h = 1 kcal/h = 3.968320719 BTU/h
            toImperial: value => value * 3.968320719,
            toMetric: value => value / 3.968320719,
            metricUnit: "frig/h",
            imperialUnit: "BTU/h",
            decimals: 0
        },
        kcalPerHour: {
            toImperial: value => value * 3.968320719,
            toMetric: value => value / 3.968320719,
            metricUnit: "kcal/h",
            imperialUnit: "BTU/h",
            decimals: 0
        },
        temperature: {
            toImperial: value => (value * 9 / 5) + 32,
            toMetric: value => (value - 32) * 5 / 9,
            metricUnit: "°C",
            imperialUnit: "°F",
            decimals: 1
        }
    };

    function inputKind(input) {
        const name = input.name ?? "";
        if (/CoverageSquareMetersPerLiterPerCoat$/i.test(name)) return "paintCoverage";
        if (/CapacityFrigories$/i.test(name)) return "coolingCapacity";
        if (/HeatingCapacityWatts$/i.test(name)) return "heatingPower";
        if (/ContainerLiters$/i.test(name)) return "liters";
        if (/(LengthMeters|WidthMeters|HeightMeters)$/i.test(name)) return "length";
        if (/(AreaSquareMeters|CoverageSquareMetersPerBox|SquareMeters)$/i.test(name)) return "area";
        if (/(Celsius|TemperatureCelsius)$/i.test(name)) return "temperature";
        return null;
    }

    function parseInputNumber(value) {
        if (typeof value !== "string") return Number(value);
        const normalized = value.trim().replace(",", ".");
        return Number.parseFloat(normalized);
    }

    function parseDisplayNumber(text) {
        const value = compact(text).replace(/\s/g, "");
        if (!value) return NaN;

        const hasComma = value.includes(",");
        const hasDot = value.includes(".");
        let normalized = value;

        if (hasComma && hasDot) {
            if (value.lastIndexOf(",") > value.lastIndexOf(".")) {
                normalized = value.replace(/\./g, "").replace(",", ".");
            } else {
                normalized = value.replace(/,/g, "");
            }
        } else if (hasComma) {
            // N0 can render 1,560 in an English culture. Treat groups of 3 as thousands.
            normalized = /^-?\d{1,3}(,\d{3})+$/.test(value)
                ? value.replace(/,/g, "")
                : value.replace(",", ".");
        } else if (hasDot) {
            // N0 can render 1.560 in an es-AR culture. Treat groups of 3 as thousands.
            normalized = /^-?\d{1,3}(\.\d{3})+$/.test(value)
                ? value.replace(/\./g, "")
                : value;
        }

        return Number.parseFloat(normalized);
    }

    function formatValue(value, kind) {
        return new Intl.NumberFormat(getLanguage() === "en" ? "en-US" : "es-AR", {
            minimumFractionDigits: 0,
            maximumFractionDigits: converters[kind].decimals
        }).format(value);
    }

    function ensureUnitHint(item, units) {
        const { input, kind } = item;
        const converter = converters[kind];
        const unit = units === "imperial" ? converter.imperialUnit : converter.metricUnit;
        const groupSuffix = input.closest(".input-group")?.querySelector(".input-group-text:last-child");

        if (groupSuffix) {
            const knownUnits = new Set([
                "m", "ft", "m²", "ft²", "m³", "ft³", "L", "US gal",
                "m²/L", "ft²/US gal", "W", "BTU/h", "frig/h", "kcal/h", "°C", "°F"
            ]);
            if (knownUnits.has(compact(groupSuffix.textContent))) {
                groupSuffix.textContent = unit;
                return;
            }
        }

        if (!input.id) return;
        const label = document.querySelector(`label[for="${CSS.escape(input.id)}"]`);
        if (!label) return;

        let hint = label.querySelector(".presentation-unit-hint");
        if (!hint) {
            hint = document.createElement("span");
            hint.className = "presentation-unit-hint";
            hint.setAttribute("aria-hidden", "true");
            label.appendChild(hint);
        }
        hint.textContent = ` (${unit})`;
    }

    function prepareManagedInputs() {
        // Decimal tag helpers are not guaranteed to render type=number, so classify every named input.
        document.querySelectorAll("input[name]").forEach(input => {
            const kind = inputKind(input);
            if (!kind || managedInputs.some(item => item.input === input)) return;
            managedInputs.push({ input, kind, metricStep: input.step, currentUnits: "metric" });
        });
    }

    function convertInput(item, targetUnits) {
        const converter = converters[item.kind];
        if (item.currentUnits !== targetUnits) {
            const value = parseInputNumber(item.input.value);
            if (Number.isFinite(value)) {
                const converted = targetUnits === "imperial"
                    ? converter.toImperial(value)
                    : converter.toMetric(value);
                item.input.value = Number(converted.toFixed(converter.decimals === 0 ? 0 : 4)).toString();
            }
            item.currentUnits = targetUnits;
            item.input.dataset.presentationUnits = targetUnits;
            item.input.step = targetUnits === "imperial" && converter.decimals !== 0 ? "0.01" : item.metricStep;
        }
        ensureUnitHint(item, targetUnits);
    }

    function restoreMetricInputsBeforeSubmit(form, event) {
        const changed = [];
        for (const item of managedInputs) {
            if (item.input.form !== form || item.currentUnits !== "imperial") continue;
            const value = parseInputNumber(item.input.value);
            if (Number.isFinite(value)) {
                item.input.value = Number(converters[item.kind].toMetric(value).toFixed(6)).toString();
            }
            item.currentUnits = "metric";
            changed.push(item);
        }

        window.setTimeout(() => {
            if (event.defaultPrevented) changed.forEach(item => convertInput(item, "imperial"));
        }, 0);
    }

    function trackRenderedNode(node) {
        if (!renderedNodes.has(node)) renderedNodes.set(node, node.nodeValue ?? "");
    }

    function restoreRenderedNodes() {
        for (const [node, original] of renderedNodes) {
            if (node.isConnected) node.nodeValue = original;
        }
    }

    function replaceSingle(text, regex, kind, targetUnits) {
        return text.replace(regex, (match, raw) => {
            const value = parseDisplayNumber(raw);
            if (!Number.isFinite(value)) return match;
            const converter = converters[kind];
            const converted = targetUnits === "imperial" ? converter.toImperial(value) : value;
            const unit = targetUnits === "imperial" ? converter.imperialUnit : converter.metricUnit;
            return `${formatValue(converted, kind)} ${unit}`;
        });
    }

    function transformMeasurementText(source, targetUnits) {
        let text = source;
        text = replaceSingle(text, /(-?[\d.,]+)\s*(?:frigorías|cooling units|frig\/h)/gi, "coolingCapacity", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*m²/g, "area", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*m³/g, "volume", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*(?:litros?|liters?|L)(?![\w²/])/gi, "liters", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*kcal\/h/gi, "kcalPerHour", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*W(?![\w/])/g, "heatingPower", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*°C/g, "temperature", targetUnits);
        return text;
    }

    function convertSplitCoolingValues(targetUnits) {
        document.querySelectorAll("p").forEach(paragraph => {
            const text = compact(paragraph.textContent);
            if (!/(frigorías|cooling units|frig\/h)/i.test(text)) return;

            const numericStrongNodes = Array.from(paragraph.querySelectorAll("strong"))
                .map(element => element.firstChild)
                .filter(node => node && /^\s*-?[\d.,]+\s*$/.test(node.nodeValue ?? ""));

            numericStrongNodes.forEach(node => {
                trackRenderedNode(node);
                const metric = parseDisplayNumber(renderedNodes.get(node));
                if (!Number.isFinite(metric)) return;
                const value = targetUnits === "imperial"
                    ? converters.coolingCapacity.toImperial(metric)
                    : metric;
                node.nodeValue = formatValue(value, "coolingCapacity");
            });

            Array.from(paragraph.childNodes)
                .filter(node => node.nodeType === Node.TEXT_NODE && /(frigorías|cooling units|frig\/h)/i.test(node.nodeValue ?? ""))
                .forEach(node => {
                    trackRenderedNode(node);
                    const unit = targetUnits === "imperial" ? "BTU/h" : "frig/h";
                    node.nodeValue = (renderedNodes.get(node) ?? "")
                        .replace(/frigorías|cooling units|frig\/h/gi, unit);
                });
        });
    }

    function convertRenderedMeasurements(targetUnits) {
        restoreRenderedNodes();

        const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
        const nodes = [];
        while (walker.nextNode()) nodes.push(walker.currentNode);

        for (const node of nodes) {
            const parent = node.parentElement;
            if (!parent || ["SCRIPT", "STYLE", "CODE", "PRE", "INPUT", "OPTION", "TEXTAREA"].includes(parent.tagName)) continue;
            const source = node.nodeValue ?? "";
            if (!/[\d]/.test(source)) continue;
            if (!/(m²|m³|litros?|liters?|\bL\b|kcal\/h|\bW\b|frigorías|cooling units|frig\/h|°C)/i.test(source)) continue;

            trackRenderedNode(node);
            node.nodeValue = transformMeasurementText(renderedNodes.get(node) ?? source, targetUnits);
        }

        convertSplitCoolingValues(targetUnits);
    }

    function applyUnits(units) {
        prepareManagedInputs();
        managedInputs.forEach(item => convertInput(item, units));
        convertRenderedMeasurements(units);

        document.querySelectorAll("[data-pref-units]").forEach(button => {
            const active = button.dataset.prefUnits === units;
            button.classList.toggle("active", active);
            button.setAttribute("aria-pressed", active ? "true" : "false");
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-pref-language]").forEach(button => {
            button.addEventListener("click", event => {
                event.preventDefault();
                event.stopImmediatePropagation();
                localStorage.setItem(LANGUAGE_KEY, button.dataset.prefLanguage === "en" ? "en" : "es");
                window.location.reload();
            }, true);
        });

        document.querySelectorAll("[data-pref-units]").forEach(button => {
            button.addEventListener("click", () => {
                const next = button.dataset.prefUnits === "imperial" ? "imperial" : "metric";
                if (getUnits() === next) return;
                localStorage.setItem(UNITS_KEY, next);
                applyUnits(next);
            });
        });

        document.querySelectorAll("form").forEach(form => {
            form.addEventListener("submit", event => restoreMetricInputsBeforeSubmit(form, event), true);
        });

        window.setTimeout(() => applyUnits(getUnits()), 0);
    });
})();

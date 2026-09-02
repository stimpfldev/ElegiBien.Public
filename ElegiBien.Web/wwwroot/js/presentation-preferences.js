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
            toImperial: value => value * 3.280839895,
            toMetric: value => value / 3.280839895,
            metricUnit: "m",
            imperialUnit: "ft",
            decimals: 2
        },
        area: {
            toImperial: value => value * 10.763910417,
            toMetric: value => value / 10.763910417,
            metricUnit: "m²",
            imperialUnit: "ft²",
            decimals: 2
        },
        volume: {
            toImperial: value => value * 35.314666721,
            toMetric: value => value / 35.314666721,
            metricUnit: "m³",
            imperialUnit: "ft³",
            decimals: 2
        },
        liters: {
            toImperial: value => value * 0.264172052,
            toMetric: value => value / 0.264172052,
            metricUnit: "L",
            imperialUnit: "US gal",
            decimals: 2
        },
        paintCoverage: {
            toImperial: value => value * 40.74583339,
            toMetric: value => value / 40.74583339,
            metricUnit: "m²/L",
            imperialUnit: "ft²/US gal",
            decimals: 2
        },
        heatingPower: {
            toImperial: value => value * 3.412141633,
            toMetric: value => value / 3.412141633,
            metricUnit: "W",
            imperialUnit: "BTU/h",
            decimals: 0
        },
        coolingCapacity: {
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
        if (/(AreaSquareMeters|SquareMetersPerBox|SquareMeters)$/i.test(name)) return "area";
        if (/(Celsius|TemperatureCelsius)$/i.test(name)) return "temperature";
        return null;
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
            normalized = value.replace(",", ".");
        } else if (hasDot && /^-?\d{1,3}(\.\d{3})+$/.test(value)) {
            normalized = value.replace(/\./g, "");
        }

        return Number.parseFloat(normalized);
    }

    function formatValue(value, kind) {
        const decimals = converters[kind].decimals;
        return new Intl.NumberFormat(getLanguage() === "en" ? "en-US" : "es-AR", {
            minimumFractionDigits: 0,
            maximumFractionDigits: decimals
        }).format(value);
    }

    function ensureUnitHint(item, units) {
        const { input, kind } = item;
        const converter = converters[kind];
        const unit = units === "imperial" ? converter.imperialUnit : converter.metricUnit;
        const groupSuffix = input.closest(".input-group")?.querySelector(".input-group-text:last-child");

        if (groupSuffix) {
            const knownUnits = [converter.metricUnit, converter.imperialUnit, "m", "m²", "W", "ft", "ft²", "BTU/h"];
            if (knownUnits.includes(compact(groupSuffix.textContent))) {
                groupSuffix.textContent = unit;
                return;
            }
        }

        if (!input.id) return;
        const label = document.querySelector(`label[for="${input.id}"]`);
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
        document.querySelectorAll('input[type="number"], input:not([type])').forEach(input => {
            const kind = inputKind(input);
            if (!kind || managedInputs.some(item => item.input === input)) return;
            managedInputs.push({
                input,
                kind,
                metricStep: input.step,
                currentUnits: "metric"
            });
        });
    }

    function convertInput(item, targetUnits) {
        const { input, kind, metricStep } = item;
        const converter = converters[kind];
        if (item.currentUnits === targetUnits) {
            ensureUnitHint(item, targetUnits);
            return;
        }

        const value = Number.parseFloat(input.value);
        if (Number.isFinite(value)) {
            const converted = targetUnits === "imperial"
                ? converter.toImperial(value)
                : converter.toMetric(value);
            input.value = Number(converted.toFixed(converter.decimals === 0 ? 0 : 4)).toString();
        }

        item.currentUnits = targetUnits;
        input.dataset.presentationUnits = targetUnits;
        input.step = targetUnits === "imperial" && converter.decimals !== 0 ? "0.01" : metricStep;
        ensureUnitHint(item, targetUnits);
    }

    function restoreMetricInputsBeforeSubmit(form, event) {
        const changed = [];
        for (const item of managedInputs) {
            if (item.input.form !== form || item.currentUnits !== "imperial") continue;
            const value = Number.parseFloat(item.input.value);
            if (Number.isFinite(value)) {
                item.input.value = Number(converters[item.kind].toMetric(value).toFixed(4)).toString();
            }
            item.currentUnits = "metric";
            changed.push(item);
        }

        if (changed.length > 0) {
            window.setTimeout(() => {
                if (event.defaultPrevented) changed.forEach(item => convertInput(item, "imperial"));
            }, 0);
        }
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

    function replaceRange(text, regex, kind, targetUnits) {
        return text.replace(regex, (match, firstRaw, separator, secondRaw) => {
            const first = parseDisplayNumber(firstRaw);
            const second = parseDisplayNumber(secondRaw);
            if (!Number.isFinite(first) || !Number.isFinite(second)) return match;
            const converter = converters[kind];
            const firstValue = targetUnits === "imperial" ? converter.toImperial(first) : first;
            const secondValue = targetUnits === "imperial" ? converter.toImperial(second) : second;
            const unit = targetUnits === "imperial" ? converter.imperialUnit : converter.metricUnit;
            return `${formatValue(firstValue, kind)} ${separator} ${formatValue(secondValue, kind)} ${unit}`;
        });
    }

    function transformMeasurementText(source, targetUnits) {
        let text = source;
        const number = "(-?[\\d.,]+)";
        const separator = "(a|to)";

        text = replaceRange(text, new RegExp(`${number}\\s*${separator}\\s*${number}\\s*(?:frigorías|cooling units|frig\\/h)`, "gi"), "coolingCapacity", targetUnits);
        text = replaceRange(text, new RegExp(`${number}\\s*${separator}\\s*${number}\\s*kcal\\/h`, "gi"), "kcalPerHour", targetUnits);
        text = replaceRange(text, new RegExp(`${number}\\s*${separator}\\s*${number}\\s*W\\b`, "g"), "heatingPower", targetUnits);

        text = replaceSingle(text, /(-?[\d.,]+)\s*(?:frigorías|cooling units|frig\/h)\b/gi, "coolingCapacity", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*m²\b/g, "area", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*m³\b/g, "volume", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*(?:litros?|liters?|L)\b/gi, "liters", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*kcal\/h\b/gi, "kcalPerHour", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*W\b/g, "heatingPower", targetUnits);
        text = replaceSingle(text, /(-?[\d.,]+)\s*°C\b/g, "temperature", targetUnits);

        return text;
    }

    function convertSplitCoolingRanges(targetUnits) {
        document.querySelectorAll("p").forEach(paragraph => {
            const text = compact(paragraph.textContent);
            if (!/(frigorías|cooling units|frig\/h)/i.test(text)) return;

            const numericStrongNodes = Array.from(paragraph.querySelectorAll("strong"))
                .map(element => element.firstChild)
                .filter(node => node && /^\s*-?[\d.,]+\s*$/.test(node.nodeValue ?? ""));

            if (numericStrongNodes.length < 2) return;

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

        convertSplitCoolingRanges(targetUnits);
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

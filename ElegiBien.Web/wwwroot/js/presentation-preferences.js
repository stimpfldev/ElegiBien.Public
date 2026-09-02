"use strict";

(() => {
    const UNITS_KEY = "elegibien.units";
    const managedInputs = [];

    const getUnits = () => localStorage.getItem(UNITS_KEY) === "imperial" ? "imperial" : "metric";
    const compact = value => (value ?? "").replace(/\s+/g, " ").trim();

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
        if (/HeatingCapacityWatts$/i.test(name)) return "power";
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

    function parseDisplayNumber(text) {
        const lastComma = text.lastIndexOf(",");
        const lastDot = text.lastIndexOf(".");
        let normalized = text;
        if (lastComma > lastDot) normalized = text.replace(/\./g, "").replace(",", ".");
        else if (lastDot > lastComma && lastComma >= 0) normalized = text.replace(/,/g, "");
        else if (lastComma >= 0) normalized = text.replace(",", ".");
        return Number.parseFloat(normalized);
    }

    function convertRenderedMeasurements(targetUnits) {
        document.querySelectorAll("[data-presentation-converted]").forEach(element => {
            element.textContent = element.dataset.presentationOriginal ?? element.textContent;
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
            const active = button.dataset.prefUnits === units;
            button.classList.toggle("active", active);
            button.setAttribute("aria-pressed", active ? "true" : "false");
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-pref-units]").forEach(button => {
            button.addEventListener("click", () => {
                const next = button.dataset.prefUnits === "imperial" ? "imperial" : "metric";
                if (getUnits() === next) return;
                localStorage.setItem(UNITS_KEY, next);
                applyUnits(next);
            });
        });

        document.querySelectorAll("form").forEach(form => {
            form.addEventListener("submit", () => restoreMetricInputsBeforeSubmit(form), true);
        });

        applyUnits(getUnits());
    });
})();

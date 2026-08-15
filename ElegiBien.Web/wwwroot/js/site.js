"use strict";

if ("serviceWorker" in navigator) {
    window.addEventListener("load", () => {
        navigator.serviceWorker.register("/service-worker.js").catch(() => {
            // La aplicación sigue funcionando aunque el navegador no registre la PWA.
        });
    });
}

document.addEventListener("DOMContentLoaded", () => {
    const isVisible = element => {
        if (!element) return false;
        const style = window.getComputedStyle(element);
        return style.display !== "none" && style.visibility !== "hidden" && element.getClientRects().length > 0;
    };

    const firstValidationError = Array.from(
        document.querySelectorAll(".field-validation-error, [data-validation-summary] li")
    ).find(isVisible);

    let target = firstValidationError?.closest(".mb-3, .form-check, [data-validation-summary]")
        ?? document.querySelector("[data-auto-scroll-target]");

    if (!target) return;

    const header = document.querySelector(".site-header");
    const headerHeight = header?.getBoundingClientRect().height ?? 0;
    const top = target.getBoundingClientRect().top + window.scrollY - headerHeight - 18;
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    window.setTimeout(() => {
        window.scrollTo({ top: Math.max(0, top), behavior: reducedMotion ? "auto" : "smooth" });
        if (target.matches("[data-auto-scroll-target]")) {
            target.focus({ preventScroll: true });
            target.classList.add("result-focus-visible");
            window.setTimeout(() => target.classList.remove("result-focus-visible"), 1800);
        }
    }, 80);
});

document.addEventListener("click", async event => {
    const button = event.target.closest("[data-copy-share-url]");
    if (!button) return;

    const container = button.closest(".input-group");
    const input = container?.querySelector("[data-share-url]");
    if (!input?.value) return;

    try {
        await navigator.clipboard.writeText(input.value);
        const originalText = button.textContent;
        button.textContent = "Copiado";
        window.setTimeout(() => { button.textContent = originalText; }, 1400);
    } catch {
        input.focus();
        input.select();
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const pattern = document.querySelector("[data-flooring-pattern]");
    const waste = document.querySelector("[data-flooring-waste]");
    if (!pattern || !waste) return;

    const value = document.querySelector("[data-flooring-waste-value]");
    const explanation = document.querySelector("[data-flooring-waste-explanation]");
    const recommendations = {
        "1": { value: "10", text: "para cortes, roturas y futuras reposiciones." },
        "2": { value: "12", text: "porque la colocación trabada suele requerir más cortes." },
        "3": { value: "15", text: "porque la colocación diagonal requiere más cortes." }
    };

    const applyRecommendation = () => {
        const recommendation = recommendations[pattern.value] ?? recommendations["1"];
        waste.value = recommendation.value;
        if (value) value.textContent = `${recommendation.value} %`;
        if (explanation) explanation.textContent = recommendation.text;
    };

    pattern.addEventListener("change", applyRecommendation);
    applyRecommendation();
});

document.addEventListener("DOMContentLoaded", () => {
    const measurementElement =
        document.querySelector('meta[name="google-analytics-measurement-id"]');

    const measurementId = measurementElement?.content?.trim();

    if (!measurementId) {
        return;
    }

    window.dataLayer = window.dataLayer || [];

    window.gtag = function () {
        window.dataLayer.push(arguments);
    };

    window.gtag("js", new Date());
    window.gtag("config", measurementId);

    const script = document.createElement("script");
    script.async = true;
    script.src =
        `https://www.googletagmanager.com/gtag/js?id=${encodeURIComponent(measurementId)}`;

    document.head.appendChild(script);
});
document.addEventListener("DOMContentLoaded", () => {
    const measurementElement =
        document.querySelector('meta[name="google-analytics-measurement-id"]');

    const measurementId = measurementElement?.content?.trim();

    if (!measurementId) {
        return;
    }

    window.dataLayer = window.dataLayer || [];

    window.gtag = function () {
        window.dataLayer.push(arguments);
    };

    // Por defecto: Analytics habilitado.
    // Publicidad permanece deshabilitada porque AdSense todavía no está activo.
    window.gtag("consent", "default", {
        analytics_storage: "granted",
        ad_storage: "denied",
        ad_user_data: "denied",
        ad_personalization: "denied"
    });

    // EEE + Reino Unido + Suiza:
    // Analytics requiere decisión de consentimiento.
    window.gtag("consent", "default", {
        analytics_storage: "denied",
        ad_storage: "denied",
        ad_user_data: "denied",
        ad_personalization: "denied",
        wait_for_update: 500,
        region: [
            "AT", "BE", "BG", "HR", "CY", "CZ",
            "DK", "EE", "FI", "FR", "DE", "GR",
            "HU", "IE", "IT", "LV", "LT", "LU",
            "MT", "NL", "PL", "PT", "RO", "SK",
            "SI", "ES", "SE",
            "IS", "LI", "NO",
            "GB", "CH"
        ]
    });

    window.gtag("js", new Date());

    window.gtag("config", measurementId);

    const script = document.createElement("script");
    script.async = true;
    script.src =
        `https://www.googletagmanager.com/gtag/js?id=${encodeURIComponent(measurementId)}`;

    document.head.appendChild(script);
});
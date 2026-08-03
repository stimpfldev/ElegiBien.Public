const CACHE_NAME = "elegibien-static-v1";

const STATIC_ASSETS = [
    "/offline.html",
    "/manifest.webmanifest",
    "/icons/icon-192.png",
    "/icons/icon-512.png",
    "/css/site.css",
    "/js/site.js",
    "/lib/bootstrap/dist/css/bootstrap.min.css",
    "/lib/bootstrap/dist/js/bootstrap.bundle.min.js"
];

self.addEventListener("install", event => {
    event.waitUntil(
        caches
            .open(CACHE_NAME)
            .then(cache => cache.addAll(STATIC_ASSETS))
    );

    self.skipWaiting();
});

self.addEventListener("activate", event => {
    event.waitUntil(
        caches
            .keys()
            .then(keys =>
                Promise.all(
                    keys
                        .filter(key => key !== CACHE_NAME)
                        .map(key => caches.delete(key))
                )
            )
    );

    self.clients.claim();
});

self.addEventListener("fetch", event => {
    const request = event.request;

    if (request.method !== "GET") {
        return;
    }

    if (request.mode === "navigate") {
        event.respondWith(
            fetch(request).catch(() =>
                caches.match("/offline.html")
            )
        );

        return;
    }

    event.respondWith(
        caches.match(request).then(cachedResponse => {
            if (cachedResponse) {
                return cachedResponse;
            }

            return fetch(request);
        })
    );
});
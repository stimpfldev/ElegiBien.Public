"use strict";

(() => {
    const unitsKey = "elegibien.units";
    const migrationKey = "elegibien.units-default-1.5.0";

    if (localStorage.getItem(migrationKey) !== "done") {
        localStorage.setItem(unitsKey, "metric");
        localStorage.setItem(migrationKey, "done");
    }
})();

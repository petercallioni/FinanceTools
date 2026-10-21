(() => {
    const storageKey = "financetools-theme";

    function sync() {
        const isDark = localStorage.getItem(storageKey) === "dark";

        document.documentElement.setAttribute(
            "data-bs-theme",
            isDark ? "dark" : "light");

        const toggle = document.getElementById("dark-mode-toggle");
        if (toggle) {
            toggle.checked = isDark;
        }
    }

    window.financeTheme = { sync };

    document.addEventListener("change", event => {
        if (event.target?.id !== "dark-mode-toggle") return;

        localStorage.setItem(
            storageKey,
            event.target.checked ? "dark" : "light");

        sync();
    });

    sync();

    const themeObserver = new MutationObserver(() => {
        const theme = localStorage.getItem(storageKey) === "dark" ? "dark" : "light";
        if (document.documentElement.getAttribute("data-bs-theme") !== theme) {
            sync();
        }
    });
    themeObserver.observe(document.documentElement, {
        attributes: true,
        attributeFilter: ["data-bs-theme"]
    });
})();
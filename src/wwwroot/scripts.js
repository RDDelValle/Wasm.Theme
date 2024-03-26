const _darkThemeMediaQuery = window.matchMedia("(prefers-color-scheme: dark)"),
    _bodyThemeAttribute = "data-bs-theme",
    _darkTheme = "Dark",
    _lightTheme = "Light";

export function notifyIfSystemThemeChanges(ref, callback) {
    _darkThemeMediaQuery.addEventListener("change", () => {
        let theme = getSystemTheme();
        ref.invokeMethodAsync(callback, theme);
    })
}

export function getSystemTheme() {
    return _darkThemeMediaQuery.matches ? _darkTheme : _lightTheme;
}

export function setDocumentTheme(attr, value) {
    document.body.setAttribute(attr, value.toLowerCase())
}

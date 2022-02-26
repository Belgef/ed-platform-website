function darkMode() {
    var url = window.location.href.toString();

    if (url.indexOf("dark") > -1) {
        url = url.replace(/dark/g, "light");
    }
    else if (url.indexOf("light") > -1) {
        url = url.replace(/light/g, "dark");
    }

    else {
        if (document.body.classList.contains("dark-mode"))
            url += "?mode=light";
        else
            url += "?mode=dark";
    }
    window.location.href = url;
}


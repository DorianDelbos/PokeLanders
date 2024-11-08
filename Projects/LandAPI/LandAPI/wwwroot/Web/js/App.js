$('#ThemeToggle').on('click', () => {
    const body = $('body');

    // Check current theme and toggle between light and dark
    if (body.attr('data-bs-theme') === 'dark') {
        // If dark mode is on, switch to light mode
        body.attr('data-bs-theme', 'light');
        localStorage.setItem('theme', 'light'); // Save 
    } else {
        // If dark mode is off, switch to dark mode
        body.attr('data-bs-theme', 'dark');
        localStorage.setItem('theme', 'dark'); // Save preference
    }

    var theme = body.attr('data-bs-theme');
    localStorage.setItem('theme', theme);
    document.getElementById('ThemeToggle').innerHTML = theme.charAt(0).toUpperCase() + String(theme).slice(1) + " Mode";
});

$(document).ready(() => {
    const button = $('#ThemeToggle');
    var theme = localStorage.getItem('theme');
    $('body').attr('data-bs-theme', theme);
    document.getElementById('ThemeToggle').innerHTML = theme.charAt(0).toUpperCase() + String(theme).slice(1) + " Mode";
    button.attr('aria-pressed', (theme === "dark" ? true : false));

    if (theme === 'dark') {
        button.addClass('active');
    } else {
        button.removeClass('active');
    }
});

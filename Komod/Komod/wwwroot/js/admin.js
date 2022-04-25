$(document).ready(function () {
    $('.admin-menu__link').on('click', function(e) {
        e.preventDefault();
        $(this).next().toggleClass("admin-submenu_active");
    });
});
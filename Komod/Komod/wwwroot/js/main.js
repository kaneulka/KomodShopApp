$(document).ready(function () {

    var slider = document.getElementById('price__slider');
    var inputMin = document.getElementsByClassName("price__input_min");
    var inputMax = document.getElementsByClassName("price__input_max");
    var minPrice = parseInt($('.price__input_min').val());
    var maxPrice = parseInt($('.price__input_max').val());

    noUiSlider.create(slider, {
        start: [minPrice, maxPrice],
        connect: true,
        range: {
            'min': minPrice,
            'max': maxPrice
        }
    });

    slider.noUiSlider.on('update', function (values, handle) {

        var value = values[handle];
        var val = Math.round(value);

        if (handle) {
            $('.price__input_max').val(val);
        } else {
            $('.price__input_min').val(val);
        }
    });

    $('.price__input_min').change(function () {
        slider.noUiSlider.set([this.value, null]);
    });

    $('.price__input_max').change(function () {
        slider.noUiSlider.set([null, this.value]);
    });

});

$(document).on('click', '.filter_open', function (e) {
    e.preventDefault();
    var height = $(this).next('.filter__propertyValues').height();
    if (height > 200) {
        $(this).next('.filter__propertyValues').addClass("filter__propertyValues_height");
    }
    $(this).next('.filter__propertyValues').toggleClass("filter__propertyValues_active");
});

/* Переключения между табами в информации о продукте */
/*$('.description-btn').on('click', function (e) {
    e.preventDefault();
    $('.properties-btn').removeClass("tabs__link_active");
    $(this).addClass("tabs__link_active");
    $('#properties').removeClass("property_active");
    $('#description').addClass("description_active");
});

$('.properties-btn').on('click', function (e) {
    e.preventDefault();
    $('.description-btn').removeClass("tabs__link_active");
    $(this).addClass("tabs__link_active");
    $('#description').removeClass("description_active");
    $('#properties').addClass("property_active");
});*/

/* Работа счетчика количества */
/*$('.counter__input').change(function () {
    var max = parseInt($(this).attr('max'));
    var min = parseInt($(this).attr('min'));
    if ($(this).val() > max) {
        $(this).val(max);
    } else if ($(this).val() < min) {
        $(this).val(min);
    }
});

$('.counter__minus').on('click', function(e) {
    e.preventDefault();
    var value = parseInt($('.counter__input').val());
    
    if(value > 1) {
        value -= 1;
    }
    
    $('.counter__input').val(value);
});

$('.counter__plus').on('click', function(e) {
    e.preventDefault();
    var value = parseInt($('.counter__input').val());
    
    if(value < 10000) {
        value += 1;
    }
    
    $('.counter__input').val(value);
});*/


/* Переключение картинок в информации о товаре */
$('.img-slider__img').on('click', function() {
    var src = $(this).attr("src");
    $('.img-slider__main-img').attr("src", src);
});

/* Для открытия выподающего меню с логином и регистрацией */
$('#open-login').on('click', function(e) {
    e.preventDefault();
    $('.login-menu').toggleClass("login-menu_active");
});
$('.login-menu a').on('click', function() {
    $('.login-menu').removeClass("login-menu_active");
});

/* Выподающее меню каталога */
$(document).on('click', ".dropbtn-catalog", function (e) {
    e.preventDefault();
    $('.catalog-menu').load("/Home/GetCategoriesList");
    $(this).next().toggleClass("catalog-menu_active")
});
$(document).on("mouseenter", ".catalog-menu__item", function () {
    var parentId = $(this).attr("data-parentId");
    parentId = encodeURIComponent(parentId);
    $('.catalog-submenu').load("/Home/GetSubCategoriesList/?parentId=" + parentId);
    $(this).children(".catalog-submenu").addClass("catalog-menu_active");
});
$(document).on("mouseleave", ".catalog-menu__item", function () {
    $(this).children(".catalog-submenu").removeClass("catalog-menu_active");
});

$(document).on("mouseenter", ".catalog-submenu__item", function () {
    var parentId = $(this).attr("data-parentId");
    parentId = encodeURIComponent(parentId);
    $(this).children(".catalog-submenu").load("/Home/GetSubCategoriesList/?parentId=" + parentId);
    $(this).children(".catalog-submenu").addClass("catalog-submenu_active");
});
$(document).on("mouseleave", ".catalog-submenu__item", function () {
    $(this).children(".catalog-submenu").removeClass("catalog-submenu_active");
});

window.onclick = function (event) {
    if (!event.target.matches('.dropbtn-catalog') && !event.target.matches('.dropbtn')) {

        var dropdowns = document.getElementsByClassName("catalog-menu");
        var dropdowns1 = document.getElementsByClassName("catalog-submenu");
        var i;
        var i1;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('catalog-menu_active')) {
                openDropdown.classList.remove('catalog-menu_active');
            }
        }
        for (i1 = 0; i1 < dropdowns1.length; i1++) {
            openDropdown = dropdowns1[i1];
            if (openDropdown.classList.contains('catalog-submenu_active')) {
                openDropdown.classList.remove('catalog-submenu_active');
            }
        }
    }
};

$(document).on("click", ".burger-btn", function (e) {
    e.preventDefault();
    $('.search-mobile').removeClass("search-mobile_active");
    $('.phone-list-mobile').removeClass("phone-list-mobile_active");
    $('.user-menu-mobile').removeClass("user-menu-mobile_active");
    $('.catalog-menu-mobile').removeClass("catalog-menu-mobile_active");
    $(this).toggleClass("burger-btn_active");
    $('.mobile-menu').toggleClass("mobile-menu_active");
});

$(document).on("click", ".user-mobile-btn", function (e) {
    e.preventDefault();
    $('.search-mobile').removeClass("search-mobile_active");
    $('.phone-list-mobile').removeClass("phone-list-mobile_active");
    $('.burger-btn').removeClass("burger-btn_active");
    $('.mobile-menu').removeClass("mobile-menu_active");
    $('.catalog-menu-mobile').removeClass("catalog-menu-mobile_active");
    $('.user-menu-mobile').toggleClass("user-menu-mobile_active");
});

$(document).on("click", ".search-mobile-btn", function (e) {
    e.preventDefault();
    $('.phone-list-mobile').removeClass("phone-list-mobile_active");
    $('.user-menu-mobile').removeClass("user-menu-mobile_active");
    $('.burger-btn').removeClass("burger-btn_active");
    $('.mobile-menu').removeClass("mobile-menu_active");
    $('.catalog-menu-mobile').removeClass("catalog-menu-mobile_active");
    $('.search-mobile').toggleClass("search-mobile_active");
}); 

$(document).on("click", ".phone-list-mobile-btn", function (e) {
    e.preventDefault();
    $('.user-menu-mobile').removeClass("user-menu-mobile_active");
    $('.search-mobile').removeClass("search-mobile_active");
    $('.burger-btn').removeClass("burger-btn_active");
    $('.mobile-menu').removeClass("mobile-menu_active");
    $('.catalog-menu-mobile').removeClass("catalog-menu-mobile_active");
    $('.phone-list-mobile').toggleClass("phone-list-mobile_active");
});

$(document).on("click", ".catalog-bar-mobile__title", function (e) {
    e.preventDefault();
    $('.catalog-menu-mobile').load("/Home/GetCategoriesListMobile");
    $('.user-menu-mobile').removeClass("user-menu-mobile_active");
    $('.search-mobile').removeClass("search-mobile_active");
    $('.burger-btn').removeClass("burger-btn_active");
    $('.mobile-menu').removeClass("mobile-menu_active");
    $('.phone-list-mobile').removeClass("phone-list-mobile_active");
    $('.catalog-menu-mobile').toggleClass("catalog-menu-mobile_active");
});

$(document).on("click", ".catalog-menu-mobile__link", function (e) {
    var href = $(this).attr("href");
    if (href === "#") {
        e.preventDefault();
        var parentId = $(this).attr("data-parentId");
        parentId = encodeURIComponent(parentId);
        $(this).parent(".catalog-menu-mobile__group").next(".catalog-submenu-mobile").load("/Home/GetSubCategoriesListMobile/?parentId=" + parentId);
        $(this).parent(".catalog-menu-mobile__group").next(".catalog-submenu-mobile").toggleClass("catalog-submenu-mobile_active");
    }
});

$(document).on("click", ".catalog-submenu-mobile__link", function (e) {
    var href = $(this).attr("href");
    if (href === "#") {
        e.preventDefault();
        var parentId = $(this).attr("data-parentId");
        parentId = encodeURIComponent(parentId);
        $(this).parent(".catalog-submenu-mobile__group").next(".catalog-submenu-mobile").load("/Home/GetSubCategoriesListMobile/?parentId=" + parentId);
        $(this).parent(".catalog-submenu-mobile__group").next(".catalog-submenu-mobile").toggleClass("catalog-submenu-mobile_active");
    }
});

$(document).on("click", ".product-info-mobile__btn-properties", function (e) {
    e.preventDefault();
    $(this).addClass("product-info-mobile__btn-properties_active");
    $('.product-info-mobile__btn-description').removeClass("product-info-mobile__btn-description_active");
    $('.product-info-mobile__properties').addClass("product-info-mobile__properties_active");
    $('.product-info-mobile__description').removeClass("product-info-mobile__description_active");
});

$(document).on("click", ".product-info-mobile__btn-description", function (e) {
    e.preventDefault();
    $(this).addClass("product-info-mobile__btn-description_active");
    $('.product-info-mobile__btn-properties').removeClass("product-info-mobile__btn-properties_active");
    $('.product-info-mobile__description').addClass("product-info-mobile__description_active");
    $('.product-info-mobile__properties').removeClass("product-info-mobile__properties_active");
});

$(document).on('click', ".lk-menu__btn", function (e) {
    e.preventDefault();
    $('.lk-menu').toggleClass("lk-menu_active");
});

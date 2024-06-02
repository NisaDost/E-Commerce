(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow');
            } else {
                $('.fixed-top').removeClass('shadow');
            }
        } else {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow').css('top', -55);
            } else {
                $('.fixed-top').removeClass('shadow').css('top', 0);
            }
        } 
    });
    
    
   // Back to top button
   $(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
        $('.back-to-top').fadeIn('slow');
    } else {
        $('.back-to-top').fadeOut('slow');
    }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 50, 'easeInOutExpo');
        return false;
    });


    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:1
            },
            992:{
                items:2
            },
            1200:{
                items:2
            }
        }
    });


    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: false,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            },
            1200:{
                items:4
            }
        }
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });



    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });

})(jQuery);

document.addEventListener('DOMContentLoaded', function () {
    const categoryCheckboxes = document.querySelectorAll('.category-checkbox');
    const brandCheckboxes = document.querySelectorAll('.brand-checkbox');
    const priceFilter = document.getElementById('priceFilter');
    const productItems = document.querySelectorAll('.product-item');

    function filterProducts() {
        let selectedCategories = Array.from(categoryCheckboxes)
            .filter(checkbox => checkbox.checked && checkbox.value !== 'all')
            .map(checkbox => checkbox.value);

        let selectedBrands = Array.from(brandCheckboxes)
            .filter(checkbox => checkbox.checked && checkbox.value !== 'all')
            .map(checkbox => checkbox.value);

        productItems.forEach(item => {
            const itemCategory = item.getAttribute('data-category');
            const itemBrand = item.getAttribute('data-brand');

            let categoryMatch = selectedCategories.length === 0 || selectedCategories.includes(itemCategory);
            let brandMatch = selectedBrands.length === 0 || selectedBrands.includes(itemBrand);

            if (categoryMatch && brandMatch) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });

        sortProducts();
    }

    function sortProducts() {
        const sortOrder = priceFilter.value;
        const productList = document.getElementById('product-list');
        const products = Array.from(productItems).filter(item => item.style.display !== 'none');

        products.sort((a, b) => {
            const priceA = parseFloat(a.getAttribute('data-price'));
            const priceB = parseFloat(b.getAttribute('data-price'));
            return sortOrder === 'asc' ? priceA - priceB : priceB - priceA;
        });

        products.forEach(product => productList.appendChild(product));
    }

    function handleCheckboxChange(checkboxes, allCheckbox) {
        return function () {
            const isAll = this.value === 'all';

            if (isAll) {
                if (this.checked) {
                    checkboxes.forEach(cb => {
                        if (cb !== this) cb.checked = false;
                    });
                } else {
                    this.checked = true;
                }
            } else {
                if (!this.checked) {
                    if (!Array.from(checkboxes).some(cb => cb.checked && cb !== allCheckbox)) {
                        allCheckbox.checked = true;
                    }
                } else {
                    allCheckbox.checked = false;
                }
            }

            filterProducts();
        };
    }

    categoryCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', handleCheckboxChange(categoryCheckboxes, document.getElementById('category-all')));
    });

    brandCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', handleCheckboxChange(brandCheckboxes, document.getElementById('brand-all')));
    });

    priceFilter.addEventListener('change', sortProducts);

    filterProducts();
});

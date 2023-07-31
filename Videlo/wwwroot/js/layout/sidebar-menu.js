const $btnSidebarToggle = $('#btnSidebarToggle');
const $sidebar = $('#sidebarCollapse');
const $main = $('main');

$(document).ready(function () {
    $btnSidebarToggle.on('click', function () {
        if ($(window).width() >= 992) {
            toggleLarge();
        } else {
            toggleSmall();
        }
    });

    function toggleLarge() {
        $main.toggleClass('col-lg-10 offset-lg-2');
        $sidebar.toggleClass('d-lg-block d-lg-none');

        $sidebar.addClass('d-none');
    }

    function toggleSmall() {
        $sidebar.toggleClass('d-none');
    }
});
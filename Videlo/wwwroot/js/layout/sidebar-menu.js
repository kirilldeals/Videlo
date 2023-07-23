const btnSidebarId = '#btnSidebarToggle';
const $sidebar = $('#sidebarCollapse');
const $main = $('main');

$(document).ready(function () {
    $(btnSidebarId).on('click', () => {
        $main.toggleClass('col-lg-10 offset-lg-2');
        $sidebar.toggleClass('d-none');
    });
});
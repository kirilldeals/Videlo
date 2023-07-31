const $searchInput = $('#video-search');
const $searchList = $('#search-suggestions');

$(document).ready(function () {
    $searchInput.on('input', function () {
        const query = $(this).val();

        $.get('/Home/Search',
            {
                query: query
            },
            function (data) {
                $searchList.empty();

                $.each(data, function () {
                    const suggestion = $('<div/>')
                        .attr('type', 'button')
                        .addClass('list-group-item list-group-item-action')
                        .text(this);
                    $searchList.append(suggestion);
                });
            });
    });

    $searchList.on('click', '.list-group-item', function () {
        $searchInput.val($(this).text());
    });
});
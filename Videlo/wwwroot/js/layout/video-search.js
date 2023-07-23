const searchInputId = '#video-search';
const searchListId = '#search-suggestions';

$(document).ready(function () {
    $(searchInputId).on('input', function () {
        const query = $(this).val();

        $.get('/Home/Search',
            {
                query: query
            },
            function (data) {
                $(searchListId).empty();

                $.each(data, function () {
                    const suggestion = $('<div/>')
                        .attr('type', 'button')
                        .addClass('list-group-item list-group-item-action')
                        .text(this);
                    $(searchListId).append(suggestion);
                });
            });
    });

    $(searchListId).on('click', '.list-group-item', function () {
        $(searchInputId).val($(this).text());
    });
});
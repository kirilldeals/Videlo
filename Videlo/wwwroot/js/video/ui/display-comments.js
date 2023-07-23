const videoCommentsContainerId = '#video-comments-container';

$(document).ready(function () {
    $('#comments-popular').on('click', function () {
        handleCommentsFilter(true);
    });

    $('#comments-new').on('click', function () {
        handleCommentsFilter(false);
    });

    handleCommentsFilter(true);
});

function handleCommentsFilter(isPopular) {
    getVideoComments($('#video').data('videoid'), isPopular)
        .then(displayComments)
        .catch(xhr => {
            console.log(xhr);
            displayComments('Unable to load comment section.');
        });
}

function displayComments(htmlString) {
    $(videoCommentsContainerId).html(htmlString);
}
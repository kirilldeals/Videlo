$(document).ready(function () {
    const $videoCommentsContainer = $('#video-comments-container');
    const $popularCommentsTab = $('#comments-popular');
    const $newCommentsTab = $('#comments-new');
    const $htmlVideo = $('#video');

    $popularCommentsTab.on('click', function () {
        handleCommentsFilter(true);
    });

    $newCommentsTab.on('click', function () {
        handleCommentsFilter(false);
    });

    handleCommentsFilter(true);

    function handleCommentsFilter(isPopular) {
        getVideoComments($htmlVideo.data('videoid'), isPopular)
            .then(displayComments)
            .catch(xhr => {
                console.log(xhr);
                displayComments('Unable to load comment section.');
            });
    }

    function displayComments(htmlString) {
        $videoCommentsContainer.html(htmlString);
    }
});
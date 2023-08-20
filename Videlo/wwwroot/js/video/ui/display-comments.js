$(document).ready(function () {
    const $videoCommentsContainer = $('#video-comments-container');
    const $popularCommentsTab = $('#comments-popular');
    const $newCommentsTab = $('#comments-new');
    const $htmlVideo = $('#video');

    let isPopularFilter = true;
    let pageIndex = 0;   

    $(window).on('scroll', function () {
        if (!$videoCommentsContainer.is(':empty'))
            infiniteScroll(loadComments)
    });

    $popularCommentsTab.on('click', function () {
        tabClick(true);
    });

    $newCommentsTab.on('click', function () {
        tabClick(false);
    });

    function tabClick(filter) {
        isPopularFilter = filter;
        pageIndex = 0;
        $videoCommentsContainer.empty();
        loadComments();
    }

    loadComments();

    function loadComments() {
        getVideoComments($htmlVideo.data('videoid'), isPopularFilter, pageIndex)
            .then(displayComments)
            .catch(xhr => {
                infiniteScrollError(xhr, 'Unable to load comment section.');
            });
    }

    function displayComments(htmlString) {
        $videoCommentsContainer.append(htmlString);
        pageIndex++;
    }
});
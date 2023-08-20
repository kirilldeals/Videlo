$(document).ready(function () {
    const $videosContainer = $('#videos-container');
    const $videoFilters = $('#video-filters');

    let filter = 0;
    let pageIndex = 0;

    $(window).on('scroll', function () {
        if (!$videosContainer.is(':empty'))
            infiniteScroll(handleVideosFilter)
    });

    $videoFilters.on('click', '.nav-link', function () {
        filter = $(this).data('filter');
        pageIndex = 0;
        $videosContainer.empty();
        handleVideosFilter();
    });

    handleVideosFilter();

    function handleVideosFilter() {
        getVideos($videosContainer.data('userid'), filter, $videosContainer.data('viewname'), pageIndex)
            .then(displayVideos)
            .catch(xhr => {
                infiniteScrollError(xhr, 'Unable to load video section.');
            });
    }

    function displayVideos(htmlString) {
        $videosContainer.append(htmlString);
        pageIndex++;
    }
});
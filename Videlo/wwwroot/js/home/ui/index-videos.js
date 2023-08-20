$(document).ready(function () {
    const $videosContainer = $('#videos-container');
    const searchQuery = $('#video-search').val();

    let pageIndex = 0;

    $(window).on('scroll', function () { infiniteScroll(loadVideos) });

    loadVideos();

    function loadVideos() {
        getIndexVideos(searchQuery, pageIndex)
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
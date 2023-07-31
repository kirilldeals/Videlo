$(document).ready(function () {
    const $videosContainer = $('#videos-container');
    const $videoFilters = $('#video-filters');

    $videoFilters.on('click', '.nav-link', function () {
        handleVideosFilter($(this).data('filter'));
    });

    handleVideosFilter(0);

    function handleVideosFilter(filter) {
        getVideos($videosContainer.data('userid'), filter, $videosContainer.data('viewname'))
            .then(displayVideos)
            .catch(xhr => {
                console.log(xhr);
                displayVideos('Unable to load videos.');
            });
    }

    function displayVideos(htmlString) {
        $videosContainer.html(htmlString);
    }
});
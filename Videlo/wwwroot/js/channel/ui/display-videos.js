const videosContainerId = '#videos-container';
const videoFiltersId = '#video-filters';

$(document).ready(function () {
    $(videoFiltersId).on('click', '.nav-link', function () {
        handleVideosFilter($(this).data('filter'));
    });

    handleVideosFilter(0);
});

function handleVideosFilter(filter) {
    getVideos($(videosContainerId).data('userid'), filter, $(videosContainerId).data('viewname'))
        .then(displayVideos)
        .catch(xhr => {
            console.log(xhr);
            displayVideos('Unable to load videos.');
        });
}

function displayVideos(htmlString) {
    $(videosContainerId).html(htmlString);
}
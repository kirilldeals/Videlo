$(document).ready(function () {
    const $videosContainer = $('#videos-container');

    let pageIndex = 0;

    $(window).on('scroll', function () { infiniteScroll(loadVideos) });

    loadVideos();

    function loadVideos() {
        getSubscriptionVideos(pageIndex)
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
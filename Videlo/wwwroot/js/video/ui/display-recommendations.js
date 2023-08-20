$(document).ready(function () {
    const $recommendationsContainer = $('#recommendations-container');
    const $htmlVideo = $('#video');

    let pageIndex = 0;

    $(window).on('scroll', function () { infiniteScroll(loadRecommendations) });

    loadRecommendations();

    function loadRecommendations() {
        getRecommendations($htmlVideo.data('videoid'), $htmlVideo.data('channelid'), pageIndex)
            .then(displayRecommendations)
            .catch(xhr => {
                infiniteScrollError(xhr, 'Unable to load video section.');
            });
    }

    function displayRecommendations(htmlString) {
        $recommendationsContainer.append(htmlString);
        pageIndex++;
    }
});
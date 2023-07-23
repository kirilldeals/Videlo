const $htmlVideo = $('#video');

$(document).ready(function () {
    let playbackStartTime = 0;
    let totalPlayTime = 0;
    const video = $htmlVideo[0];
    let timeBoundSeconds;

    $htmlVideo.on('timeupdate', function () {
        timeBoundSeconds = video.duration < 42 ? video.duration * 0.7 : 30;
        $(this).off('timeupdate');
    });

    $htmlVideo.on('play', function () {
        playbackStartTime = new Date().getTime();
    });

    $htmlVideo.on('pause', function () {
        updatePlaybackTime();
        checkPlayTime();
    });

    function updatePlaybackTime() {
        if (playbackStartTime != 0) {
            const elapsedTime = new Date().getTime() - playbackStartTime;
            totalPlayTime += elapsedTime;
            playbackStartTime = 0;
        }
    }


    function checkPlayTime() {
        if (totalPlayTime >= timeBoundSeconds * 1000) {
            $htmlVideo.off();
            $(window).off('beforeunload');
            incrementVideoViewCount($htmlVideo.data('videoid'));
        }
    }

    $(window).on('beforeunload', function () {
        updatePlaybackTime();
        checkPlayTime();
    });
});
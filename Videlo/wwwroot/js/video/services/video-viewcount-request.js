function incrementVideoViewCount(videoId) {
    $.ajax({
        type: "POST",
        url: '/Video/IncrementViewCount',
        data: {
            videoId
        }
    });
}
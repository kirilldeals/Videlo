function getRecommendations(videoId, channelId, pageIndex) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Video/GetRecommendations',
            data: {
                videoId,
                channelId,
                pageIndex
            },
            success: resolve,
            error: reject
        });
    });
}

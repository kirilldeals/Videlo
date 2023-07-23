function getVideoComments(videoId, byPopular) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Comment/VideoComments',
            data: {
                videoId,
                byPopular
            },
            success: resolve,
            error: reject
        });
    });
}

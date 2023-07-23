function getVideos(userId, filter, viewName) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Channel/GetVideos',
            data: {
                userId,
                filter,
                viewName
            },
            success: resolve,
            error: reject
        });
    });
}

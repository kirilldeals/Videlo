function getVideos(userId, filter, viewName, pageIndex) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Channel/GetVideos',
            data: {
                userId,
                filter,
                viewName,
                pageIndex
            },
            success: resolve,
            error: reject
        });
    });
}

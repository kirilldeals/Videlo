function getSubscriptionVideos(pageIndex) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Home/SubscriptionVideos',
            data: {
                pageIndex
            },
            success: resolve,
            error: reject
        });
    });
}

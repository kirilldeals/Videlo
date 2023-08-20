function getIndexVideos(searchQuery, pageIndex) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Home/IndexVideos',
            data: {
                searchQuery,
                pageIndex
            },
            success: resolve,
            error: reject
        });
    });
}

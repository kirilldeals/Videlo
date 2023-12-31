﻿function getVideoComments(videoId, byPopular, pageIndex) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Comment/VideoComments',
            data: {
                videoId,
                byPopular,
                pageIndex
            },
            success: resolve,
            error: reject
        });
    });
}

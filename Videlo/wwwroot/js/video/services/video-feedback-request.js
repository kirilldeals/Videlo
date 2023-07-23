function updateVideoFeedback(videoId, islike) {
    $.ajax({
        type: "POST",
        url: '/Video/UpdateFeedback',
        data: {
            videoId,
            islike
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while rating the video.');
        }
    });
}

function deleteVideoFeedback(videoId) {
    $.ajax({
        type: "POST",
        url: '/Video/DeleteFeedback',
        data: {
            videoId
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while rating the video.');
        }
    });
}
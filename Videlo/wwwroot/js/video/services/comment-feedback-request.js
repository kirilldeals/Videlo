function updateCommentFeedback(commentId, islike) {
    $.ajax({
        type: "POST",
        url: '/Comment/UpdateFeedback',
        data: {
            commentId,
            islike
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while rating the comment.');
        }
    });
}

function deleteCommentFeedback(commentId) {
    $.ajax({
        type: "POST",
        url: '/Comment/DeleteFeedback',
        data: {
            commentId
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while rating the comment.');
        }
    });
}
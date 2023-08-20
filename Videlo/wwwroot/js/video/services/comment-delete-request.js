function deleteComment(commentId) {
    $.ajax({
        type: "POST",
        url: '/Comment/Delete',
        data: {
            commentId
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while deleting the comment.');
        }
    });
}
$(document).ready(function () {
    $('#video-comments-container').on('click', 'div[name=delete-comment]', function () {
        const commentId = $(this).data('commentid');

        deleteComment(commentId);
        $(this).parents('.comment').remove();
    });
});
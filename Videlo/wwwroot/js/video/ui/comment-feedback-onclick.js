$('#video-comments-container').on('click', 'input[name^=comment-feedback]', function () {
    const thisInput = $(this);
    const thisSpan = $(`#${thisInput.data('span-id')}`);

    const anotherInput = $(`#${thisInput.data('another-input-id')}`);
    const anotherSpan = $(`#${anotherInput.data('span-id')}`);

    const updateFeedback = (input) => {
        updateCommentFeedback(input.data('commentid'), input.data('islike'));
    };
    const deleteFeedback = (input) => {
        deleteCommentFeedback(input.data('commentid'));
    };

    feedbackHandler(thisInput, thisSpan, anotherInput, anotherSpan, updateFeedback, deleteFeedback);
});
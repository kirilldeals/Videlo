$(document).ready(function () {
    const updateFeedback = (input) => {
        updateVideoFeedback(input.data('videoid'), input.data('islike'));
    };
    const deleteFeedback = (input) => {
        deleteVideoFeedback(input.data('videoid'));
    };

    $('#like').on('click', function () {
        feedbackHandler($(this), $('#likecount'), $('#dislike'), $('#dislikecount'), updateFeedback, deleteFeedback);
    });
    $('#dislike').on('click', function () {
        feedbackHandler($(this), $('#dislikecount'), $('#like'), $('#likecount'), updateFeedback, deleteFeedback);
    });
});
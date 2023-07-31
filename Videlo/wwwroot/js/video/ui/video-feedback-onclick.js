$(document).ready(function () {
    const $likeInput = $('#like');
    const $dislikeInput = $('#dislike');
    const $likeSpan = $('#likecount');
    const $dislikeSpan = $('#dislikecount');

    const updateFeedback = ($input) => {
        updateVideoFeedback($input.data('videoid'), $input.data('islike'));
    };
    const deleteFeedback = ($input) => {
        deleteVideoFeedback($input.data('videoid'));
    };

    $likeInput.on('click', function () {
        feedbackHandler(
            $(this), $likeSpan,
            $dislikeInput, $dislikeSpan,
            updateFeedback, deleteFeedback);
    });

    $dislikeInput.on('click', function () {
        feedbackHandler(
            $(this), $dislikeSpan,
            $likeInput, $likeSpan,
            updateFeedback, deleteFeedback);
    });
});
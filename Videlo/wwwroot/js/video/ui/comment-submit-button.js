$(document).ready(function () {
    $commentInput = $('#new-comment-input');
    $commentButton = $('#comment-button');

    $commentInput.on('input', function () {
        if (!$.trim($(this).val())) {
            $commentButton.prop('disabled', true);
        }
        else {
            $commentButton.prop('disabled', false);
        }
    });
});
$(document).ready(function () {
    $("#sub-button").on("click", function () {
        if (this.checked) {
            channelSubscribe($(this).data('channelid'));
        } else {
            channelUnsubscribe($(this).data('channelid'));
        }
    });
});
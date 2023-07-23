function channelSubscribe(channelId) {
    $.ajax({
        type: "POST",
        url: '/Channel/Subscribe',
        data: {
            channelId
        },
        error: function(xhr) {
            handleHttpPostError(xhr, 'An error occurred while trying to subscribe to the channel.');
        }
    });
}

function channelUnsubscribe(channelId) {
    $.ajax({
        type: "POST",
        url: '/Channel/Unsubscribe',
        data: {
            channelId
        },
        error: function (xhr) {
            handleHttpPostError(xhr, 'An error occurred while trying to unsubscribe from the channel.');
        }
    });
}
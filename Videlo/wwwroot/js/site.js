function handleHttpPostError(xhr, message) {
    if (xhr.status === 401) {
        window.location.href = '/Login';
    } else {
        alert(message);
    }
}

function infiniteScroll(callback) {
    if ($(document).height() - $(this).height() == $(this).scrollTop()) {
        callback();
    }
}

function infiniteScrollError(xhr, message) {
    console.log(xhr);
    $(window).off('scroll');
    $videoCommentsContainer.append(message);
}
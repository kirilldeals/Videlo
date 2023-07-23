function handleHttpPostError(xhr, message) {
    if (xhr.status === 401) {
        window.location.href = '/Login';
    } else {
        alert(message);
    }
}
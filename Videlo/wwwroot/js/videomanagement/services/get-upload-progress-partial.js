function uploadProgress() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/VideoManagement/UploadProgress',
            success: resolve,
            error: reject
        });
    });
}

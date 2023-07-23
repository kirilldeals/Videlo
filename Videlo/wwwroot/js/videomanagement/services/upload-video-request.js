function uploadVideo(data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: '/VideoManagement/Upload',
            processData: false,
            contentType: false,
            data: data,
            success: function (response, status, xhr) {
                resolve({
                    response,
                    status,
                    xhr
                })
            },
            error: function (error) {
                reject(error)
            },
        });
    });
}

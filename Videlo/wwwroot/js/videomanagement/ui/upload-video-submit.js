$(document).ready(function () {
    const $submit = $('#upload-submit-btn');
    const $awaitContainer = $('#await-container');

    $('#form-upload').on('submit', async function (e) {
        e.preventDefault();

        if ($(this).valid()) {
            $submit.prop('disabled', true);

            try {
                const htmlString = await uploadProgress();
                $awaitContainer.html(htmlString);
            } catch (error) {
                console.log(error);
            }

            try {
                const successResponse = await uploadVideo(new FormData(this))

                const status = successResponse.xhr.status;
                console.log(status);
                if (status === 200) {
                    window.location.href = '/Channel/Studio';
                }
            } catch (error) {
                if (error.status === 503) {
                    alert(`You already have a video in the uploading process. Please wait until the current upload is finished.`);

                    $awaitContainer.empty();
                    $submit.prop('disabled', false);
                }
            }
        }
    });
});
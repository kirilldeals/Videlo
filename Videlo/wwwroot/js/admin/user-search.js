$(document).ready(function () {
    $('#userSearch').on('input', function () {
        const query = $(this).val().toLowerCase();

        $('table tbody tr').each(function () {
            let showRow = false;

            $(this).find('.user-filter').each(function () {
                const rowData = $(this).text().toLowerCase();

                if (rowData.indexOf(query) !== -1) {
                    showRow = true;
                    return false;
                }
            });

            if (showRow) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});
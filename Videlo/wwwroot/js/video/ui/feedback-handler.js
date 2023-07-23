function feedbackHandler(thisInput, thisSpan, anotherInput, anotherSpan, updateFeedback, deleteFeedback) {
    if (thisInput.hasClass('active')) {
        thisSpan.text(Number(thisSpan.text()) - 1);
        thisInput.toggleClass('active');
        thisInput.prop("checked", false);

        deleteFeedback(thisInput);
    }
    else {
        thisSpan.text(Number(thisSpan.text()) + 1);
        thisInput.toggleClass('active');

        if (anotherInput.hasClass('active')) {
            anotherSpan.text(Number(anotherSpan.text()) - 1);
            anotherInput.toggleClass('active');
        }

        updateFeedback(thisInput);
    }
}
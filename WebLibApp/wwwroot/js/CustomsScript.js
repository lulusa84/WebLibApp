function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan' + uniqueId;
    if (isDeletedClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

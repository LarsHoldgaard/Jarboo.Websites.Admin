$(function() {
    $('.delete-btn-form').submit(function () {
        return confirm("Are you sure you want to delete this item?");
    });
});
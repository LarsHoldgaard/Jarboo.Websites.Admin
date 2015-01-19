$(function() {
    $('.delete-btn-form').submit(function () {
        return confirm("Are you sure you want to delete this item?");
    });

    $(".input-daterange").datepicker({});

    $(".dataTable").each(function() {
        var $this = $(this);

        var sortingColumn = $this.data("sorting-column");
        var sortingDirection = $this.data("sorting-direction");

        $this.dataTable({
            "order": [sortingColumn, sortingDirection]
        });
    });
});
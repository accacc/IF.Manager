$(document).on("click", "#if-paging-number-link", function (e) {

    e.preventDefault();
    var page = parseInt($(this).attr("if-paging-pagenumber"));
    var url = $(this).attr("if-paging-url");
    var updateId = $(this).attr("if-paging-update-id");
    var mergeFormData = $(this).attr("if-paging-merge-formdata");

    var data = "PageNumber=" + page + "&";

    if (mergeFormData.toLowerCase() === "true")
    {
        data += $(this).parents('form:first').serialize();
    }

    //alert(data);

    $.ajax({
        url: url,
        data: data,
        method:"get",
        success: function (data) {
            $("#" + updateId).html(data);
        }
    });
});
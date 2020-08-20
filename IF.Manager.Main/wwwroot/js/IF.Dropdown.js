$(document).ready(function () {

    $("select[if-ajax-cascadefrom]").each(function () {
        //debugger;
        var cascadeFromId = $(this).attr("if-ajax-cascadefrom");
        var cascadeUrl = $(this).attr("if-ajax-url");
        var selected = $(this).attr("if-ajax-selected");
        var id = $(this).attr("id");
        //alert(selected);
        //alert(cascadeFromId);
        //alert(cascadeUrl);
        //alert(id);

        var cascadeFromValue = $('select[id=' + cascadeFromId + ']').val();

        //alert(cascadeFromValue);
        if (cascadeFromValue > -1) {
            Cascade(id, cascadeUrl, cascadeFromId, cascadeFromValue, selected);
        }

        $(document).on("change", "select[id=" + cascadeFromId + "]", function (e) {
            cascadeFromValue = $('select[id=' + cascadeFromId + ']').val();

            Cascade(id, cascadeUrl, cascadeFromId, cascadeFromValue, selected);
        });
    });
  
});





function Cascade(id, cascadeUrl, cascadeFromId, cascadeFromValue,selected) {
    var dropdown = $('select[id=' + id + ']');
    dropdown.find('option:not(:first)').remove();
    var options = {};
    options.url = cascadeUrl + "&" + cascadeFromId + "=" + cascadeFromValue;
    //alert(options.url);
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        $.each(data, function (i, item) {
            if (selected === item.value) {
                dropdown.append('<option selected="selected" value="' + item.value + '">' + item.name + '</option>');
            }
            else {
                dropdown.append('<option  value="' + item.value + '">' + item.name + '</option>');
            }
        });
    };
    options.error = function (xhr, ajaxOptions, thrownError) {

    };

    $.ajax(options);
}


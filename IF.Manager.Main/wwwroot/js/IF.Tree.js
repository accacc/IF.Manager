function setselectedtreenode() {
    debugger;
    var id = $("#PageControlMapTree").jstree('get_selected');
    //alert(id);
    if (id == "") {
        alert("Lütfen bir eleman seçiniz.")
    }

    var data = {};

    data.TreeSelectedId = id[0];

    return data;
}


$(document).ready(function () {
    $("#PageControlMapTree").jstree({
        "plugins": ["wholerow"]
    })
});
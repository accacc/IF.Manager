$(document).on('click', 'a[data-toggle=tabcontent]', function (e) {
    alert("ok");
    e.preventDefault();
    var $this = $(this);
    $this.tab('show');
    return false;
});



$(document).on('click', 'a[data-toggle=tabajax]', function (e) {

    //alert("ok");
    var $this = $(this);
    var targ = $this.attr('data-target');

    if ($(targ).html() === "") {
        var loadurl = $this.attr('href');

        $.get(loadurl, function (data) {
            $(targ).html(data);
        });
    }

    $this.tab('show');

    return false;
});
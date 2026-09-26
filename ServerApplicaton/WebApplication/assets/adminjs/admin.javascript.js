jQuery(document).ready(function () {
    $(".ace-checkbox").find("input[type='hidden']").remove();
    $('[data-rel=tooltip]').tooltip();
    $(".loadingstatus").hide();
    $(document).ajaxStart(function () {
        //$('body').css({ 'cursor': 'wait' });
        $('.loadingstatus').show();
    }).ajaxStop(function () {
        // $('body').css({ 'cursor': 'default' });
        $(".loadingstatus").hide();
    });
});

function keepAlive() {
    $.post("/Admin/Home/KeepAlive", function (e) {
        if (e.Status == "Success") {
            setTimeout(keepAlive, 60000);
        }
    });
}
function pushNotify(txt, type) {
    $.pnotify({
        title: "Thông báo",
        text: txt,
        type: type
    });
}
function GotoPage(element) {
    var reporttype = $("#reporttype").val();
    var searchvalue = $("#searchvalue").val();
    var pageindx = $(element).attr("pageindex");
    var tBeginDate = $("#tBeginDate").val();
    var tEndDate = $("#tEndDate").val();

    var advid = $("#advid").val();
    $('#result').load('/Admin/Advertisement/List?pageindex=' +
            pageindx +
            '&reporttype=' +
            reporttype +
            '&searchvalue=' +
            searchvalue +
            '&advid=' +
            advid +
            '&fromdate=' +
            tBeginDate +
            '&todate=' +
            tEndDate
            );
};

function Search() {
    var reporttype = $("#reporttype").val();
    var searchvalue = $("#searchvalue").val();
    var pageindx = 0;
    var advid = $("#advid").val();
    var tBeginDate = $("#tBeginDate").val();
    var tEndDate = $("#tEndDate").val();

    $('#result').load('/Admin/Advertisement/List?pageindex=' +
            pageindx +
            '&reporttype=' +
            reporttype +
            '&searchvalue=' +
            searchvalue +
            '&advid=' +
            advid +
            '&fromdate=' +
            tBeginDate +
            '&todate=' +
            tEndDate
            );
};

var App = App || {};

App.Advertisment = (function ($) {
    var registerEvent = function () {

    };

    return {
        init: registerEvent
    };
}($));
$(function () {
    App.Advertisment.init();
});
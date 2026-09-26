
function GotoPage(element) {
    var SchoolCode = $("#tSchoolCode").val();
    var pageindx = $(element).attr("pageindex");
    $('#result')
        .load('/Admin/School/List?pageindex=' +
            pageindx +
            '&schoolCode=' +
            SchoolCode);
};

function Search() {
    var SchoolCode = $("#tSchoolCode").val();
    var citycode = $("#tCityCode").val();
    var districtcode = $("#tDistrictCode").val();
    var deploymentmethod = $("#tDeploymentMethod").val();
    var pageindx = 0;
    $('#result')
        .load('/Admin/School/List?pageindex=' +
            pageindx +
            '&schoolCode=' +
            SchoolCode +
            '&citycode=' +
            citycode +
            '&districtcode=' +
            districtcode + 
            '&deploymentmethod=' +
            deploymentmethod
        );
};

var App = App || {};

App.School = (function ($) {
    var addSchoolClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/School/Create';
        $.get(urlA)
            .success(function (data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function () {
                    $("#myModal .modal-content").show();
                },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function () {
                            setTimeout(function () {
                                $("#AddForm").html("");
                            },
                                100);
                        });
            });
    };
    var editSchoolClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/School/Edit/' + id;
        $.get(urlA)
            .success(function (data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function () {
                    $("#myModal .modal-content").show();
                },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function () {
                            setTimeout(function () {
                                $("#AddForm").html("");
                            },
                                100);
                        });
            });
    };
    var registerEvent = function () {
        $("#AddSchool").click(function () {
            $("#AddForm").html("");
            var urlA = '/Admin/School/Create';
            $.get(urlA).success(function (data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function () {
                    $("#myModal .modal-content").show();
                }, 100);
                $('#myModal').on('hidden.bs.modal', function () {
                    setTimeout(function () {
                        $("#AddForm").html("");
                    }, 100);
                });
            });
        });
    };

    var exportListSchoolClick = function () {
        var SchoolCode = $("#tSchoolCode").val();
        var CityCode = $("#tCityCode").val();
        var tDistrictCode = $("#tDistrictCode").val();
        var deploymentmethod = $("#tDeploymentMethod").val();
        var pageindx = 0;
        var url = '/Admin/Client/ExportSchool?schoolCode=' +
            SchoolCode +
            '&cityCode=' +
            CityCode +
            '&districtCode=' +
            tDistrictCode +
            '&deploymentmethod=' +
            deploymentmethod;
        document.location = url;
    };

    var ShowLog = function (id, element) {
        $("#AddForm").html("");
        $(element).parents('tr').removeClass('bolder');;
        var urlA = '/Admin/School/ShowLog/' + id;
        $.get(urlA).success(function (data) {
            $('#AddForm').append(data.Html);
            $("#myModal").modal('show');
            setTimeout(function () {
                $("#myModal .modal-content").show();
            }, 100);
            $('#myModal').on('hidden.bs.modal', function () {
                setTimeout(function () {
                    $("#AddForm").html("");
                }, 100);
            });
        });
    };

    var initForm = function () {
        //$('#CreateSchool').on('keyup keypress', function (e) {
        //    var code = e.keyCode || e.which;
        //    if (code == 13) {
        //        e.preventDefault();
        //        return false;
        //    }
        //});
        $('#CreateSchool').validate({
            ignore: "",
            rules: {
                Name: {
                    required: true
                },
                Code: {
                    required: true
                },
            }, messages: {
                Name: {
                    required: 'Chưa nhập tên trường'
                },
                Code: {
                    required: 'Chưa nhập mã trường'
                }
            },
            invalidHandler: function (event, validator) {

            },
            submitHandler: function (form) {
                debugger;
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);
                var postdata = {
                    SchoolId: $("#SchoolId").val(),
                    Name: $("#Name").val(),
                    Code: $("#Code").val(),
                    CityCode: $("#CityCode").val(),
                    DistrictCode: $("#DistrictCode").val(),
                    DeploymentMethod: $("#DeploymentMethod").val(),
                    NumberOfStudent: $("#NumberOfStudent").val(),
                    PrintDocument: $("#PrintDocument").val(),
                    Certificate: $("#Certificate").val(),
                };
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: $(form).attr('action'),
                    success: function (data) {
                        btnsubmit.removeAttr("disabled");
                        if (!data.Success) {
                            $("#errormsg").html(data.Message);
                            $("#errormsg").show();

                        } else {

                            $("#myModal").modal("toggle");

                            $('#result').html(data.Html);
                        }
                    },
                    error: function (data) {
                        btnsubmit.removeAttr("disabled");
                        App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            }
        });
    }
    return {
        addSchoolClick: addSchoolClick,
        editSchoolClick: editSchoolClick,
        init: registerEvent,
        initForm: initForm,
        exportListSchoolClick: exportListSchoolClick,
        ShowLog: ShowLog
    };
}($));
$(function () {
    App.School.init();
});
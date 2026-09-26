
function GotoPage(element) {
    var tclass = $("#tClass").val();
    var SchoolCode = $("#tSchoolCode").val();
    var CityCode = $("#tCityCode").val();
    var tDistrictCode = $("#tDistrictCode").val();
    var tFullName = $("#tFullName").val();
    var tapprove = $("#ddlApproved").val();
    var pageindx = $(element).attr("pageindex");
    $('#result')
        .load('/Admin/Client/ListClientLessonView?pageindex=' +
            pageindx +
            '&tclass=' +
            tclass +
            '&schoolCode=' +
            SchoolCode +
            '&cityCode=' +
            CityCode +
            '&districtCode=' +
            tDistrictCode +
            '&fullName=' +
            tFullName +
            '&approve=' +
            tapprove);
};

function Search() {
    var tclass = $("#tClass").val();
    var SchoolCode = $("#tSchoolCode").val();
    var CityCode = $("#tCityCode").val();
    var tDistrictCode = $("#tDistrictCode").val();
    var tFullName = $("#tFullName").val();
    var tapprove = $("#ddlApproved").val();
    var pageindx = 0;
    $('#result')
        .load('/Admin/Client/ListClientLessonView?pageindex=' +
            pageindx +
            '&tclass=' +
            tclass +
            '&schoolCode=' +
            SchoolCode +
            '&cityCode=' +
            CityCode +
            '&districtCode=' +
            tDistrictCode +
            '&fullName=' +
            tFullName +
            '&approve=' +
            tapprove);
};

var App = App || {};

App.Client = (function($) {
    var addClientClick = function() {
        $("#AddForm").html("");
        var urlA = '/Admin/Client/Create';
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };

    var exportListClientLessonClick = function () {
        var tclass = $("#tClass").val();
        var SchoolCode = $("#tSchoolCode").val();
        var CityCode = $("#tCityCode").val();
        var tDistrictCode = $("#tDistrictCode").val();
        var tFullName = $("#tFullName").val();
        var tapprove = $("#ddlApproved").val();
        var pageindx = 0;
        var url = '/Admin/Client/ExportClientLesson?tclass=' +
            tclass +
            '&schoolCode=' +
            SchoolCode +
            '&cityCode=' +
            CityCode +
            '&districtCode=' +
            tDistrictCode +
            '&fullName=' +
            tFullName +
            '&approve=' +
            tapprove;
        document.location = url;
    };

    var editClientClick = function(id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Client/Edit/' + id;
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };
    var showHistoryClick = function(id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Client/ClientPayment/' + id;
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };

    var showOpenLessonClick = function(id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Client/ClientLessonViewDetail/' + id;
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };
    var showLessonClick = function(id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Client/ClientLesson/' + id;
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };
    var showLessonListClientClick = function() {
        var listId = [];
        $("#sample-table-1 input[name^='client.IsSelected']:checked")
            .each(function() {
                listId.push($(this).attr("clientId"));
            });
        if (listId.length == 0) {
           App.Common.Alert("Vui lòng chọn giáo viên");
            return;
        }
        $("#AddForm").html("");
        var urlA = '/Admin/Client/ListClientLesson/';
        $.get(urlA)
            .success(function(data) {
                $('#AddForm').append(data.Html);
                $("#myModal").modal('show');
                setTimeout(function() {
                        $("#myModal .modal-content").show();
                    },
                    100);
                $('#myModal')
                    .on('hidden.bs.modal',
                        function() {
                            setTimeout(function() {
                                    $("#AddForm").html("");
                                },
                                100);
                        });
            });
    };
    var exportListClientClick = function() {
        var tclass = $("#tClass").val();
        var SchoolCode = $("#tSchoolCode").val();
        var CityCode = $("#tCityCode").val();
        var tDistrictCode = $("#tDistrictCode").val();
        var tFullName = $("#tFullName").val();
        var tapprove = $("#ddlApproved").val();
        var pageindx = 0;
        var url = '/Admin/Client/ExportClient?tclass=' +
            tclass +
            '&schoolCode=' +
            SchoolCode +
            '&cityCode=' +
            CityCode +
            '&districtCode=' +
            tDistrictCode +
            '&fullName=' +
            tFullName +
            '&approve=' +
            tapprove;
        document.location = url;
    };
    var approveClientClick = function() {
        var listId = [];
        $("#sample-table-1 input[name^='client.IsSelected']:checked")
            .each(function() {
                listId.push($(this).attr("clientId"));
            });
        if (listId.length == 0) {
           App.Common.Alert("Vui lòng chọn giáo viên");
            return;
        }
        var postdata = {
            ListId: listId,
            Approved: 1
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(postdata),
            contentType: "application/json",
            url: '/Admin/Client/Approve',
            success: function(data) {
                if (!data.Success) {
                    $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    $("#errormsg").html('Phê duyệt tài khoản thành công.');
                    Search();
                }
            },
            error: function(data) {
                $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var lockClientClick = function (locked) {
        var listId = [];
        $("#sample-table-1 input[name^='client.IsSelected']:checked")
            .each(function() {
                listId.push($(this).attr("clientId"));
            });
        if (listId.length == 0) {
           App.Common.Alert("Vui lòng chọn giáo viên");
            return;
        }
        App.Common.Confirm("Bạn muốn khóa/mở khóa những giáo viên  này ?",
            function () {
                var postdata = {
                    ListId: listId,
                    IsLock: locked
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: '/Admin/Client/LockClients',
                    success: function (data) {
                        if (!data.Success) {
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        } else {
                            if (locked) {
                                $("#errormsg").html('Khóa tài khoản thành công.');
                            } else {
                                $("#errormsg").html('Mở Khóa tài khoản thành công.');
                            }

                            Search();
                        }
                    },
                    error: function (data) {
                        $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            });
       
    };

    var deleteClients = function (locked) {
        var listId = [];
        $("#sample-table-1 input[name^='client.IsSelected']:checked")
            .each(function() {
                listId.push($(this).attr("clientId"));
            });
        if (listId.length == 0) {
            App.Common.Alert("Vui lòng chọn giáo viên");
            return;
        }
        App.Common.Confirm("Bạn muốn xóa những giáo viên  này ?",
            function () {
                var postdata = {
                    ListId: listId,
                    IsLock: locked
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: '/Admin/Client/DeleteClients',
                    success: function (data) {
                        if (!data.Success) {
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        } else {
                            if (locked) {
                                $("#errormsg").html('Khóa tài khoản thành công.');
                            } else {
                                $("#errormsg").html('Mở Khóa tài khoản thành công.');
                            }
                            Search();
                        }
                    },
                    error: function (data) {
                        $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            });
       
    };

    var deleteClient = function(id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Client/Delete',
            data: { id: id },
            success: function(data) {
                if (!data.Success) {
                    $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    $('#result').empty();
                    $('#result').html(data.Html);
                }
            },
            error: function(data) {
                $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var deleteClientClick = function(id) {
        App.Common.Confirm("Bạn muốn xóa giáo viên  này ?",
            function() {
                deleteClient(id);
            });
    };
    var registerEvent = function() {

    };
    var initForm = function() {

        $('#CreateClient')
            .validate({
                ignore: "",
                rules: {
                    UserName: {
                        required: true
                    },
                    FullName: {
                        required: true
                    },
                    PassWork: {
                        required: true,
                        minlength: 6
                    },
                    ConfirmPassword: {
                        minlength: 6,
                        equalTo: "#PassWork"
                    }
                },
                messages: {
                    UserName: {
                        required: 'Chưa nhập tên đăng nhập'
                    },
                    FullName: {
                        required: 'Chưa nhập họ tên'
                    },
                    PassWork: {
                        minlength: 'Mật khẩu tối 6-32 ký tự'
                    },
                    ConfirmPassword: {
                        equalTo: 'Xác nhận mật khẩu chưa đúng'
                    }
                },
                invalidHandler: function(event, validator) {

                },
                submitHandler: function(form) {
                    var btnsubmit = $(form).find(":submit");
                    btnsubmit.attr("disabled", true);

                    var postdata = {
                        ClientId: $("#ClientId").val(),
                        FullName: $("#FullName").val(),
                        UserName: $("#UserName").val(),
                        PassWork: $("#PassWork").val(),
                        Phone: $("#Phone").val(),
                        Email: $("#Email").val(),
                        IsLock: $("#IsLock").is(":checked"),
                        SchoolCode: $("#SchoolCode").val(),
                        Address: $("#Address").val(),
                        CityCode: $("#CityCode").val(),
                        DistrictCode: $("#DistrictCode").val(),
                        MacIp: $("#MacIp").val(),
                        FullLesson: $("#CreateClient #FullLesson").is(":checked")
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(postdata),
                        contentType: "application/json",
                        url: $(form).attr('action'),
                        success: function(data) {
                            btnsubmit.removeAttr("disabled");
                            if (!data.Success) {
                                $("#errormsg").html(data.Message);
                                $("#errormsg").show();
                            } else {

                                $("#myModal").modal("toggle");
                                $("#searchKey").val('');
                                $('#result').html(data.Html);
                            }
                        },
                        error: function(data) {
                            btnsubmit.removeAttr("disabled");
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        }
                    });
                }
            });
    };
    var initClientPayments = function() {

        $('#ClientPayment')
            .validate({
                ignore: "",
                rules: {
                    Amount: {
                        number: true,
                        required: true
                    },
                    PaymentDate: {
                        required: true
                    },
                    BeginDate: {
                        required: true
                    },
                    EndDate: {
                        required: true
                    }
                },
                messages: {
                    Amount: {
                        number: "Số tiền không hợp lệ",
                        required: 'Chưa nhập'
                    },
                    PaymentDate: {
                        required: 'Chưa nhập'
                    },
                    BeginDate: {
                        required: 'Chưa nhập'
                    },
                    EndDate: {
                        required: 'Chưa nhập'
                    },
                },
                invalidHandler: function(event, validator) {

                },
                submitHandler: function(form) {
                    var btnsubmit = $(form).find(":submit");
                    btnsubmit.attr("disabled", true);
                    var postdata = {
                        ClientId: $("#ClientPayment #ClientId").val(),
                        Amount: $("#ClientPayment #Amount").val(),
                        PaymentDate: $("#ClientPayment #PaymentDate").val(),
                        BeginDate: $("#ClientPayment #BeginDate").val(),
                        EndDate: $("#ClientPayment #EndDate").val(),
                        CreatedUserName: $("#ClientPayment #CreatedUserName").val(),
                        Description: $("#ClientPayment #Description")
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(postdata),
                        contentType: "application/json",
                        url: $(form).attr('action'),
                        success: function(data) {
                            btnsubmit.removeAttr("disabled");
                            if (!data.Success) {
                                $("#errormsg").html(data.Message);
                                $("#errormsg").show();
                            } else {

                                $("#myModal").modal("toggle");
                                //$("#searchKey").val('');
                                //$('#result').html(data.Html);
                            }
                        },
                        error: function(data) {
                            btnsubmit.removeAttr("disabled");
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        }
                    });
                }
            });
    };
    var initClientLessons = function() {

        $('#ClientLesson')
            .validate({
                ignore: "",
                submitHandler: function(form) {
                    var btnsubmit = $(form).find(":submit");
                    btnsubmit.attr("disabled", true);
                    var clientLessons = [];
                    $("#ClientLesson .IsSelected:checked")
                        .each(function() {
                            clientLessons.push({
                                IsSelected: true,
                                LessonId: $(this).attr("lessonid")
                            });
                        });
                    var postdata = {
                        ClientId: $("#ClientLesson #ClientId").val(),
                        ClientLessons: clientLessons,
                        FullLesson: $("#ClientLesson #FullLesson").is(":checked")
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(postdata),
                        contentType: "application/json",
                        url: $(form).attr('action'),
                        success: function(data) {
                            btnsubmit.removeAttr("disabled");
                            if (!data.Success) {
                                $("#errormsg").html(data.Message);
                                $("#errormsg").show();
                            } else {

                                $("#myModal").modal("toggle");
                                //$("#searchKey").val('');
                                //$('#result').html(data.Html);
                            }
                        },
                        error: function(data) {
                            btnsubmit.removeAttr("disabled");
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        }
                    });
                }
            });
    };
    var initListClientLessons = function() {

        $('#ListClientLesson')
            .validate({
                ignore: "",
                rules: {
                    EndDate: {
                        required: true
                    }
                },
                messages: {
                    EndDate: {
                        required: 'Chưa nhập'
                    },
                },
                submitHandler: function(form) {
                    var btnsubmit = $(form).find(":submit");
                    btnsubmit.attr("disabled", true);
                    var clientLessons = [];
                    $("#ListClientLesson .IsSelected:checked")
                        .each(function() {
                            clientLessons.push({
                                IsSelected: true,
                                LessonId: $(this).attr("lessonid")
                            });
                        });
                    var listId = [];
                    $("#sample-table-1 input[name^='client.IsSelected']:checked")
                        .each(function() {
                            listId.push($(this).attr("clientId"));
                        });
                    if (listId.length == 0) {
                       App.Common.Alert("Vui lòng chọn giáo viên");
                        return;
                    }
                    var postdata = {
                        ListId: listId,
                        EndDate: $("#ListClientLesson #EndDate").val(),
                        ClientLessons: clientLessons,
                        FullLesson: $("#ListClientLesson #FullLesson").is(":checked"),
                        ResetLesson: $("#ListClientLesson #ResetLesson").is(":checked")
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(postdata),
                        contentType: "application/json",
                        url: $(form).attr('action'),
                        success: function(data) {
                            btnsubmit.removeAttr("disabled");
                            if (!data.Success) {
                                $("#errormsg").html(data.Message);
                                $("#errormsg").show();
                            } else {

                                $("#myModal").modal("toggle");
                                //$("#searchKey").val('');
                                //$('#result').html(data.Html);
                            }
                        },
                        error: function(data) {
                            btnsubmit.removeAttr("disabled");
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        }
                    });
                }
            });
    };
    return {
        addClientClick: addClientClick,
        editClientClick: editClientClick,
        deleteClientClick: deleteClientClick,
        deleteClient: deleteClient,
        init: registerEvent,
        initForm: initForm,
        showHistoryClick: showHistoryClick,
        showLessonClick: showLessonClick,
        deleteClients: deleteClients,
        lockClientClick: lockClientClick,
        showLessonListClientClick: showLessonListClientClick,
        exportListClientClick: exportListClientClick,
        exportListClientLessonClick: exportListClientLessonClick,
        showOpenLessonClick: showOpenLessonClick,
        approveClientClick: approveClientClick,
        initClientPayments: initClientPayments,
        initClientLessons: initClientLessons,
        initListClientLessons: initListClientLessons
    };
}($));
$(function() {
    App.Client.init();
});
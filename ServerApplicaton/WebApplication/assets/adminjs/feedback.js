
function GotoPage(element) {

    var tId = $("#tId").val();
    if (tId == '') tId = 0;
    var tFullName = $("#tFullName").val();
    var tBeginDate = $("#tBeginDate").val();
    var tEndDate = $("#tEndDate").val();
    var tClass = $("#tClass").val();
    var tSchoolCode = $("#tSchoolCode").val();
    var tLessonCode = $("#tLessonCode").val();
    var viewed = $("#ddlViewed").val();
    var pageindx = $(element).attr("pageindex");

    $('#result')
        .load('/Admin/Feedback/List?pageindex=' +
            pageindx +
            '&clientid=' +
            tId +
            '&fromdate=' +
            tBeginDate +
            '&todate=' +
            tEndDate +
            '&fullName=' +
            tFullName +
            '&tClass=' +
            tClass +
            '&tSchoolCode=' +
            tSchoolCode +
            '&tLessonCode=' +
            tLessonCode +
            '&viewed=' +
            viewed);
};

function Search() {

    var tId = $("#tId").val();
    if (tId == '') tId = 0;
    var tFullName = $("#tFullName").val();
    var tBeginDate = $("#tBeginDate").val();
    var tEndDate = $("#tEndDate").val();
    var pageindx = 0;
    var tClass = $("#tClass").val();
    var tSchoolCode = $("#tSchoolCode").val();
    var tLessonCode = $("#tLessonCode").val();
    var viewed = $("#ddlViewed").val();

    $('#result')
        .load('/Admin/Feedback/List?pageindex=' +
            pageindx +
            '&clientid=' +
            tId +
            '&fromdate=' +
            tBeginDate +
            '&todate=' +
            tEndDate +
            '&fullName=' +
            tFullName +
            '&tClass=' +
            tClass +
            '&tSchoolCode=' +
            tSchoolCode +
            '&tLessonCode=' +
            tLessonCode +
            '&viewed=' +
            viewed)
    ;
};

var App = App || {};

App.Feedback = (function ($) {
    var addFeedbackClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Feedback/Create';
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
    var editFeedbackClick = function (id, element) {
        $("#AddForm").html("");
        $(element).parents('tr').removeClass('bolder');;
        var urlA = '/Admin/Feedback/Edit/' + id;
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

    var deleteFeedback = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Feedback/Delete',
            data: { id: id },
            success: function (data) {
                if (!data.Success) {
                    $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    $('#result').empty();
                    $('#result').html(data.Html);
                }
            },
            error: function (data) {
                $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var deleteFeedbackClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa môn học này ?", function () {
            deleteFeedback(id);
        });
    };

    var showAnswerClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Feedback/ShowAnswer/' + id;
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

    var initFeedbackAnswer = function () {
        $('#ShowAnswer')
            .validate({
                ignore: "",
                rules: {
                    Answer: {
                        required: true
                    }
                },
                messages: {
                    Answer: {
                        required: 'Chưa nhập nội dung trả lời'
                    }
                },
                invalidHandler: function (event, validator) {

                },
                submitHandler: function (form) {
                    var btnsubmit = $(form).find(":submit");
                    btnsubmit.attr("disabled", true);
                    var postdata = {
                        FeedbackId: $("#FeedbackId").val(),
                        Answer: $("#Answer").val(),
                        UserId: $("#UserId").val(),
                        UserName: $("#UserName").val()
                    }
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
                                //$("#searchKey").val('');
                                //$('#result').html(data.Html);
                            }
                        },
                        error: function (data) {
                            btnsubmit.removeAttr("disabled");
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        }
                    });
                }
            });
    };

    var finishFeedbackClick = function (viewed) {
        var listId = [];
        $("#sample-table-1 input[name^='feedback.Viewed']:checked")
            .each(function () {
                listId.push($(this).attr("feedbackid"));
            });
        if (listId.length == 0) {
            App.Common.Alert("Vui lòng chọn ý kiến khách hàng");
            return;
        }
        App.Common.Confirm("Bạn muốn chuyển trạng thái hoàn thành hỗ trợ cho ý kiến khách hàng này ?",
            function () {
                var postdata = {
                    ListId: listId,
                    Viewed: viewed
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: '/Admin/Feedback/FinishFeedback',
                    success: function (data) {
                        if (!data.Success) {
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        } else {
                            if (viewed) {
                                $("#errormsg").html('Hoàn thành hỗ trợ thành công.');
                            } else {
                                $("#errormsg").html('Chuyển trạng thái thành công.');
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
    //exportFeedbakClick
    var exportFeedbakClick = function () {
        var cLientId = $("#tId").val();
        var fromdate = $("#tBeginDate").val();
        var todate = $("#tEndDate").val();
        var fullName = $("#tFullName").val();
        var tClass = $("#tClass").val();
        var tSchoolCode = $("#tSchoolCode").val();
        var tLessonCode = $("#tLessonCode").val();
        var viewed = $("#ddlViewed").val();
        var pageindx = 0;
        var url = '/Admin/Feedback/ExportFeedback?cLientId=' +
            cLientId +
            '&fromdate=' +
            fromdate +
            '&todate=' +
            todate +
            '&fullName=' +
            fullName +
            '&tClass=' +
            tClass +
            '&tSchoolCode=' +
            tSchoolCode +
            '&tLessonCode=' +
            tLessonCode +
            '&viewed=' +
            viewed
        ;
        document.location = url;
    };

    var registerEvent = function () {

    };
    var initForm = function () {

        $('#CreateFeedback').validate({
            ignore: "",
            rules: {
                Name: {
                    required: true
                }, SubjectId: {
                    required: true
                }
            }, messages: {
                Name: {
                    required: 'Chưa nhập tên lớp học'
                }, SubjectId: {
                    required: 'Chưa chọn môn học'
                }
            },
            invalidHandler: function (event, validator) {

            },
            submitHandler: function (form) {
                debugger
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);

                var postdata = {
                    FeedbackId: $("#FeedbackId").val(),
                    Code: $("#Code").val(),
                    Name: $("#Name").val(),
                    Description: $("#Description").val(),
                    Order: $("#Order").val(),
                    SubjectId: $("#SubjectId").val(),
                    IsLocked: $("#IsLocked").is(":checked")
                }
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
                            $("#searchKey").val('');
                            $('#result').html(data.Html);
                        }
                    },
                    error: function (data) {
                        btnsubmit.removeAttr("disabled");
                        $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            }
        });
    };
    return {
        addFeedbackClick: addFeedbackClick,
        editFeedbackClick: editFeedbackClick,
        deleteFeedbackClick: deleteFeedbackClick,
        deleteFeedback: deleteFeedback,
        exportFeedbakClick: exportFeedbakClick,
        init: registerEvent,
        initForm: initForm,
        showAnswerClick: showAnswerClick,
        initFeedbackAnswer: initFeedbackAnswer,
        finishFeedbackClick: finishFeedbackClick
    };
}($));
$(function () {
    App.Feedback.init();
});

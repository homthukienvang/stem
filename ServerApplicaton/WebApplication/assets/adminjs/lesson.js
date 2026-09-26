
function GotoPage(element) {
    var pageindx = $(element).attr("pageindex");
    var keyword = $("#keyword").val();
    var tSubjectId = $("#tSubjectId").val();
    var tRoomId = $("#tRoomId").val();
    $('#result')
        .load('/Admin/Lesson/List?pageindex=' +
            pageindx +
            '&s=' +
            tSubjectId +
            '&r=' +
            tRoomId +
            '&keyword=' +
            keyword);
};

function Search() {
    var keyword = $("#keyword").val();
    var pageindx = 0;
    var tSubjectId = $("#tSubjectId").val();
    var tRoomId = $("#tRoomId").val();
    $('#result')
        .load('/Admin/Lesson/List?pageindex=' +
            pageindx +
            '&s=' +
            tSubjectId +
            '&r=' +
            tRoomId +
            '&keyword=' +
            keyword);
};


var App = App || {};

App.Lesson = (function ($) {
    var addLessonClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Lesson/Create';
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

    var exportLessonClick = function () {
        var tlessonname = $("#keyword").val();
        var ddsubject = $("#tSubjectId").val();
        var ddroom = $("#tRoomId").val();
        
        var pageindx = 0;
        var url = '/Admin/Lesson/ExportLesson?tlessonname=' +
            tlessonname +
            '&ddsubject=' +
            ddsubject +
            '&ddroom=' +
            ddroom;
        document.location = url;
    };

    var editLessonClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Lesson/Edit/' + id;
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

    var deleteLesson = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Lesson/Delete',
            data: { id: id },
            success: function (data) {
                if (!data.Success) {
                    $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    var index = $("#pagination-demo li[class*='active']").attr("pageindex");
                    $('#result').load('/Admin/Lesson/List?pageindex=' + index);
                }
            },
            error: function (data) {
                $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var lockLesson = function (id, islock) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Lesson/Lock',
            data: { id: id, islock: islock },
            success: function (data) {
                if (!data.Success) {
                    App.Common.Alert.html('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    $("#errormsg").html('Thành công.');
                    // $('#result').empty();
                    // $('#result').html(data.Html);
                    var index = $("#pagination-demo li[class*='active']").attr("pageindex");
                    $('#result').load('/Admin/Lesson/List?pageindex=' + index);
                }
            },
            error: function (data) {
                App.Common.Alert.html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var deleteLessonClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa bài học này ?", function () {
            deleteLesson(id);
        });
    };
    var registerEvent = function () {

    };
    var initForm = function () {
        $("#SubjectId").change(function () {
            var selectedItem = $(this).val();

            var ddlroom = $("#RoomId");
            if (selectedItem == '') {
                ddlroom.html(''); return;
            }
            var urlA = '/Lesson/GetRoom/' + selectedItem;
            $.get(urlA).success(function (data) {

                $.each(data.Data, function (id, option) {
                    ddlroom.append($('<option></option>').val(option.RoomId).html(option.Name));
                });
            });
        });
        $('#CreateLesson').validate({
            ignore: "",
            rules: {
                Name: {
                    required: true
                },
                SubjectId: {
                    required: true
                },
                RoomId: {
                    required: true
                }
            },
            messages: {
                Name: {
                    required: 'Chưa nhập tên bài học'
                },
                SubjectId: {
                    required: 'Chưa chọn môn học'
                },
                RoomId: {
                    required: 'Chưa nhập lớp học'
                }
            },
            invalidHandler: function (event, validator) {

            },
            submitHandler: function (form) {
                debugger
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);

                var postdata = {
                    LessonId: $("#LessonId").val(),
                    Code: $("#Code").val(),
                    Name: $("#Name").val(),
                    NormalImage: $("#NormalImage").val(),
                    Description: $("#Description").val(),
                    Order: $("#Order").val(),
                    RoomId: $("#RoomId").val(),
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
    var uploadDocumentClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Lesson/UploadDocument/' + id;
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
    var uploadDocument = function (id) {

        var documents = [];
        var ok = true;
        $(".iFileGroup").each(function () {
            if ($(this).find(".DocName").val() == '') {
                ok = false;
            }
            documents.push({
                DocumentId: $(this).find(".DocumentId").val(),
                DocumentName: $(this).find(".DocName").val(),
                FileName: $(this).find(".FileName").val()
            });
        });
        if (!ok) {
            alert("Chưa nhập tên tài liệu");
            return;
        }
        var postdata = {
            GuideFile: $("#GuideFile").val(),
            LessonId: $("#LessonId").val(),
            Documents: documents
        }
        debugger;
        $.ajax({
            type: "POST",
            data: JSON.stringify(postdata),
            contentType: "application/json",
            url: "/Admin/Lesson/UploadDocument",
            success: function (data) {
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
                $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };

    var deleteLessons = function () {
        var listId = [];
        $("#sample-table-1 input[name^='lesson.IsSelected']:checked")
            .each(function () {
                listId.push($(this).attr("lessonid"));
            });
        if (listId.length == 0) {
            App.Common.Alert("Vui lòng chọn bài học");
            return;
        }
        App.Common.Confirm("Bạn muốn xóa những bài học này ?",
            function () {
                var postdata = {
                    ListId: listId
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: '/Admin/Lesson/DeleteLessons',
                    success: function (data) {
                        if (!data.Success) {
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        } else {

                            $("#errormsg").html('Xóa bài học thành công.');

                            Search();
                        }
                    },
                    error: function (data) {
                        $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            });

    };
    var lockLessonClick = function (locked) {
        var listId = [];
        $("#sample-table-1 input[name^='lesson.IsSelected']:checked")
            .each(function () {
                listId.push($(this).attr("lessonid"));
            });
        if (listId.length == 0) {
            App.Common.Alert("Vui lòng chọn bài học");
            return;
        }
        App.Common.Confirm("Bạn muốn khóa/mở khóa những bài học này ?",
            function () {
                var postdata = {
                    ListId: listId,
                    IsLocked: locked
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(postdata),
                    contentType: "application/json",
                    url: '/Admin/Lesson/LockLessons',
                    success: function (data) {
                        if (!data.Success) {
                            $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                        } else {
                            if (locked) {
                                $("#errormsg").html('Khóa bài học thành công.');
                            } else {
                                $("#errormsg").html('Mở Khóa bài học  thành công.');
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
    return {
        addLessonClick: addLessonClick,
        editLessonClick: editLessonClick,
        deleteLessonClick: deleteLessonClick,
        deleteLesson: deleteLesson,
        exportLessonClick: exportLessonClick,
        init: registerEvent,
        initForm: initForm,
        uploadDocumentClick: uploadDocumentClick,
        uploadDocument: uploadDocument,
        lockLesson: lockLesson,
        deleteLessons: deleteLessons,
        lockLessonClick: lockLessonClick
    };
}($));
$(function () {
    App.Lesson.init();
});


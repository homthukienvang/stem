
function GotoPage(element) {

    $('#result').load('/Admin/Subject/List?pageindex=' + $(element).attr("pageindex"));
};

var App = App || {};

App.Subject = (function ($) {
    var addSubjectClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Subject/Create';
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
    var editSubjectClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Subject/Edit/' + id;
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

    var deleteSubject = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Subject/Delete',
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
    var deleteSubjectClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa môn học này ?", function () {
            deleteSubject(id);
        });
    };
    var registerEvent = function () {
          
    };        
    var initForm = function () {
         
        $('#CreateSubject').validate({
            ignore: "",
            rules: {
                Name: {
                    required: true
                } 
            }, messages: {
               Name: {
                    required: 'Chưa nhập tên môn học'
                } 
            },
            invalidHandler: function (event, validator) {

            },
            submitHandler: function (form) {
                debugger 
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);
                 
                var postdata = {
                    SubjectId: $("#SubjectId").val(),
                    Code: $("#Code").val(),
                    Name: $("#Name").val(),
                    Description: $("#Description").val(),
                    Order: $("#Order").val(),  
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
        addSubjectClick: addSubjectClick,
        editSubjectClick: editSubjectClick,
        deleteSubjectClick: deleteSubjectClick,
        deleteSubject: deleteSubject,
        init: registerEvent,
        initForm: initForm,
    };
}($));
$(function () {
    App.Subject.init();
});

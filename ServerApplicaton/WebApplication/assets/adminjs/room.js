
function GotoPage(element) {

    $('#result').load('/Admin/Room/List?pageindex=' + $(element).attr("pageindex"));
};

var App = App || {};

App.Room = (function ($) {
    var addRoomClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Room/Create';
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
    var editRoomClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Room/Edit/' + id;
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

    var deleteRoom = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Room/Delete',
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
    var deleteRoomClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa môn học này ?", function () {
            deleteRoom(id);
        });
    };
    var registerEvent = function () {
          
    };        
    var initForm = function () {
         
        $('#CreateRoom').validate({
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
                    RoomId: $("#RoomId").val(),
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
        addRoomClick: addRoomClick,
        editRoomClick: editRoomClick,
        deleteRoomClick: deleteRoomClick,
        deleteRoom: deleteRoom,
        init: registerEvent,
        initForm: initForm,
    };
}($));
$(function () {
    App.Room.init();
});

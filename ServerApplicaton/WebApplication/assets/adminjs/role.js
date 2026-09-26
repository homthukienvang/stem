
function GotoPage(element) {
    $('#result').load('/Admin/Role/List?pageindex=' + $(element).attr("pageindex") + '&RoleName=' + $('input#searchKey').val());
};

var App = App || {};

App.Role = (function ($) {
    var addRoleClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Role/Create';
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
   
    var editRoleClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Role/Edit/' + id;
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
   
    var deleteRoleClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa bản ghi này ?", function() {
            deleteRole(id);
        });
    };
    var deleteRole = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Role/Delete',
            data: { id: id },
            success: function (data) {
                if (!data.Success) {
                    App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại..');
                } else {                                
                    $("#RoleForm").empty();
                    $('#result').empty();
                    $('#result').html(data.Html);
                }
            },
            error: function (data) {
                App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại..');
            }
        });
    };
     
    var registerEvent = function() {
        $("#AddRole").click(function (){
            $("#AddForm").html("");
            var urlA = '/Admin/Role/Create';
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
    var initForm = function () {
        
        $('#CreateRole').on('keyup keypress', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                e.preventDefault();
                return false;
            }
        });
        $('#CreateRole').validate({
            ignore: "",
            rules: {
                RoleName: {
                    required: true
                } 
            }, messages: {
                RoleName: {
                    required: 'Chưa nhập tên vai trò'
                }
            },
            invalidHandler: function (event, validator) {
              
            },
            submitHandler: function (form) {
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);
                var rights = [];
                $(".rightselect:checked").each(function() {
                    rights.push({
                        RightId: $(this).attr("rightId")
                    });
                });
                var postdata = {
                    RoleName: $("#RoleName").val(),
                    RoleDesc: $("#RoleDesc").val(),
                    RoleActivated:  $("#RoleActivated").is(":checked"),
                    RoleId: $("#RoleId").val(),
                    Rights: rights
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
                        $("#errormsg").html('Có lỗi xảy ra. Vui lòng thử lại.');
                    }
                });
            }
        });
    };
    return {
        addRoleClick: addRoleClick,
        editRoleClick: editRoleClick,
        deleteRoleClick: deleteRoleClick,
        deleteRole: deleteRole,
        init: registerEvent,
        initForm: initForm
    };
}($));
$(function() {
    App.Role.init();
});

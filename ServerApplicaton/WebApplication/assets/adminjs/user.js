
function GotoPage(element) {
    $('#result').load('/Admin/User/List?pageindex=' + $(element).attr("pageindex") + '&UserName=' + $('input#searchKey').val());
};

var App = App || {};

App.User = (function ($) {
    var addUserClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/User/Create';
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
    var resetPassClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/User/ResetPassword/' + id;
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
    var editUserClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/User/Edit/' + id;
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
   
    var deleteUserClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa bản ghi này ?", function() {
            deleteUser(id);
        });
    };
    var deleteUser = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/User/Delete',
            data: { id: id },
            success: function (data) {
                if (!data.Success) {
                    App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại..');
                } else {                                
                    $("#UserForm").empty();
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
        $("#AddUser").click(function (){
            $("#AddForm").html("");
            var urlA = '/Admin/User/Create';
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
        
        $('#CreateUser').on('keyup keypress', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                e.preventDefault();
                return false;
            }
        });
        $('#CreateUser').validate({
            ignore: "",
            rules: {
                UserName: {
                    required: true
                },
                DisplayName: {
                    required: true
                } ,
                Password: {
                    required: true,
                    minlength: 6
                },
                ConfirmPassword: {
                    minlength: 6,
                    equalTo: "#Password"
                },
//                CityCode: {
//                    required: true
//                } 
                
            }, messages: {
                DisplayName: {
                    required: 'Chưa nhập họ tên'
                },
                UserName: {
                    required: 'Chưa nhập tên đăng nhập'
                }, 
                Password: {
                    minlength: 'Mật khẩu tối 6-32 ký tự'
                },
                ConfirmPassword: {
                    equalTo: 'Xác nhận mật khẩu chưa đúng'
                }
            },
            invalidHandler: function (event, validator) {
              
            },
            submitHandler: function (form) {
                var btnsubmit = $(form).find(":submit");
                btnsubmit.attr("disabled", true);
                var roles = [];
                $(".rightselect:checked").each(function() {
                    roles.push({
                        RoleId: $(this).attr("roleId")
                    });
                });
                var postdata = {
                    UserName: $("#UserName").val(),
                    DisplayName: $("#DisplayName").val(),
                    UserActivated: $("#UserActivated").is(":checked"),
                    UserId: $("#UserId").val(),
                    Password: $("#Password").val(),
                    Roles: roles,
                    CityCode: $("#CityCode").val(),
                    DistrictCode: $("#DistrictCode").val()
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
    };
    return {
        addUserClick: addUserClick,
        editUserClick: editUserClick,
        resetPassClick:resetPassClick,
        deleteUserClick: deleteUserClick,
        deleteUser: deleteUser,
        init: registerEvent,
        initForm: initForm
    };
}($));
$(function() {
    App.User.init();
});

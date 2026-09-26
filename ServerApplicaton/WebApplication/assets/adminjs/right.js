
function GotoPage(element) {
    $('#result').load('/Admin/Right/List?pageindex=' + $(element).attr("pageindex") + '&RightName=' + $('input#searchKey').val());
};

var App = App || {};

App.Right = (function ($) {
    var addRightClick = function () {
        $("#AddForm").html("");
        var urlA = '/Admin/Right/Create';
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
    var createRight = function () {
        var form = $('#CreateRight');
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: form.attr('action'),
            data: form.serialize(),
            success: function (data) {
                if (!data.Success) {
                    App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại.');
                } else {
                    $("#myModal").modal("toggle");
                    $('#result').load('/Admin/Right/List?pageindex=' + 0);
                }
            },
            error: function (data) {
                App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    };
    var editRightClick = function (id) {
        $("#AddForm").html("");
        var urlA = '/Admin/Right/Edit/' + id;
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
    var editRight = function () {
        var form = $('#EditRight');
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: form.attr('action'),
            data: form.serialize(),
            success: function (result) {
                if (!result.Success) {
                    App.Common.Alert(result.Message);
                } else {
                    $("#myModal").modal("toggle");
                    $('#result').html(result.Html);
                }
            },
            error: function (result) {
                App.Common.Alert(message);
            }
        });
    };
   
    var deleteRightClick = function (id) {
        App.Common.Confirm("Bạn muốn xóa bản ghi này ?", function() {
            deleteRight(id);
        });
    };
    var deleteRight = function (id) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/Admin/Right/Delete',
            data: { id: id },
            success: function (data) {
                if (!data.Success) {
                    App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại..');
                } else {                                
                    $("#myModal").modal("toggle");
                    $('#result').html(data.Html);
                }
            },
            error: function (data) {
                App.Common.Alert('Có lỗi xảy ra. Vui lòng thử lại..');
            }
        });
    };
     
    var registerEvent = function() {
        $("#AddRight").click(function (){
            $("#AddForm").html("");
            var urlA = '/Admin/Right/Create';
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
    var registerSelectLayoutEvent = function() {
        $("#RightId").change(function () {
            var pId = $("#RightId").val();
            if (pId == '') {
                $('#result').empty();
                return false;
            }
            var urlA = '/Admin/Right/ListLayoutSelected/' + pId;
            $.get(urlA).success(function (data) {
                $('#result').empty();
                $('#result').html(data.Html);
            });
        });
    };
    var assignLayoutToRight = function() {
        var form = $('#SelectLayout');
            $.ajax({
                cache: false,
                async: true,
                type: "POST",
                url: form.attr('action'),
                data: form.serialize(),
                success: function (result) {
                    if (!result.Success) {
                        App.Common.Alert(result.Message);
                    } else {                            
                        $('#result').html(result.Html);
                    }
                },
                error: function (result) {
                    App.Common.Alert(message);
                }
            });
    };
    return {
        addRightClick: addRightClick,
        createRight: createRight,
        editRightClick: editRightClick,
        editRight: editRight,
        deleteRightClick: deleteRightClick,
        deleteRight: deleteRight,
        AssignLayoutToRight:assignLayoutToRight,
        init:registerEvent  ,
        initSelectLayOut:registerSelectLayoutEvent
    };
}($));
$(function() {
    App.Right.init();
});

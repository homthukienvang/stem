var App = App || {};
App.Common = {
    Alert: function (msg) {
        bootbox.alert(msg, function () {
        });
    },
    Confirm: function (msg, action) {
        bootbox.confirm(msg, function (result) {
            if (result) {
                action();
            }
        });
    },
    UploadFile: function (fileuploadGroup, complete) {
        $(fileuploadGroup + ' .fileupload').fileupload({
            dataType: 'json',
            url: '/Admin/Home/UploadFiles',
            autoUpload: true,
            done: function (e, data) {
                $(fileuploadGroup + ' .progress').hide();
                $(fileuploadGroup + ' .progress .progress-bar').css('width', '0%');
                complete(data);
            }
        }).on('fileuploadprogressall', function (e, data) {
            $(fileuploadGroup + ' .progress').show();
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $(fileuploadGroup + ' .progress .progress-bar').css('width', progress + '%');
        });
    },
    UploadImage: function (fileuploadGroup, complete) {
        $(fileuploadGroup).fileupload({
            dataType: 'json',
            url: '/Admin/Home/UploadImages',
            autoUpload: true,
            done: function (e, data) {
                $(fileuploadGroup + ' .progress').hide();
                $(fileuploadGroup + ' .progress .progress-bar').css('width', '0%');
                complete(data);
            }
        }).on('fileuploadprogressall', function (e, data) {
            $(fileuploadGroup + ' .progress').show();
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $(fileuploadGroup + ' .progress .progress-bar').css('width', progress + '%');
        });
    }
}
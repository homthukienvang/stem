/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'vi';
    //config.uiColor = '#AADC6E';
    config.extraPlugins = 'sourcedialog';
    config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?type=Images';
    config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.toolbar_Full = [
   { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source',  'Preview', 'Print', 'Templates', 'document'] },
   // On the basic preset, clipboard and undo is handled by keyboard.
   // Uncomment the following line to enable them on the toolbar as well.
   // { name: 'clipboard',   groups: [ 'clipboard', 'undo' ], items: [ 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Undo', 'Redo' ] },
   //{ name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find', 'Replace', 'SelectAll', 'Scayt'] },
   { name: 'insert', items: ['CreatePlaceholder', 'Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe', 'InsertPre'] },
  // { name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
 //  '/',
   { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'] },
   { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'], items: ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'BidiLtr', 'BidiRtl'] },
   { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
   '/',
   { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
   { name: 'colors', items: ['TextColor', 'BGColor'] },
   { name: 'tools', items: ['UIColor', 'Maximize', 'ShowBlocks'] },
   //{ name: 'about', items: ['About'] }
    ];

    config.toolbar = "Full";
};
 
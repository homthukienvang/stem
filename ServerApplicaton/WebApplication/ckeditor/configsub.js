/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbar = [
        { name: 'styles', items: ['Source', '-', 'Bold', 'Italic', 'Underline', '-', 'Link', 'Unlink', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
    ];
    // utf-8
    config.entities = false;
    config.basicEntities = false;
    config.language = 'vi';
    config.uiColor = '#fdfdfd';
    config.height = '135px';
    config.fillEmptyBlocks = false;
    config.removePlugins = 'wsc,scayt,image,exportpdf';

    config.forcePasteAsPlainText = true;
    config.pasteFromWordRemoveStyles = true;
    config.pasteFromWordRemoveFontStyles = true;
    config.disallowedContent = 'script; p(style); br';
};
 
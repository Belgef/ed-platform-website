"use strict";

var editor = ace.edit("editor");
editor.setTheme("ace/theme/crimson_editor");
editor.session.setMode("ace/mode/python");
document.getElementById('editor').style.fontSize = '12px';

function sendCode() {
    document.codeForm.code.value = editor.getValue();
    document.codeForm.submit();
}


var oDoc;

function initDoc() {
    oDoc = document.getElementById("textBox");
    if (document.lessonEditForm.switchMode.checked) { setDocMode(true); }
    document.getElementById('textBox').innerHTML = document.getElementById('text-input').value;
}

function formatDoc(sCmd, sValue) {
    if (validateMode()) { document.execCommand(sCmd, false, sValue); oDoc.focus(); }
}

function validateMode() {
    if (!document.lessonEditForm.switchMode.checked) { return true; }
    alert("Uncheck \"Show HTML\".");
    oDoc.focus();
    return false;
}

function setDocMode(bToSource) {
    var oContent;
    if (bToSource) {
        oContent = document.createTextNode(oDoc.innerHTML);
        oDoc.innerHTML = "";
        var oPre = document.createElement("pre");
        oDoc.contentEditable = false;
        oPre.id = "sourceText";
        oPre.contentEditable = true;
        oPre.appendChild(oContent);
        oDoc.appendChild(oPre);
        document.execCommand("defaultParagraphSeparator", false, "div");
    } else {
        if (document.all) {
            oDoc.innerHTML = oDoc.innerText;
        } else {
            oContent = document.createRange();
            oContent.selectNodeContents(oDoc.firstChild);
            oDoc.innerHTML = oContent.toString();
        }
        oDoc.contentEditable = true;
    }
    oDoc.focus();
}

function save() {
    document.getElementById('text-input').setAttribute('value', document.getElementById('textBox').innerHTML);
}

document.addEventListener("DOMContentLoaded", initDoc);
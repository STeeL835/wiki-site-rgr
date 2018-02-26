$(document).ready(function() {
    function updateSize() {
        var file = document.getElementById("uploadInput").files[0],
            str = "";
        if (!file.type.match("image.*")) {
            str = "Не-не-не, только картинки";
            document.getElementById('outputImage').src = null;
        }
        document.getElementById("e-fileinfo").innerHTML = [
            str
        ].join("<br>");
    }
    document.getElementById("uploadInput").addEventListener("change", updateSize);
});
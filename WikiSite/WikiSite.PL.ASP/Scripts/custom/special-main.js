$(document).ready(function () {
    function updateSize() {
        var file = document.getElementById("uploadInput").files[0],
            str = "";
        if (!file.type.match("image.*")) {
            str = "Не-не-не, только картинки";
            document.getElementById('outputImage').src = "http://project-wikisite-835.azurewebsites.net/Image/Get?id=00000000-0000-0000-0000-000000000000";
        }
        document.getElementById("exepthionInfo").innerHTML = [str].join("<br>");
    }

    function handleFileSelect(evt) {
        var files = evt.target.files,
            file = file[0];
        if (!file.type.match("image.*")) {
            str = "Не-не-не, только картинки";
            //document.getElementById('outputImage').src = "/Image/Get?id=00000000-0000-0000-0000-000000000000";
            document.getElementById('exepthionInfo').innerHTML = [str].join('');
            files.file[0] = null;
        } else {
            var reader = new FileReader();
            reader.onload = (function (theFile) {
                return function (e) {
                    document.getElementById('outputImage').src = e.target.result;
                };
            })(f);
            reader.readAsDataURL(file);
        }
    }
    document.getElementById('uploadInput').addEventListener('change', handleFileSelect, false);
});
$(document).ready(function () { 
    var outputImage = document.getElementById('outputImage'), 
        uploadInput = document.getElementById('uploadInput'), 
        exepthionInfo = document.getElementById('exepthionInfo'), 
        imageHeader = document.getElementById('imageHeader'),
        submitButton = document.getElementById('submitButton'); 
    function handleFileSelect(evt) { 
        var files = evt.target.files, 
            file = files[0]; 
        if (!file.type.match("image/*")) { 
            str = "Не-не-не, только картинки"; 
            exepthionInfo.innerHTML = [str].join('');
            //outputImage.src = "/Image/Get?id=00000000-0000-0000-0000-000000000000";  
            imageHeader.classList.remove('text-success');
            imageHeader.classList.add('text-danger'); 
            submitButton.setAttribute('disabled', true);
        } else { 
            var reader = new FileReader(); 
            reader.onload = (function (theFile) { 
                return function (e) { 
                    outputImage.src = e.target.result; 
                }; 
            })(file); 
            reader.readAsDataURL(file); 
            exepthionInfo.innerHTML = [].join(''); 
            imageHeader.classList.add('text-success');
            imageHeader.classList.remove('text-danger');
            submitButton.removeAttribute('disabled');
        } 
    } 
    uploadInput.addEventListener('change', handleFileSelect, false); 
});
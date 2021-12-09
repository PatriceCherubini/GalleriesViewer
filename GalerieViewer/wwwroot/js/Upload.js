var actualBtn = document.getElementById('actual-btn-upload');
var fileChosen = document.getElementById('upload-file-txt');

actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
});

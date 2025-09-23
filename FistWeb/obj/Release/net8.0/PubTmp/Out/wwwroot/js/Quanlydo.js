document.querySelectorAll('.image-upload-area').forEach(area => {
    area.addEventListener('click', function (e) {
        // Nếu click đúng vào input thì không làm gì (tránh trigger lại)
        if (e.target.tagName.toLowerCase() === 'input') return;

        const input = this.querySelector('.file-input');
        if (input) {
            input.value = ''; // reset file input
            input.click();
        }
    });
});

function previewImage(input) {
    const file = input.files[0];
    if (!file) return; // Người dùng bấm hủy

    const container = input.closest('.image-upload-area');
    const preview = container.querySelector('.image-preview');
    const text = container.querySelector('.image-upload-text');
    const icon = container.querySelector('.image-upload-icon');

    const reader = new FileReader();
    reader.onload = function (e) {
        preview.src = e.target.result;
        preview.style.display = 'block';
        text.style.display = 'none';
        icon.style.display = 'none';
    };

    reader.readAsDataURL(file);
}


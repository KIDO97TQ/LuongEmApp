document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        // đổi active trên nút
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        this.classList.add('active');

        // đổi nội dung hiển thị
        document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
        document.getElementById(this.dataset.tab).classList.add('active');
    });
});
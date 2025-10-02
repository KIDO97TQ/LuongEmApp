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

function toggleOptionMenu(btn) {
    const menu = btn.parentElement.querySelector('.option-menu');
    menu.classList.toggle('show');

    // Đóng menu nếu click bên ngoài
    document.addEventListener('click', function handler(e) {
        if (!btn.parentElement.contains(e.target)) {
            menu.classList.remove('show');
            document.removeEventListener('click', handler);
        }
    });
}

function closeAllOptionMenus() {
    document.querySelectorAll('.option-menu.show').forEach(menu => {
        menu.classList.remove('show');
    });
}
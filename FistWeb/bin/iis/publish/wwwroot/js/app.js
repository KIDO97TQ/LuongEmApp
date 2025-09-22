window.closeSidebar = function () {
    const sidebar = document.querySelector('.sidebar');
    console.log("Closing sidebar...", sidebar);
    if (sidebar) {
        sidebar.classList.add("collapsed");
    }
};
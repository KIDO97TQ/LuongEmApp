
let productTypeCounter = 0;
let priceRangeCounter = 0;

// Add new product type
function addProductType() {
    productTypeCounter++;
    const container = document.getElementById('productTypes');
    const newItem = document.createElement('div');
    newItem.className = 'dynamic-item';
    newItem.innerHTML = `
    <input type="text" class="dynamic-input" placeholder="Nhập loại sản phẩm mới..." id="productType${productTypeCounter}">
        <button class="btn btn-danger" onclick="removeItem(this)">
            <span>🗑️</span> Xóa
        </button>
        `;
    container.appendChild(newItem);
}

// Add new price range
function addPriceRange() {
    priceRangeCounter++;
    const container = document.getElementById('priceRanges');
    const newItem = document.createElement('div');
    newItem.className = 'dynamic-item';
    newItem.innerHTML = `
        <input type="number" class="dynamic-input" placeholder="Nhập giá thuê (VNĐ)..." id="priceRange${priceRangeCounter}" min="0">
            <button class="btn btn-danger" onclick="removeItem(this)">
                <span>🗑️</span> Xóa
            </button>
            `;
    container.appendChild(newItem);
}

// Remove dynamic item
function removeItem(button) {
    const item = button.closest('.dynamic-item');
    item.style.animation = 'slideOutUp 0.3s ease-out';
    setTimeout(() => {
        item.remove();
    }, 300);
}

// Remove product type from select
function removeProductType() {
    const select = document.querySelector('select.form-select');
    if (select.selectedIndex > 0) {
        select.remove(select.selectedIndex);
        showNotification('Đã xóa loại sản phẩm!', 'success');
    } else {
        showNotification('Vui lòng chọn loại sản phẩm để xóa!', 'warning');
    }
}

// Remove price range from select
function removePriceRange() {
    const selects = document.querySelectorAll('select.form-select');
    const priceSelect = selects[1]; // Second select is for price
    if (priceSelect.selectedIndex > 0) {
        priceSelect.remove(priceSelect.selectedIndex);
        showNotification('Đã xóa mức giá!', 'success');
    } else {
        showNotification('Vui lòng chọn mức giá để xóa!', 'warning');
    }
}

// Preview uploaded image
function previewImage(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const uploadArea = document.querySelector('.image-upload-area');
            uploadArea.innerHTML = `
            <img src="${e.target.result}" alt="Preview" class="image-preview">
                <input type="file" id="imageInput" class="file-input" accept="image/*" onchange="previewImage(this)">
                    `;
        };
        reader.readAsDataURL(input.files[0]);
        showNotification('Ảnh đã được tải lên!', 'success');
    }
}

// Submit form
function submitForm() {
    const formData = collectFormData();
    console.log('Form Data:', formData);
    showNotification('Sản phẩm đã được thêm thành công!', 'success');
}

// Collect all form data
function collectFormData() {
    const data = {
        name: document.querySelector('input[placeholder="Nhập tên sản phẩm..."]').value,
        type: document.querySelector('select.form-select').value,
        size: document.querySelector('input[placeholder="S, M, L, XL..."]').value,
        price: document.querySelectorAll('select.form-select')[1].value,
        quantity: document.querySelector('input[type="number"]').value,
        description: document.querySelector('textarea').value,
        additionalTypes: [],
        additionalPrices: [],
        image: document.querySelector('.image-preview')?.src || null
    };

    // Collect dynamic data
    document.querySelectorAll('#productTypes .dynamic-input').forEach(input => {
        if (input.value) data.additionalTypes.push(input.value);
    });

    document.querySelectorAll('#priceRanges .dynamic-input').forEach(input => {
        if (input.value) data.additionalPrices.push(input.value);
    });

    return data;
}

// Show notification
function showNotification(message, type) {
    const notification = document.createElement('div');
    notification.style.cssText = `
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    background: ${type === 'success' ? '#28a745' : type === 'warning' ? '#ffc107' : '#dc3545'};
                    color: white;
                    padding: 15px 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                    z-index: 1000;
                    animation: slideInRight 0.3s ease-out;
                    `;
    notification.textContent = message;
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s ease-out';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// Add CSS for notification animations
const style = document.createElement('style');
style.textContent = `
                    @keyframes slideInRight {
                        from {transform: translateX(100%); opacity: 0; }
                    to {transform: translateX(0); opacity: 1; }
            }
                    @keyframes slideOutRight {
                        from {transform: translateX(0); opacity: 1; }
                    to {transform: translateX(100%); opacity: 0; }
            }
                    @keyframes slideOutUp {
                        from {transform: translateY(0); opacity: 1; }
                    to {transform: translateY(-20px); opacity: 0; }
            }
                    `;
document.head.appendChild(style);

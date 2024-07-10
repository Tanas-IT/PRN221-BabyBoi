const uploadBtn = document.querySelectorAll('.upload-btn');

const imageGalleries = document.querySelectorAll('.image-gallery');
imageGalleries.forEach((gallery, index) => {
    gallery.id = `gallery-${index}`;
});

const overlay = document.querySelector('.upload_overlay');
const overlayImage = overlay.querySelector('img');
const closeBtn = overlay.querySelector('.close-btn');


const uploadImageContainers = document.querySelectorAll('.upload_image-container');

uploadImageContainers.forEach(container => {
    const closeBtnIcon = container.querySelector('.close-icon');
    const img = container.querySelector('img');

    container.addEventListener('click', () => {
        overlayImage.src = img.src;
        overlay.classList.add('active');
        document.body.appendChild(overlay);
        overlay.appendChild(overlayImage);
    });

    closeBtnIcon.addEventListener('click', (e) => {
        e.stopPropagation();
        const imgToRemove = overlay.querySelector(`img[src="${img.src}"]`);
        if (imgToRemove) {
            overlay.removeChild(imgToRemove);
        }
        if (!overlay.contains(overlayImage)) {
            overlay.classList.remove('active');
            document.body.removeChild(overlay);
        }
        container.remove();
    });
});

// Hàm để thêm hình ảnh vào gallery
function addImageToGallery(imageUrl, galleryId) {
    const gallery = document.getElementById(`${galleryId}`);
    const imageContainer = document.createElement('div');
    imageContainer.classList.add('upload_image-container');

    const img = document.createElement('img');
    img.src = imageUrl;
    img.dataset.imageUrl = imageUrl;
    

    const inputSaveLinkImage = document.createElement('input');
    inputSaveLinkImage.setAttribute("name", "imageUrlsUpLoad");
    inputSaveLinkImage.value = imageUrl;
    inputSaveLinkImage.type = 'hidden';
    

    const preview = document.createElement('div');
    preview.classList.add('preview');
    preview.textContent = 'Preview';

    const closeIcon = document.createElement('span');
    closeIcon.classList.add('close-icon');
    closeIcon.textContent = 'x';

   
    imageContainer.appendChild(img);
    imageContainer.appendChild(preview);
    imageContainer.appendChild(closeIcon);
    imageContainer.appendChild(inputSaveLinkImage);

    imageContainer.addEventListener('click', () => {
        overlayImage.src = imageUrl;
        overlay.classList.add('active');
    });

    gallery.appendChild(imageContainer);

    closeIcon.addEventListener('click', (e) => {
        e.stopPropagation();
        imageContainer.remove();
        
    });
}

uploadBtn.forEach((uploadBtnChild, index) => {
    // Xử lý sự kiện chọn file
    uploadBtnChild.classList.add(`gallery-${index}`);
    uploadBtnChild.addEventListener('change', (event) => {
        const files = event.target.files;

        // Duyệt qua danh sách các file
        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const imageUrl = URL.createObjectURL(file);

            // Duyệt qua các gallery và thêm ảnh vào gallery tương ứng
            imageGalleries.forEach((gallery) => {
                if (uploadBtnChild.classList.contains(`${gallery.id}`))
                    addImageToGallery(imageUrl, gallery.id);
            });
        }
        
    });
})




// Xử lý sự kiện click vào nút đóng overlay
closeBtn.addEventListener('click', () => {
    overlay.classList.remove('active');
});

// Xử lý sự kiện click ra ngoài overlay
overlay.addEventListener('click', (event) => {
    if (event.target === overlay) {
        overlay.classList.remove('active');
    }
});

var toastEl = document.querySelector('.toast');
if (toastEl) {
    toastEl.classList.remove('d-none');
    var toastInstance = new bootstrap.Toast(toastEl);
    toastInstance.show();
    setTimeout(function () {
        toastInstance.hide();
    }, 5000);
}

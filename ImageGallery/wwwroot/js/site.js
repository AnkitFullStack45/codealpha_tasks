document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM loaded");

    let currentIndex = 1;
    const totalImages = 52;

    const lightbox = document.getElementById("lightbox");
    const lightboxImage = document.getElementById("lightboxImage");

    const closeButton = document.getElementById("closeLightbox");
    const previousButton = document.getElementById("previousImage");
    const nextButton = document.getElementById("nextImage");

    const galleryImages = document.querySelectorAll(".gallery-img");

    console.log("Gallery images found:", galleryImages.length);
    /* OPEN */
    galleryImages.forEach(function (image) {

        image.addEventListener("click", function () {

            console.log("IMAGE CLICKED");

            currentIndex = Number(this.dataset.index);

            lightboxImage.src =
                "/images/gallery/img" + currentIndex + ".jpg";

            lightbox.style.display = "flex";
        });
    });

    /* CLOSE */
    closeButton.addEventListener("click", function () {
        lightbox.style.display = "none";
    });
    /* NEXT */
    nextButton.addEventListener("click", function () {
        currentIndex++;
        if (currentIndex > totalImages) {
            currentIndex = 1;
        }
        lightboxImage.src =
            "/images/gallery/img" + currentIndex + ".jpg";
    });
    /* PREVIOUS */
    previousButton.addEventListener("click", function () {

        currentIndex--;

        if (currentIndex < 1) {
            currentIndex = totalImages;
        }
        lightboxImage.src =
            "/images/gallery/img" + currentIndex + ".jpg";
    });
    /* CLICK BACKGROUND TO CLOSE */
    lightbox.addEventListener("click", function (event) {
   if (event.target === lightbox) {
            lightbox.style.display = "none";
        }

    });

});
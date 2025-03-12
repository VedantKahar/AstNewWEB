
function openPDF(pdfName) {
    // Open the PDF in a new tab
    window.open(pdfName, '_blank');
}

    document.addEventListener("DOMContentLoaded", function () {
        const highlightText = new URLSearchParams(window.location.search).get('highlight');

    if (highlightText) {
        let regex = new RegExp(`(${highlightText})`, 'gi');
    document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
        }
    });

    // Get the video elements
    var corporateVideoElement = document.getElementById("corporate-video");
    var bgVideoElement = document.getElementById("bg-video");

    // Function to toggle play/pause on both videos
    function playPauseVideos() {
        if (corporateVideoElement.paused && bgVideoElement.paused) {
        corporateVideoElement.play();
    bgVideoElement.play();
        } else {
        corporateVideoElement.pause();
    bgVideoElement.pause();
        }
    }

    // Function to toggle fullscreen for the background video only
    function toggleFullScreen() {
        var videoElement = document.getElementById("bg-video"); // Targeting the background video

    if (videoElement.requestFullscreen) {
        videoElement.requestFullscreen();
        } else if (videoElement.mozRequestFullScreen) { // Firefox
        videoElement.mozRequestFullScreen();
        } else if (videoElement.webkitRequestFullscreen) { // Chrome, Safari, Opera
        videoElement.webkitRequestFullscreen();
        } else if (videoElement.msRequestFullscreen) { // IE/Edge
        videoElement.msRequestFullscreen();
        }
    }



let currentSlide = 0;

function updateSliderPosition() {
    const sliderContent = document.querySelector('.slider-content');
    
    sliderContent.style.transform = `translateX(-${currentSlide * slideWidth}px)`;
}

    // Ensure the DOM is fully loaded before running the script
    document.addEventListener("DOMContentLoaded", function () {
        // Function for auto-sliding the carousel every 5 seconds
        let currentIndex = 0;
    const sliderWrapper = document.querySelector('.slider-wrapper');
    const totalItems = document.querySelectorAll('.slider-item').length;

    function moveSlide(direction) {
        currentIndex = (currentIndex + direction + totalItems) % totalItems;
    sliderWrapper.style.transform = `translateX(-${currentIndex * 100}%)`;
            }

        // Auto slide every 5 seconds
        setInterval(() => {
        moveSlide(1);
        }, 9000);
    });


document.addEventListener("DOMContentLoaded", function () {
    const highlightText = new URLSearchParams(window.location.search).get('highlight');

    if (highlightText) {
        let regex = new RegExp(`(${highlightText})`, 'gi');
        document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
    }
});

document.addEventListener("DOMContentLoaded", function () {
    let currentJoineeIndex = 0; // To track the current joinee displayed
    const joinees = document.querySelectorAll('.new-joinee-content'); // All joinee cards
    const sliderContent = document.querySelector('.slider-content'); // The container of the cards

    // Function to move to the next joinee
    function nextJoinee() {
        if (currentJoineeIndex < joinees.length - 1) {
            currentJoineeIndex++;
        } else {
            currentJoineeIndex = 0; // Loop back to the first joinee
        }
        updateJoineeSlider();
    }

    // Function to move to the previous joinee
    function prevJoinee() {
        if (currentJoineeIndex > 0) {
            currentJoineeIndex--;
        } else {
            currentJoineeIndex = joinees.length - 1; // Loop back to the last joinee
        }
        updateJoineeSlider();
    }

    // Function to update the slider based on the current index
    function updateJoineeSlider() {
        // Adjust the transform of the slider content
        sliderContent.style.transform = `translateX(-${currentJoineeIndex * 100}%)`;
    }

    // Set an interval to change the joinee automatically every 7 seconds
    setInterval(nextJoinee, 10000); // Change every 7 seconds

    // Bind the prev and next buttons to their functions
    document.querySelector('.prev-btn').addEventListener('click', prevJoinee);
    document.querySelector('.next-btn').addEventListener('click', nextJoinee);
});

/*-----------------------------------------------------------------------------*/
let currentImageIndex = 0;

function changeImages(joineeId) {
    const images = document.querySelectorAll(`#${joineeId} .slider-image`);
    images.forEach((img, index) => {
        img.classList.remove('active');
    });

    currentImageIndex++;
    if (currentImageIndex >= images.length) {
        currentImageIndex = 0;
    }
    images[currentImageIndex].classList.add('active');
}

// Call this function on page load or when you navigate to an joinee
function startImageSlider() {
    setInterval(function () {
        changeImages('joinee1');  // Change to your specific joinee id
        // You can call the same function for other joinees, e.g. 'joinee2'
    }, 2000);  // Change image every 2 seconds
}

window.onload = function () {
    startImageSlider();
};

/*************************************************/
let currentOrderSlide = 0; // Initial slide position for orders
const orderSlides = document.querySelectorAll('.order-slide');
const totalOrderSlides = orderSlides.length;

function showOrderSlide(index) {
    const orderSliderContent = document.querySelector('.orders-slider'); // Apply transform to orders-slider
    if (index < 0) {
        currentOrderSlide = totalOrderSlides - 1;
    } else if (index >= totalOrderSlides) {
        currentOrderSlide = 0;
    } else {
        currentOrderSlide = index;
    }
    orderSliderContent.style.transform = `translateX(-${currentOrderSlide * 100}%)`;
}

function nextOrderSlide() {
    showOrderSlide(currentOrderSlide + 1);
}

function prevOrderSlide() {
    showOrderSlide(currentOrderSlide - 1);
}

// Initially show the first order slide
showOrderSlide(currentOrderSlide);

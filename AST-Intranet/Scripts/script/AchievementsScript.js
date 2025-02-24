////let currentIndex = 0;  // To track the current achievement displayed

////const achievements = document.querySelectorAll('.achievement-content');
////const sliderContent = document.querySelector('.slider-content');

////// Show the first achievement initially
////achievements[currentIndex].style.opacity = 1;

////// Function to move to the next achievement
////function nextAchievement() {
////    if (currentIndex < achievements.length - 1) {
////        currentIndex++;
////    } else {
////        currentIndex = 0;  // Loop back to the first achievement
////    }
////    updateSlider();
////}

////// Function to move to the previous achievement
////function prevAchievement() {
////    if (currentIndex > 0) {
////        currentIndex--;
////    } else {
////        currentIndex = achievements.length - 1;  // Loop back to the last achievement
////    }
////    updateSlider();
////}

////// Function to update the slider based on the current index
////function updateSlider() {
////    // Move the slider content based on the current index
////    sliderContent.style.transform = `translateX(-${currentIndex * 100}%)`;
////    // Update opacity for fade-in/out effect
////    achievements.forEach((achievement, index) => {
////        achievement.style.opacity = index === currentIndex ? 1 : 0;
////    });
////}

////// Set an interval to change the achievement automatically every 5 seconds
////setInterval(nextAchievement, 3000);

    document.addEventListener("DOMContentLoaded", function () {
        const highlightText = new URLSearchParams(window.location.search).get('highlight');

    if (highlightText) {
        let regex = new RegExp(`(${highlightText})`, 'gi');
    document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
        }
    });


let currentIndex = 0;  // To track the current achievement displayed

const achievements = document.querySelectorAll('.achievement-content');
const sliderContent = document.querySelector('.slider-content');

// Show the first achievement initially
achievements[currentIndex].style.opacity = 1;

// Function to move to the next achievement
function nextAchievement() {
    if (currentIndex < achievements.length - 1) {
        currentIndex++;
    } else {
        currentIndex = 0;  // Loop back to the first achievement
    }
    updateSlider();
}

// Function to move to the previous achievement
function prevAchievement() {
    if (currentIndex > 0) {
        currentIndex--;
    } else {
        currentIndex = achievements.length - 1;  // Loop back to the last achievement
    }
    updateSlider();
}

// Function to update the slider based on the current index
function updateSlider() {
    // Move the slider content based on the current index
    sliderContent.style.transform = `translateX(-${currentIndex * 100}%)`;
    // Update opacity for fade-in/out effect
    achievements.forEach((achievement, index) => {
        achievement.style.opacity = index === currentIndex ? 1 : 0;
    });
}

// Set an interval to change the achievement automatically every 5 seconds
setInterval(nextAchievement, 5000); // 5000ms = 5 seconds

/*-----------------------------------------------------------------------------*/
let currentImageIndex = 0;

function changeImages(achievementId) {
    const images = document.querySelectorAll(`#${achievementId} .slider-image`);
    images.forEach((img, index) => {
        img.classList.remove('active');
    });

    currentImageIndex++;
    if (currentImageIndex >= images.length) {
        currentImageIndex = 0;
    }
    images[currentImageIndex].classList.add('active');
}

// Call this function on page load or when you navigate to an achievement
function startImageSlider() {
    setInterval(function () {
        changeImages('achievement1');  // Change to your specific achievement id
        // You can call the same function for other achievements, e.g. 'achievement2'
    }, 2000);  // Change image every 2 seconds
}

window.onload = function () {
    startImageSlider();
};

document.addEventListener("DOMContentLoaded", function () {
    const highlightText = new URLSearchParams(window.location.search).get('highlight');

    if (highlightText) {
        let regex = new RegExp(`(${highlightText})`, 'gi');
        document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
    }
});


const toggleSidebar = document.getElementById('toggleSidebar');
        const sidebar = document.getElementById('sidebar');

        let isSidebarClicked = false; // Flag to track if sidebar is manually toggled

        // Toggle sidebar open/close on click
        toggleSidebar.addEventListener('click', function () {
            isSidebarClicked = !isSidebarClicked; // Toggle state

            if (isSidebarClicked) {
                sidebar.classList.add('open'); // Open the sidebar
                sidebar.style.width = '250px'; // Explicitly set width when open
            } else {
                sidebar.classList.remove('open'); // Close the sidebar
                sidebar.style.width = '70px'; // Explicitly set width when closed
            }

            // Remove hover event listeners when sidebar is manually toggled
            sidebar.removeEventListener('mouseenter', hoverOpen);
            sidebar.removeEventListener('mouseleave', hoverClose);

            // Re-enable hover effect only when sidebar is closed
            if (!isSidebarClicked) {
                // Only re-enable hover when sidebar is closed
                sidebar.addEventListener('mouseenter', hoverOpen);
                sidebar.addEventListener('mouseleave', hoverClose);
            }
        });

        // Hover behavior for opening the sidebar
        function hoverOpen() {
            if (!isSidebarClicked) { // Only open on hover if not manually toggled
                sidebar.classList.add('open');
                sidebar.style.width = '250px'; // Ensure the sidebar is open on hover
            }
        } 

        // Hover behavior for closing the sidebar
        function hoverClose() {
            if (!isSidebarClicked) { // Only close on hover if not manually toggled
                sidebar.classList.remove('open');
                sidebar.style.width = '70px'; // Ensure the sidebar is closed on hover
            }
        }

        // Initially enable hover effect only when sidebar is closed
        sidebar.addEventListener('mouseenter', hoverOpen);
        sidebar.addEventListener('mouseleave', hoverClose);
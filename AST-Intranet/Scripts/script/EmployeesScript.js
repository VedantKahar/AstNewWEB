let currentChart = null;
const ctx = document.getElementById('employeeChart').getContext('2d');

// Fetch Department Employee data from the server
function fetchDepartmentData(startYear, endYear) {
    console.log(`Fetching data for years ${startYear} - ${endYear}`);
    return fetch(`/Employees/GetEmployeesByDepartmentPerYear?startYear=${startYear}&endYear=${endYear}`)
        .then(response => response.json())
        .then(data => {
            console.log('Fetched Data:', data); // Debug log to inspect the response
            const years = Array.from(new Set(data.map(item => item.year))); // Get unique years
            const departments = Array.from(new Set(data.map(item => item.department))); // Get unique departments

            const departmentData = {
                labels: years, // Array of unique years
                datasets: departments.map(department => {
                    const employeeCounts = years.map(year => {
                        // Find the employee count for this department and year, or set it to 0 if not available
                        const item = data.find(d => d.department === department && d.year === year);
                        return item ? item.employee_count : 0;
                    });

                    return {
                        label: department,
                        data: employeeCounts,
                        borderColor: getRandomColor(),
                        backgroundColor: getRandomColor(),
                        fill: false,
                        tension: 0.1,
                    };
                }),
            };

            console.log('Department Data:', departmentData); // Log the final data structure
            return departmentData;
        })
        .catch(error => {
            console.error('Error fetching department data:', error);
            alert("Error fetching department data.");
        });
}

// Function to generate random color
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// Fetch year range dynamically from the backend
function fetchYearRange() {
    fetch('/Employees/GetYearRange')  // Adjust the URL to your actual endpoint
        .then(response => response.json())
        .then(data => {
            const { earliestYear, latestYear } = data;

            const startYearSelect = document.getElementById('startYear');
            const endYearSelect = document.getElementById('endYear');

            // Clear existing options
            startYearSelect.innerHTML = '';
            endYearSelect.innerHTML = '';

            // Add options dynamically for years
            for (let year = earliestYear; year <= latestYear; year++) {
                const option = document.createElement('option');
                option.value = year;
                option.textContent = year;
                startYearSelect.appendChild(option);

                const optionEnd = document.createElement('option');
                optionEnd.value = year;
                optionEnd.textContent = year;
                endYearSelect.appendChild(optionEnd);
            }

            // Set default values for start and end years
            startYearSelect.value = earliestYear;
            endYearSelect.value = latestYear;

            // Trigger the chart update after populating the dropdowns
            updateChart();
        })
        .catch(error => console.error('Error fetching year range:', error));
}

// Check if start year is greater than end year and show a warning
function validateYears() {
    const startYear = parseInt(document.getElementById('startYear').value);
    const endYear = parseInt(document.getElementById('endYear').value);

    // If start year is greater than end year
    if (startYear > endYear) {
        alert('Start year cannot be greater than end year. Please select valid years.');
        // Optionally, reset to default values or handle it as needed
        document.getElementById('startYear').value = endYear;  // Set start year to end year
    }
}

// Listen for changes in the year dropdowns and validate
document.getElementById('startYear').addEventListener('change', validateYears);
document.getElementById('endYear').addEventListener('change', validateYears);

// Create the chart with dynamic data
function createChart(data) {
    if (currentChart) {
        currentChart.destroy(); // Destroy previous chart instance to prevent duplication
    }

    // Get the selected chart type from the dropdown (line, bar, pie)
    const selectedGraphType = document.getElementById('chartTypeSelector').value;

    console.log('Creating chart with data:', data); // Log the chart data before creating the chart

    currentChart = new Chart(ctx, {
        type: selectedGraphType, // Use the selected graph type here (line, bar, pie, etc.)
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        boxWidth: 12, // Adjust the box size next to each legend item
                        padding: 20, // Add padding between the box and the label
                        font: {
                            size: 14, // Adjust font size of the department name
                            weight: '600', // Font weight for the department name
                        },
                    },
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    backgroundColor: 'rgba(0,0,0,0.7)', // Darken the tooltip background
                    bodyFont: {
                        size: 14, // Adjust tooltip body font size
                    },
                },
            },
            scales: {
                x: {
                    type: 'category',
                    title: { display: true, text: 'Year' },
                    grid: {
                        color: '#ddd',
                    },
                },
                y: {
                    title: { display: true, text: 'Number of Employees' },
                    ticks: { beginAtZero: true, stepSize: 1 },
                    grid: {
                        color: '#ddd',
                    },
                },
            },
        },
    });
}

// Fetch data and create chart based on selected graph type and year range
function updateChart() {
    const selectedGraphType = document.getElementById('graphType').value; // This controls the data type (department/gender)
    const startYear = parseInt(document.getElementById('startYear').value);
    const endYear = parseInt(document.getElementById('endYear').value);

    console.log('Selected Graph Type:', selectedGraphType);
    console.log('Start Year:', startYear);
    console.log('End Year:', endYear);

    if (selectedGraphType === 'department') {
        fetchDepartmentData(startYear, endYear).then(data => {
            if (data && data.labels && data.datasets) {
                createChart(data);
            } else {
                console.error('Invalid data format:', data);
                alert("No valid data received. Please try again.");
            }
        });
    } else if (selectedGraphType === 'gender') {
        console.log('Gender Graph selected. Fetching data...');
        // fetchGenderData(startYear, endYear).then(data => { createChart(data) });
    }
}

// Listen for changes in the dropdowns and update the chart accordingly
document.getElementById('graphType').addEventListener('change', updateChart);
document.getElementById('chartTypeSelector').addEventListener('change', updateChart); // Listen to the chart type change
document.getElementById('startYear').addEventListener('change', updateChart);
document.getElementById('endYear').addEventListener('change', updateChart);

// Initial chart load (on page load or when the page is ready)
fetchYearRange(); // Ensure to fetch year range on page load

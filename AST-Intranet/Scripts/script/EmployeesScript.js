// Create the chart with dynamic data
let currentChart = null;
const ctx = document.getElementById('employeeChart').getContext('2d'); // Assuming you have a canvas with id 'employeeChart'

// Fetch Department Employee data from the server
function fetchDepartmentData(startYear, endYear) {
    return fetch(`/Employees/GetEmployeesByDepartmentPerYear?startYear=${startYear}&endYear=${endYear}`)
        .then(response => response.json())
        .then(data => {
            const departments = Array.from(new Set(data.map(item => item.department))); // Get unique departments
            const departmentData = {
                labels: data.map(item => item.year), // Extract years
                datasets: departments.map(department => {
                    return {
                        label: department,
                        data: data.filter(item => item.department === department).map(item => item.employee_count),
                        borderColor: getRandomColor(),
                        backgroundColor: getRandomColor(),
                        fill: false,
                        tension: 0.1,
                    };
                }),
            };
            return departmentData;
        });
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

// Create the chart with dynamic data
function createChart(data) {
    if (currentChart) {
        currentChart.destroy(); // Destroy previous chart instance to prevent duplication
    }

    currentChart = new Chart(ctx, {
        type: 'line', // Line chart for employees per department
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                },
            },
            scales: {
                x: {
                    type: 'category',
                    title: { display: true, text: 'Year' },
                },
                y: {
                    title: { display: true, text: 'Number of Employees' },
                    ticks: { beginAtZero: true, stepSize: 1 },
                },
            },
        },
    });
}

// Fetch data and create chart based on selected graph type and year range
function updateChart() {
    const selectedGraphType = document.getElementById('graphType').value;
    const startYear = parseInt(document.getElementById('startYear').value);
    const endYear = parseInt(document.getElementById('endYear').value);

    if (selectedGraphType === 'department') {
        fetchDepartmentData(startYear, endYear).then(data => createChart(data));
    }
}

// Listen for changes in the dropdown and update the chart accordingly
document.getElementById('graphType').addEventListener('change', updateChart);
document.getElementById('startYear').addEventListener('change', updateChart);
document.getElementById('endYear').addEventListener('change', updateChart);

// Initial chart load (on page load or when the page is ready)
fetchYearRange(); // Ensure to fetch year range on page load

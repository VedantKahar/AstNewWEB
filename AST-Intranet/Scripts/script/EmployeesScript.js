document.addEventListener("DOMContentLoaded", function () {
    const highlightText = new URLSearchParams(window.location.search).get('highlight');

    // Highlight text if present in the URL
    if (highlightText) {
        highlightTextInPage(highlightText);
    }

    // Ensure elements exist before adding event listeners
    const graphTypeElement = document.getElementById('graphType');
    if (graphTypeElement) {
        // Initially fetch Total Department Employee Data
        fetchDepartmentData().then(data => {
            if (data && data.labels && data.datasets) {
                createChart(data);
            } else {
                showError("No valid data received for department data.");
            }
        });

        // Initially fetch Male/Female Employee Data
        fetchMaleFemaleData();

        // Listen for changes in the graph type (department vs gender) and update accordingly
        graphTypeElement.addEventListener('change', function () {
            const selectedGraphType = this.value;
            if (selectedGraphType === 'department') {
                fetchDepartmentData().then(createChart);
            } else if (selectedGraphType === 'gender') {
                if (window.genderData) {
                    createChart(window.genderData);
                } else {
                    showError("Gender data is not available.");
                }
            }
        });
    } else {
        console.error("graphType element not found");
    }
});

// Function to highlight text in the page based on URL
function highlightTextInPage(text) {
    let regex = new RegExp(`(${text})`, 'gi');
    document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
}

// Function to show error messages
function showError(message) {
    console.error(message);
    alert(message);
}

// Function to fetch Department Data
function fetchDepartmentData() {
    return fetch('/Employees/GetEmployeesByDepartmentTotal')
        .then(response => response.json())
        .then(data => {
            if (data.message) {
                showError(data.message);
                return;
            }
            return processDepartmentData(data);
        })
        .catch(() => {
            showError("Error fetching department data.");
        });
}

// Function to process department data
function processDepartmentData(data) {
    const departments = Array.from(new Set(data.map(item => item.department))); // Get unique departments
    const currentYear = new Date().getFullYear(); // Current year as label

    return {
        labels: [currentYear.toString()],
        datasets: departments.map(department => {
            const employeeCount = data.find(d => d.department === department)?.employee_count || 0;
            return {
                label: department,
                data: [employeeCount],
                borderColor: getRandomColor(),
                backgroundColor: getRandomColor(),
                fill: false,
                tension: 0.1,
            };
        }),
    };
}

// Function to fetch Male/Female Employee Data
function fetchMaleFemaleData() {
    return fetch('/Employees/GetMaleFemaleEmployeesTotal')
        .then(response => response.json())
        .then(data => {
            if (data.message) {
                showError(data.message);  // Show error message if API returns a message field
                console.log("Error Message: ", data.message);
                return;
            }

            if (data && Array.isArray(data) && data.length > 0) {
                // Process the male/female data for the chart
                const maleCount = data.find(d => d.gender === 'Male')?.employee_count || 0;
                const femaleCount = data.find(d => d.gender === 'Female')?.employee_count || 0;

                // Set the data structure for gender graph
                window.genderData = {
                    labels: ['Male', 'Female'], // X-axis labels for gender
                    datasets: [
                        {
                            label: 'Male Employees',
                            data: [maleCount, 0],  // Assign male data to the first position
                            borderColor: getRandomColor(),
                            backgroundColor: getRandomColor(),
                            fill: true, // Fill the bar with color
                            tension: 0.1,
                        },
                        {
                            label: 'Female Employees',
                            data: [0, femaleCount],  // Assign female data to the second position
                            borderColor: getRandomColor(),
                            backgroundColor: getRandomColor(),
                            fill: true, // Fill the bar with color
                            tension: 0.1,
                        }
                    ]
                };
            } else {
                showError("Gender data is not in the expected format.");
            }
        })
        .catch(error => {
            console.error("Error fetching male/female employee data:", error);
            showError("Error fetching male/female employee data.");
        });
}



// Function to generate a random color
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// Function to create the chart
function createChart(data) {
    const ctx = document.getElementById('employeeChart').getContext('2d');

    // If a chart already exists, destroy it to prevent duplicate charts
    if (window.currentChart) {
        window.currentChart.destroy();
    }

    // Determine if the chart is for departments or gender
    const graphType = document.getElementById('graphType').value;
    let xAxisLabel = '';
    let yAxisLabel = 'Number of Employees';

    // Set axis labels based on the graph type
    if (graphType === 'department') {
        xAxisLabel = 'Departments';  // Set X-axis label to "Departments" for department graph
    } else if (graphType === 'gender') {
        xAxisLabel = 'Gender';  // Set X-axis label to "Gender" for male/female graph
    }

    window.currentChart = new Chart(ctx, {
        type: 'bar',  // Use bar chart type
        data: data,   // The data object (either department data or gender data)
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        boxWidth: 12,
                        padding: 20,
                        font: { size: 14, weight: '600' },
                    },
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    backgroundColor: 'rgba(0,0,0,0.7)',
                    bodyFont: { size: 14 },
                },
            },
            scales: {
                x: {
                    type: 'category',
                    title: { display: true, text: xAxisLabel },  // X-axis label as "Gender" or "Departments"
                    grid: { color: '#ddd' },
                    // Control the bar width and spacing
                    ticks: {
                        padding: 10,  // Adjusts the spacing around the X-axis labels
                    },
                    // Use `categoryPercentage` and `barPercentage` to control the width of the bars
                    stacked: false, // Set to true if you want to stack the bars (like Male + Female)
                    // Adjust these values to control the bar width and spacing
                    gridLines: {
                        offsetGridLines: true
                    }
                },
                y: {
                    title: { display: true, text: yAxisLabel },
                    ticks: { beginAtZero: true, stepSize: 1 },
                    grid: { color: '#ddd' },
                },
            },
            // This will control the width of bars
            layout: {
                padding: {
                    //left: 10,
                    //right: 10,
                    //top: 10,
                    //bottom: 10,
                },
            },
            barPercentage: 0.9,  // Control the width of the bars (adjust the percentage for wider bars)
            categoryPercentage: 0.9,  // Adjust space between bars
        },
    });
}



//// Function to create custom plugin for displaying total employees and departments
//function createDisplayTextPlugin(totalEmployees, totalDepartments) {
//    return {
//        id: 'displayTextPlugin',
//        beforeDraw: function (chart) {
//            const ctx = chart.ctx;
//            const width = chart.width;
//            const height = chart.height;

//            ctx.font = '16px Arial';
//            ctx.fillStyle = 'black';
//            ctx.fillText(`Total Employees: ${totalEmployees}`, width - 200, 110);
//            ctx.fillText(`Total Departments: ${totalDepartments}`, width - 200, 130);
//        },
//    };
//}

// Listen for changes in chart type and update accordingly
//document.getElementById('chartTypeSelector').addEventListener('change', function () {
//    const currentGraphType = document.getElementById('graphType').value;
//    if (currentGraphType === 'department') {
//        fetchDepartmentData().then(createChart);
//    } else if (currentGraphType === 'gender') {
//        createChart(window.genderData);
//    }
//});

// Listen for changes in the graph type (department vs gender) and update accordingly
document.getElementById('graphType').addEventListener('change', function () {
    const selectedGraphType = this.value;

    if (selectedGraphType === 'department') {
        fetchDepartmentData().then(createChart);
    } else if (selectedGraphType === 'gender') {
        if (window.genderData) {
            createChart(window.genderData);
        } else {
            showError("Gender data is not available.");
        }
    }
});


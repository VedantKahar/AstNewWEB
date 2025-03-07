document.addEventListener("DOMContentLoaded", function () {
    const highlightText = new URLSearchParams(window.location.search).get('highlight');

    // Highlight text if present in the URL
    if (highlightText) {
        highlightTextInPage(highlightText);
    }

    // Initially fetch Total Department Employee Data
    fetchDepartmentData().then(data => {
        if (data && data.labels && data.datasets) {
            createChart(data);
        } else {
            showError("No valid data received for department data.");
        }
    });

    // Fetch Total Male/Female Employee Data (if needed later)
    fetchMaleFemaleData().then(data => {
        if (data && data.labels && data.datasets) {
            window.genderData = data;
        } else {
            showError("No valid data received for gender data.");
        }
    });
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

// Function to fetch Male/Female Data
function fetchMaleFemaleData() {
    return fetch('/Employees/GetMaleFemaleEmployeesTotal')
        .then(response => response.json())
        .then(data => {
            const maleCount = data[0]?.male_count || 0;
            const femaleCount = data[0]?.female_count || 0;
            return createGenderData(maleCount, femaleCount);
        })
        .catch(() => {
            showError("Error fetching male/female department data.");
        });
}

// Function to generate gender data structure
function createGenderData(maleCount, femaleCount) {
    const currentYear = new Date().getFullYear();
    return {
        labels: [currentYear.toString()],
        datasets: [
            createDataset('Male', maleCount),
            createDataset('Female', femaleCount),
        ],
    };
}

// Function to create dataset for gender
function createDataset(label, count) {
    return {
        label: label,
        data: [count],
        borderColor: getRandomColor(),
        backgroundColor: getRandomColor(),
        fill: false,
        tension: 0.1,
    };
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

    if (window.currentChart) {
        window.currentChart.destroy();
    }

    //const selectedGraphType = document.getElementById('chartTypeSelector').value;
    const totalEmployees = data.datasets.reduce((sum, dataset) => sum + dataset.data[0], 0);
    const totalDepartments = data.datasets.length;

    //const displayTextPlugin = createDisplayTextPlugin(totalEmployees, totalDepartments);

    window.currentChart = new Chart(ctx, {
        type: 'bar',
        data: data,
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
                    title: { display: true, text: 'Department' },
                    grid: { color: '#ddd' },
                },
                y: {
                    title: { display: true, text: 'Number of Employees' },
                    ticks: { beginAtZero: true, stepSize: 1 },
                    grid: { color: '#ddd' },
                },
            },
        },
        //plugins: [displayTextPlugin],
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
document.getElementById('chartTypeSelector').addEventListener('change', function () {
    const currentGraphType = document.getElementById('graphType').value;
    if (currentGraphType === 'department') {
        fetchDepartmentData().then(createChart);
    } else if (currentGraphType === 'gender') {
        createChart(window.genderData);
    }
});

// Listen for changes in the graph type (department vs gender) and update accordingly
document.getElementById('graphType').addEventListener('change', function () {
    const selectedGraphType = this.value;

    if (selectedGraphType === 'department') {
        fetchDepartmentData().then(data => {
            createChart(data, document.getElementById('chartTypeSelector').value);
        });
    } else if (selectedGraphType === 'gender') {
        createChart(window.genderData, document.getElementById('chartTypeSelector').value);
    }
});

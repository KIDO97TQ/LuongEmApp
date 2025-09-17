window.chartHelper = {
    renderChart: function (canvasId, chartData, chartType, options = {}) {
        const ctx = document.getElementById(canvasId).getContext('2d');

        // Xóa biểu đồ cũ nếu có
        if (!window.renderedCharts) {
            window.renderedCharts = {};
        }

        if (window.renderedCharts[canvasId]) {
            window.renderedCharts[canvasId].destroy();
        }

        const config = {
            type: chartType,
            data: chartData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 2000,
                    easing: 'easeOutQuart'
                },
                plugins: {
                    tooltip: {
                        backgroundColor: 'rgba(0,0,0,0.8)',
                        titleColor: 'white',
                        bodyColor: 'white',
                        borderColor: '#4FC3F7',
                        borderWidth: 1
                    },
                    legend: {
                        display: true,
                        position: 'top',
                        labels: {
                            usePointStyle: true,
                            padding: 20
                        }
                    }
                },
                scales: {
                    x: {
                        stacked: options.stackedX ?? false,
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        stacked: options.stackedY ?? false,
                        beginAtZero: true,
                        max: options.yMax ?? undefined,
                        grid: {
                            color: 'rgba(0,0,0,0.1)'
                        }
                    }
                }
            }
        };

        const chart = new Chart(ctx, config);
        window.renderedCharts[canvasId] = chart;
    }
};

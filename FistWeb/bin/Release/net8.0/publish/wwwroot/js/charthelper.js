window.chartHelper = {
    chartInstance: null,

    renderChart: function (canvasId, chartData, chartType) {
        if (this.chartInstance) {
            this.chartInstance.destroy();
        }

        const ctx = document.getElementById(canvasId).getContext('2d');

        // 👉 Thêm tension vào từng dataset (nếu là line chart)
        if (chartType === 'line' && chartData.datasets) {
            chartData.datasets.forEach(dataset => {
                dataset.tension = 0.4;             // Làm mềm đường
                dataset.pointRadius = 4;           // Kích thước điểm
                dataset.pointHoverRadius = 6;      // Điểm khi hover
                dataset.fill = false;              // Không tô dưới đường
            });
        }

        this.chartInstance = new Chart(ctx, {
            type: chartType,
            data: chartData,
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Biểu đồ doanh thu1'
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const value = context.raw;
                                return context.dataset.label + ': ' + value.toLocaleString('vi-VN') + ' đ';
                            }
                        }
                    },
                    legend: {
                        position: 'top'
                    }
                },
                scales: {
                    y: {
                        ticks: {
                            callback: function (value) {
                                return value.toLocaleString('vi-VN') + ' đ';
                            }
                        }
                    },
                    x: {
                        ticks: {
                            maxRotation: 45,
                            minRotation: 45
                        }
                    }
                }
            }
        });
    }
};


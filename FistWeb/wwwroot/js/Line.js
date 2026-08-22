window.userDailyExpenseChart = {

    chart: null,

    render: function (canvasId, labels, userDatasets) {

        const canvas = document.getElementById(canvasId);

        if (!canvas) {
            console.error("Không tìm thấy canvas: " + canvasId);
            return;
        }

        // Hủy chart cũ
        if (this.chart) {
            this.chart.destroy();
            this.chart = null;
        }

        // Màu tự động cho từng User
        const colors = [
            '#667eea',
            '#e74c3c',
            '#2ecc71',
            '#f39c12',
            '#9b59b6',
            '#1abc9c',
            '#34495e',
            '#e67e22'
        ];

        const datasets = userDatasets.map((user, index) => {

            const color = colors[index % colors.length];

            return {
                label: user.label,

                data: user.data,

                borderColor: color,

                backgroundColor: color,

                borderWidth: 3,

                tension: 0.3,

                fill: false,

                pointRadius: 2,

                pointHoverRadius: 5,

                pointBorderWidth: 2
            };
        });

        this.chart = new Chart(canvas, {

            type: 'line',

            data: {
                labels: labels,
                datasets: datasets
            },

            options: {

                responsive: true,
                maintainAspectRatio: false,

                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 5,
                        bottom: 0
                    }
                },

                interaction: {
                    intersect: false,
                    mode: 'index'
                },

                scales: {

                    x: {
                        title: {
                            display: true,
                            text: 'Ngày'
                        },

                        // Giảm khoảng trống hai đầu trục X
                        offset: false
                    },

                    y: {

                        beginAtZero: true,

                        title: {
                            display: false
                        },

                        ticks: {
                            padding: 4,

                            callback: function (value) {

                                if (value >= 1000000000)
                                    return (value / 1000000000).toFixed(1) + 'Tỷ';

                                if (value >= 1000000)
                                    return (value / 1000000).toFixed(1) + 'M';

                                if (value >= 1000)
                                    return (value / 1000).toFixed(0) + 'K';

                                return value + ' K';
                            }
                        }
                    }
                }
            }
            
        });
    }
};
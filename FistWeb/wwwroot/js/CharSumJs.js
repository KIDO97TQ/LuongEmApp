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

        // Cấu hình legend + scales
        const legendConfig = this.getLegendConfig(options.legendPosition || 'right');
        const scalesConfig = this.getScalesConfig(chartType, options);

        const config = {
            type: chartType,
            data: chartData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: legendConfig,
                    tooltip: {
                        backgroundColor: 'rgba(0,0,0,0.8)',
                        titleColor: 'white',
                        bodyColor: 'white',
                        borderColor: '#667eea',
                        borderWidth: 1,
                        cornerRadius: 8,
                        callbacks: {
                            label: function (context) {
                                return context.dataset.label + ': ' +
                                    new Intl.NumberFormat('vi-VN').format(context.parsed.y) + ' VNĐ';
                            }
                        }
                    }
                },
                scales: scalesConfig,
                animation: {
                    duration: options.animationDuration || 1500,
                    easing: options.animationEasing || 'easeOutQuart'
                }
            }
        };

        const chart = new Chart(ctx, config);
        window.renderedCharts[canvasId] = chart;

        // Tạo custom legend nếu bật
        if (options.customLegend) {
            this.createCustomLegend(canvasId, chart, options);
        }

        return chart;
    },

    // Legend config mặc định
    getLegendConfig: function (position) {
        return {
            display: position !== 'none',
            position: position,
            labels: {
                usePointStyle: true,
                padding: 15,
                font: { size: 12 }
            }
        };
    },

    // Scale config
    getScalesConfig: function (chartType, options) {
        if (chartType === 'pie' || chartType === 'doughnut') {
            return {}; // Pie không cần scale
        }

        return {
            x: {
                stacked: options.stackedX ?? false,
                grid: {
                    display: options.showXGrid ?? false,
                    color: options.gridColor || 'rgba(0,0,0,0.1)'
                },
                ticks: {
                    color: options.tickColor || '#6c757d',
                    font: { size: options.tickFontSize || 11 }
                }
            },
            y: {
                stacked: options.stackedY ?? false,
                beginAtZero: true,
                grid: {
                    display: options.showYGrid ?? true,
                    color: options.gridColor || 'rgba(0,0,0,0.1)',
                    borderDash: options.gridDash || [2, 2]
                },
                ticks: {
                    color: options.tickColor || '#6c757d',
                    font: { size: options.tickFontSize || 11 },
                    callback: function (value) {
                        if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                        if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                        return value;
                    }
                }
            }
        };
    },

    // Custom legend
    createCustomLegend: function (canvasId, chart, options) {
        const legendId = canvasId + '-legend';
        let legendContainer = document.getElementById(legendId);

        if (!legendContainer) {
            legendContainer = document.createElement('div');
            legendContainer.id = legendId;
            legendContainer.className = 'custom-chart-legend';
            const chartContainer = document.getElementById(canvasId).closest('.chart-container') ||
                document.getElementById(canvasId).parentElement;
            chartContainer.appendChild(legendContainer);
        }

        this.addCustomLegendStyles();

        const legendHtml = chart.data.datasets.map((dataset, index) => {
            const hidden = !chart.isDatasetVisible(index);
            return `
                <div class="legend-item ${hidden ? 'hidden' : ''}" data-index="${index}">
                    <div class="legend-color" style="background-color:${dataset.backgroundColor}"></div>
                    <span class="legend-text">${dataset.label}</span>
                </div>
            `;
        }).join('');

        legendContainer.innerHTML = legendHtml;
        legendContainer.className = `custom-chart-legend legend-${options.legendPosition || 'top'}`;

        legendContainer.querySelectorAll('.legend-item').forEach(item => {
            item.addEventListener('click', function () {
                const index = parseInt(this.dataset.index);
                const meta = chart.getDatasetMeta(index);
                meta.hidden = meta.hidden === null ? !chart.data.datasets[index].hidden : null;
                chart.update();
                this.classList.toggle('hidden', meta.hidden);
            });
        });
    },

    // CSS custom legend
    addCustomLegendStyles: function () {
        if (document.getElementById('custom-legend-styles')) return;

        const style = document.createElement('style');
        style.id = 'custom-legend-styles';
        style.textContent = `
            .custom-chart-legend {
                display: flex;
                gap: 12px;
                margin-top: 15px;
            }
            .legend-top, .legend-bottom {
                flex-direction: row;
                flex-wrap: wrap;
                justify-content: center;
            }
            .legend-right {
                flex-direction: column;
                align-items: flex-start;
                margin-left: 20px;
            }
            .legend-item {
                display: flex;
                align-items: center;
                gap: 8px;
                cursor: pointer;
                padding: 5px 10px;
                border-radius: 6px;
                transition: all 0.2s ease;
                font-size: 14px;
                color: #495057;
            }
            .legend-item:hover {
                background: #f1f3f5;
                transform: translateX(3px);
            }
            .legend-item.hidden {
                opacity: 0.5;
                text-decoration: line-through;
            }
            .legend-color {
                width: 14px;
                height: 14px;
                border-radius: 50%;
            }
        `;
        document.head.appendChild(style);
    }
};

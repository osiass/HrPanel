window.chartInterop = {
    _charts: {},  // Mevcut grafik instance'larını sakla

    getThemeColors: function () {
        const isDark = document.documentElement.getAttribute('data-theme') === 'dark';
        return {
            grid: isDark ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.05)',
            text: isDark ? '#cbd5e1' : '#6b7280'
        };
    },

    _destroyChart: function (canvasId) {
        if (this._charts[canvasId]) {
            this._charts[canvasId].destroy();
            delete this._charts[canvasId];
        }
    },

    setupLineChart: function (canvasId, labels, data) {
        const colors = this.getThemeColors();
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;
        this._destroyChart(canvasId);
        const ctx = canvas.getContext('2d');
        this._charts[canvasId] = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'İşe Alım',
                    data: data,
                    fill: true,
                    backgroundColor: 'rgba(78, 115, 223, 0.2)',
                    borderColor: '#4e73df',
                    pointBackgroundColor: '#4e73df',
                    pointBorderColor: '#fff',
                    pointHoverBackgroundColor: '#fff',
                    pointHoverBorderColor: '#4e73df',
                    pointRadius: 4,
                    pointHoverRadius: 6,
                    tension: 0.4
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { 
                            color: colors.text,
                            stepSize: 1,
                            precision: 0
                        },
                        grid: {
                            drawBorder: false,
                            color: colors.grid
                        },
                        suggestedMax: 5
                    },
                    x: {
                        ticks: { color: colors.text },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    },
    setupDonutChart: function (canvasId, labels, data) {
        const colors = this.getThemeColors();
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;
        this._destroyChart(canvasId);
        const ctx = canvas.getContext('2d');
        this._charts[canvasId] = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: ['#4e73df', '#10b981', '#36b9cc'],
                    hoverBackgroundColor: ['#2e59d9', '#059669', '#2c9faf'],
                    hoverBorderColor: "rgba(234, 236, 244, 1)",
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 20,
                            color: colors.text
                        }
                    }
                },
                cutout: '70%'
            }
        });
    }
};

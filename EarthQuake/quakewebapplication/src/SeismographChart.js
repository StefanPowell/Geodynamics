import React, { useRef, useEffect } from 'react';
import { Chart } from 'chart.js/auto';

const SeismographChart = () => {
  const canvasRef = useRef(null);
  const chartRef = useRef(null);

  useEffect(() => {
    const ctx = canvasRef.current.getContext('2d');

    const data = {
      labels: [...Array(20).keys()],
      datasets: [{
        label: 'Seismic Activity',
        data: Array.from({ length: 100 }, (_, i) => Math.sin(i / 2) + (Math.random() - 0.2)),
        borderColor: '#000000',
        borderWidth: 2,
        fill: false,
        tension: 0.4, // Cubic interpolation
        pointRadius: 0,
      }]
    };

    const options = {
        responsive: true,
        maintainAspectRatio: false, // so it fills the container
        animation: false,
        plugins: {
          legend: { display: false }
        },
        scales: {
          x: {
            display: true,
            title: {
              display: true,
              text: 'Time',
              color: '#000000',
              font: {
                size: 14
              }
            },
            grid: { color: '#f5f5f5' },
            ticks: { color: '#000000' }
          },
          y: {
            min: -2,
            max: 2,
            grid: { color: '#f5f5f5' },
            ticks: { color: '#000000' },
            title: {
              display: true,
              text: 'Amplitude',
              color: '#000000',
              font: {
                size: 14
              }
            }
          }
        }
      };      

    chartRef.current = new Chart(ctx, {
      type: 'line',
      data,
      options
    });

    return () => chartRef.current.destroy(); // Cleanup on unmount
  }, []);

  return <canvas ref={canvasRef} />;
};

export default SeismographChart;

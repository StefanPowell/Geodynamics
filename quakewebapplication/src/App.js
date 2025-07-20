import React, { useEffect, useState } from 'react';
import mapboxgl from 'mapbox-gl';
import SeismographChart from './SeismographChart';
import './App.css';

mapboxgl.accessToken = 'pk.eyJ1Ijoic3RlZmFuMzc1IiwiYSI6ImNtNTdvM3dxdDNocjMybXE3NHM5cWljcXoifQ.Trxsip1AGGVhFAMEmbyd8w';

function App() {
  const [quakeData, setQuakeData] = useState([]);

  const handleCellClick = (latitude, longitude) => {
    const maxradius = 10;
    const url = `https://localhost:44302/Station?latitude=${latitude}&longitude=${longitude}&maxradius=${maxradius}`;

    fetch(url)
      .then((response) => {
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        return response.json();
      })
      .then((result) => {
        console.log(result);
        setQuakeData([result]);
      })
      .catch((error) => console.error('Fetch error:', error));

    console.log('Cell clicked:', latitude, longitude);
  };

  useEffect(() => {
    fetch('https://localhost:44302/QuakeData')
      .then((response) => response.json())
      .then((data) => setQuakeData(data))
      .catch((error) => console.error('Error fetching quake data:', error));
  }, []);

  return (
    <div className="App">
      <div className="quadrant red">
        <div className="map-container"></div>
      </div>

      <div className="quadrant green">
        <div className="seismograph-wrapper">
          <SeismographChart />
        </div>
      </div>

      <div className="quadrant yellow">Yellow</div>
      <div className="quadrant purple">Purple</div>
      <div className="quadrant orange">Orange</div>

      <div className="quadrant blue">
        <div className="table-wrapper">
          <table className="earthquake-table">
            <thead>
              <tr>
                <th>Magnitude</th>
                <th>Depth</th>
                <th>Place</th>
                <th>Latitude</th>
                <th>Longitude</th>
                <th>Time</th>
                <th>Modified Mercalli Intensity</th>
                <th>AzimGap</th>
                <th>Tsunami</th>
              </tr>
            </thead>
            <tbody>
              {quakeData.map((quake, index) => (
                <tr key={index} onClick={() => handleCellClick(quake.lat, quake.lon)}>
                  <td>{quake.mag}</td>
                  <td>{quake.depth} km</td>
                  <td>{quake.place}</td>
                  <td>{quake.lat}°N</td>
                  <td>{quake.lon}°W</td>
                  <td>{new Date(quake.time).toLocaleString()}</td>
                  <td>{quake.mmi ?? 'N/A'}</td>
                  <td>{quake.azim}</td>
                  <td>{quake.tsunami === 1 ? 'Yes' : 'No'}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <div className="quadrant pink">Pink</div>
    </div>
  );
}

export default App;

import React, { useEffect, useRef } from 'react';
import mapboxgl from 'mapbox-gl';
import './App.css';

// Set your Mapbox access token
mapboxgl.accessToken = 'pk.eyJ1Ijoic3RlZmFuMzc1IiwiYSI6ImNtNTdvM3dxdDNocjMybXE3NHM5cWljcXoifQ.Trxsip1AGGVhFAMEmbyd8w';

function App() {
  const mapContainer = useRef(null);

  useEffect(() => {
    const map = new mapboxgl.Map({
      container: mapContainer.current,
      style: 'mapbox://styles/mapbox/streets-v11', // Map style
      center: [-74.5, 40], // Initial map center [lng, lat]
      zoom: 9, // Initial map zoom level
    });

    return () => map.remove(); // Clean up map on component unmount
  }, []);

  return (
    <div className="App">
      {/* Second Quadrant: Map */}
      <div className="quadrant red">
        <div ref={mapContainer} className="map-container"></div>
      </div>

      {/* First Quadrant: Green, Yellow, Purple, Orange */}
      <div className="quadrant green">Green</div>
      <div className="quadrant yellow">Yellow</div>
      <div className="quadrant purple">Purple</div>
      <div className="quadrant orange">Orange</div>

      {/* Third Quadrant */}
      <div className="quadrant blue">Blue</div>

      {/* Fourth Quadrant */}
      <div className="quadrant pink">Pink</div>
    </div>
  );
}

export default App;

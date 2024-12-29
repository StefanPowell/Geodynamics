import mapboxgl from 'mapbox-gl';

import 'mapbox-gl/dist/mapbox-gl.css';



// TO MAKE THE MAP APPEAR YOU MUST
	// ADD YOUR ACCESS TOKEN FROM
	// https://account.mapbox.com
	mapboxgl.accessToken = 'pk.eyJ1Ijoic3RlZmFuMzc1IiwiYSI6ImNtNTdvM3dxdDNocjMybXE3NHM5cWljcXoifQ.Trxsip1AGGVhFAMEmbyd8w';
    const map = new mapboxgl.Map({
        container: 'map', // container ID
        // Choose from Mapbox's core styles, or make your own style with Mapbox Studio
        style: 'mapbox://styles/mapbox/satellite-v9', // style URL
        projection: 'globe', // Display the map as a globe, since satellite-v9 defaults to Mercator
        zoom: 2, // starting zoom
        center: [108, 4] // // starting center in [lng, lat]
    });

    map.on('style.load', () => {
        map.setFog({}); // Set the default atmosphere style
    });

    map.on('load', () => {
        map.addSource('earthquakes', {
            type: 'geojson',
            // Use a URL for the value for the `data` property.
            data: 'https://docs.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson'
        });

        map.addLayer({
            'id': 'earthquakes-layer',
            'type': 'circle',
            'source': 'earthquakes',
            'paint': {
                'circle-radius': 4,
                'circle-stroke-width': 2,
                'circle-color': 'red',
                'circle-stroke-color': 'white'
            }
        });
    });

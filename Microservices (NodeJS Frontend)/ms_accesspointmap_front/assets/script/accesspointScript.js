const insertData = (latitude, longitude, radius) => {
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 19,
        center: { lat: latitude, lng: longitude },                   
        mapTypeId: 'satellite'
    });

    new google.maps.Circle({
        center: { lat: latitude, lng: longitude },
        radius: (radius < 10) ? 10 : parseFloat(radius),
        map: map,
        strokeColor: "#532c7a",
        strokeOpacity: 0.8,
        fillColor: "#532c7a",
        fillOpacity: 0.25
    });
};
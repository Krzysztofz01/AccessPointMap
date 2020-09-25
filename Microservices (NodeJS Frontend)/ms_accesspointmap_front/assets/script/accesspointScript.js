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
        strokeColor: "#84c69b",
        strokeOpacity: 0.8,
        fillColor: "#84c69b",
        fillOpacity: 0.25
    });

    document.getElementById('backButton').addEventListener('click', () => {
        window.history.go(-1); return false;
    });
};
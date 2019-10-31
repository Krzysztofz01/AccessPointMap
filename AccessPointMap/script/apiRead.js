let info = {};

function readData() {
    fetch('http://localhost/accesspointmap/api/actions/read.php')
    .then(function(resp) {
        return resp.json();
    })
    .then(function(data) {
        console.log(data.records);
        
        
    })
}


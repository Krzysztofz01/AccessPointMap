document.getElementById('search').addEventListener('click', () => search());

const search = () => {
    const ssid = document.getElementById('ssid').value;
    const brand = document.getElementById('brand').value;
    const freq = document.getElementById('freq').value;
    const security = document.getElementById('securityDropdown').value;

    window.location.assign(window.location.href.split('?')[0] + 
        "?ssid=" + ssid +
        "&freq=" + freq +
        "&brand=" + brand +
        "&security=" + security);
};

const elements = document.getElementsByClassName('accessPointUrl');
for(let item of elements) {
    console.log(item);
    item.addEventListener('click', () => addHref(item.id));
}

const addHref = (id) => {
    window.location.assign(window.location.href.split('?')[0].replace('search', 'accesspoint?id=' + id.toString()));
}
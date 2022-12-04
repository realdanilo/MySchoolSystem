GetLocation = new Promise((resolve, reject) => {
    navigator.geolocation.getCurrentPosition(
        position => { resolve([position.coords.latitude, position.coords.longitude]) },
        error => { reject(error) }
    )
})
function GetWeather() {
    let baseURL = "https://api.open-meteo.com/v1/forecast";
    GetLocation.then(([lat, long], rej) => { 
        fetch(`${baseURL}?latitude=${lat}&longitude=${long}&hourly=temperature_2m&temperature_unit=fahrenheit`)
            .then(res => res.json())
            .then(function (res) {
                console.log(res)
                return res.hourly["temperature_2m"][0]
            })
    })
}

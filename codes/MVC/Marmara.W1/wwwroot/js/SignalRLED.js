$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/ledhub").build();

    start();

    function start() {
        connection.start().then(() => {
            connection.invoke("ReceiveDataFromHub");
        }).catch((err) => {
            console.log(err);
            setTimeout(() => start(), 2000)
        });
    }

    connection.on("ReceiveData", (data) => {

        var image = document.getElementById('imgLed');
        $("#input-slider-blue").val(data);
        $("#range-slider-blue").val(data);

        var image = document.getElementById('imgLedLight');
        image.style.filter = "brightness(" + parseInt(data) + "%)";
        console.log("coming data from signalR for LED : " + data);
    });
})
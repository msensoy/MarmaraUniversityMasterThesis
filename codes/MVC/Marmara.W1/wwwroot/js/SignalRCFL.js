$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/cflhub").build();

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

        var image = document.getElementById('imgCfl');
        if (data == true) {
            image.src = "/img/L2a.png";
            $('#bntCfl').text("OFF")
        }
        else {
            $('#bntCfl').text("ON")
            image.src = "/img/L2.png";
        }

        console.log("coming data from signalR for CFL : " + data);
    });
})
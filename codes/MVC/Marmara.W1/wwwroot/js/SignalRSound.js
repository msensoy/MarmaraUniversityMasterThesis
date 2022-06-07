$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/soundhub").build();

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
        $("#sound").text(data.data);

        if (data.data == 1) {
            $("#SoundSensor").css("background-color", "gray");
        }
        else {
            $("#SoundSensor").css("background-color", "yellow");
        }
    });

})
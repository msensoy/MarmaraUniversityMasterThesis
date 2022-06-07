$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/pirhub").build();

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
        $("#pir").text(data.data);

        if (data.data == 1) {
            $("#PIRSensor").css("background-color", "gray");
        }
        else {
            $("#PIRSensor").css("background-color", "blue");
        }
    });

})
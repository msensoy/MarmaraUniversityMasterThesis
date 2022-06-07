$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/flamehub").build();

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
        $("#flame").text(data.data);

        if (data.data == 1) {
            $("#FlameSensor").css("background-color", "gray");
        }
        else {
            $("#FlameSensor").css("background-color", "red");
        }
       

    });

})
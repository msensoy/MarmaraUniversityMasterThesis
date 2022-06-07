$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/ldrhub").build();

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
        console.log("coming data from signalR for LDR : " + data.data);
        $("#ldr").text(data.data);
    });



})
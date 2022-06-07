$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/mq135hub").build();

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
        debugger;
        console.log(data.co2);
        $("#co2").text(data.co2);
    });

})
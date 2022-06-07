$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/mq2hub").build();

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
        console.log('data:'+ data);
        $("#smoke").text(data.smoke);
        $("#co").text(data.co);
        $("#lpg").text(data.lpg);
    });

})
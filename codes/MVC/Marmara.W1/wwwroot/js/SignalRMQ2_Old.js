$(document).ready(() => {
    var conn = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/mq2hub").build();

    statusShow();
    start();
    function statusShow() {
        $('#conStatus').text(conn.connectionState);
    }
    function start() {
        conn.start().then(() => {
            statusShow();
            $('#loading').hide();
            conn.invoke("ReceiveDataFromHub");
        }).catch((err) => {
            console.log(err);
            setTimeout(() => start(), 2000)
        });
    }

    statusShow();

    //Subscribes

    conn.onreconnecting(err => {
        $('#loading').show();
        statusShow();
        console.log("onreconnection: " + err)
    });

    conn.onreconnected(connnectionId => {
        $('#loading').hide();
        statusShow();
        console.log("onreconnected: " + connnectionId)
    });

    conn.onclose(() => {
        alert("Sayfayı yenile, bağlantı kurulamadı")
        $('#loading').hide();
        statusShow();
        console.log("onclose: " + connnectionId)
        start();
    });

    conn.on("Error", (errorText) => {
        alert(errorText);
    });

   

    conn.on("ReceiveData", (data) => {
        $("#smoke").text(data.smoke);
        $("#co").text(data.co);
        $("#lpg").text(data.lpg);
    });

})
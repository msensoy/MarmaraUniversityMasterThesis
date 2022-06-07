//çalışan ilk scriptler burda 

$(document).ready(() => {


    //var conn = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Information).withAutomaticReconnect().withUrl("https://localhost:5001/myhub").build();

    var conn = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/mq2hub").build();
    var connCFL = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/cflhub").build();
    var connLed = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/ledhub").build();
    var connFan = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/fanhub").build();
    var connAlarm = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/alarmhub").build();


    statusShow();
    start();
    function statusShow() {
        $('#conStatus').text(conn.connectionState);
    }
    function start() {
        //asenkron bu metod
        conn.start().then(() => {
            statusShow();
            $('#loading').hide();
            conn.invoke("ReceiveDataFromHub");


        }).catch((err) => {
            console.log(err);
            setTimeout(() => start(), 2000)

        });
        connCFL.start().then(() => {

            connCFL.invoke("ReceiveDataFromHub");

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



    $('#btnNameSave').click(() => {
        conn.invoke("SendName", $('#txtName').val()).catch((err) => console.log(err))
    })

    conn.on("ReceiveName", (name) => {
        $("#nameList").append(`<li class="list-group-item" > ${name} </li>`)
        console.log(name);
    })

    conn.on("ReceiveData", (data) => {

        $("#smoke").text(data.smoke);
        $("#co").text(data.co);
        $("#lpg").text(data.lpg);
    });

    connCFL.on("ReceiveData", (data) => {


        console.log(data);
    });

})
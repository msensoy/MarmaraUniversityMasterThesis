$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/alarmhub").build();

    start();

    function start() {
        connection.start().then(() => {
            connection.invoke("ReceiveDataFromHub");
        }).catch((err) => {
            console.log(err);
            setTimeout(() => start(), 2000)
        });
    }

    var changeAlarm01;
    var changeAlarm02;
    var changeAlarmVoice;

    connection.on("ReceiveData", (data) => {

        var image = document.getElementById('imgAlarm');
        if (data == true) {
            $('#bntAlarm').text("OFF");
            clearInterval(changeAlarm01);
            clearInterval(changeAlarm02);
            clearInterval(changeAlarmVoice);

            changeAlarm01 = setInterval(changeAlarm01Func, 500);
            changeAlarm02 = setInterval(changeAlarm02Func, 1000);
            changeAlarmVoice = setInterval(changeAlarmVoiceFunc, 1000);

        }
        else {
            $('#bntAlarm').text("ON");
            clearInterval(changeAlarm01);
            clearInterval(changeAlarm02);
            clearInterval(changeAlarmVoice);
            var audio = document.getElementById("audio");
            audio.pause();
            image.src = "/img/L4a0.png";
        }

        console.log("coming data from signalR for Alarm : " + data);
    });


    var changeAlarm01Func = function () {
        var image = document.getElementById('imgAlarm');
        image.src = "/img/L4a1.png";
    };
    var changeAlarm02Func = function () {
        var image = document.getElementById('imgAlarm');
        image.src = "/img/L4a2.png";
    };
    var changeAlarmVoiceFunc = function () {
        var audio = document.getElementById("audio");
        audio.pause();
        audio.play();
    };

})
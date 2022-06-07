$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl("https://192.168.43.36:5001/fanhub").build();

    start();

    function start() {
        connection.start().then(() => {
            connection.invoke("ReceiveDataFromHub");
        }).catch((err) => {
            console.log(err);
            setTimeout(() => start(), 2000)
        });
    }

    var degrees = 0;
    var setRotateFan;

    connection.on("ReceiveData", (data) => {

        if (data == true) {
            $('#bntFan').text("OFF");
            clearInterval(setRotateFan);

            setRotateFan = setInterval(rotateFanFunc, 100);
        }
        else {
            $('#bntFan').text("ON");
            clearInterval(setRotateFan);
        }

        console.log("coming data from signalR for Fan : " + data);
    });

    var rotateFanFunc = function () {
        degrees += 13;
        $('#imgFan').css({
            'transform': 'rotate(' + degrees + 'deg)',
            '-ms-transform': 'rotate(' + degrees + 'deg)',
            '-moz-transform': 'rotate(' + degrees + 'deg)',
            '-webkit-transform': 'rotate(' + degrees + 'deg)',
            '-o-transform': 'rotate(' + degrees + 'deg)'
        });
        if (degrees>360) {
            degrees =0;
        }
    };
})
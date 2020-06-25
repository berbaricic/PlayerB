
var vid = document.getElementById("myVideo");
//var videoSource = vid.getElementsByTagName("source")[0].src;
//var idVideo = "\"" + videoSource + "\"";
//event: PLAY
vid.onplay = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions';
    Http.open("POST", url);
    var date = new Date();
    date.toJSON;
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb14",
        "Status": "PLAY",
        "UserAdress": "225.225.225.1",
        "IdVIdeo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
////event: RESUME
vid.onplaying = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb14';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb14",
        "Status": "RESUME",
        "UserAdress": "225.225.225.1",
        "IdVIdeo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: PAUSE
vid.onpause = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb14';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb14",
        "Status": "PAUSE",
        "UserAdress": "225.225.225.1",
        "IdVIdeo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: SEEK
vid.onseeking = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb14';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb14",
        "Status": "SEEK",
        "UserAdress": "225.225.225.1",
        "IdVIdeo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: ENDED
vid.onended = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb14';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb14",
        "Status": "ENDED",
        "UserAdress": "225.225.225.1",
        "IdVIdeo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
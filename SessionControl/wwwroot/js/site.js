
var vid = document.getElementById("myVideo");
//to create and track different sessions, you need change Id from JSON body and Id in URL path
//event: PLAY
vid.onplay = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions';
    Http.open("POST", url);
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb15",
        "Status": "PLAY",
        "UserAdress": "225.225.225.1",
        "IdVideo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
////event: RESUME
vid.onplaying = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb15';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb15",
        "Status": "RESUME",
        "UserAdress": "225.225.225.1",
        "IdVideo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: PAUSE
vid.onpause = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb15';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb15",
        "Status": "PAUSE",
        "UserAdress": "225.225.225.1",
        "IdVideo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: SEEK
vid.onseeking = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb15';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb15",
        "Status": "SEEK",
        "UserAdress": "225.225.225.1",
        "IdVideo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
//event: ENDED
vid.onended = function () {
    const Http = new XMLHttpRequest();
    const url = 'https://localhost:44321/Sessions/12bb15';
    Http.open("PUT", url);
    var date = new Date();
    date.toJSON();
    Http.setRequestHeader('Content-Type', 'application/json');
    Http.send(JSON.stringify({
        "Id": "12bb15",
        "Status": "ENDED",
        "UserAdress": "225.225.225.1",
        "IdVideo": "video"
    }));

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText)
    }
};
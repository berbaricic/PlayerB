// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var vid = document.getElementById("myVideo");
vid.onplay = function () {
    alert("The video is now playing");
};
vid.onpause = function () {
    alert("The video has been paused");
};
vid.onseeking = function () {
    alert("Seek operation began!");
};
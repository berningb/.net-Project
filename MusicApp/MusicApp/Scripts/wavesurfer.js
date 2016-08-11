﻿var wavesurfer;

window.onload = function () {
    wavesurfer = WaveSurfer.create({
        container: '#waveform',
        waveColor: 'violet',
        progressColor: 'purple',
        height: 300,
        barWidth: 1,
        containerWidth: 1000
    });

    wavesurfer.load('../Content/MP3/Weekend.mp3');

    wavesurfer.on('ready', function () {
        wavesurfer.play();
        wavesurfer.skipLength = 10;
        wavesurfer.audioRate = 1;
    });


}

function muteSound() {
    wavesurfer.toggleMute();
    if (document.getElementById("muteUnmute").innerHTML == "Mute") {
        document.getElementById("muteUnmute").innerHTML = "Unmute";
    } else {
        document.getElementById("muteUnmute").innerHTML = "Mute";
    }
}
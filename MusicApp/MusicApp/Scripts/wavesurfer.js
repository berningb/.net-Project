var songPaths = new Array();
var songNames = new Array();
var names = [];

var list = document.getElementsByClassName("hiddenTitle");

var i;


window.onload = function () {

    for (i = 0; i < x.length; i++) {
        songPaths[i] = (document.getElementsByClassName('hidden')[i].innerHTML);
        songNames[i] = (document.getElementsByClassName('hiddenTitle')[i].innerHTML);
    }

    for (var p = 0; p < songNames.length; p++) {
        console.log(songNames[p]);
        names[p] = WaveSurfer.create({
            container: songNames[p],
            waveColor: 'black',
            progressColor: 'white',
            height: 300,
            barWidth: 5,
            hideScrollbar: true,
            containerWidth: 200
        });

        names[p].load('../Content/MP3/' + songPaths[p]);

        
    }

    for (var i = 0; i < 7; i++) {
        names[i].on('ready', function () {
            names[i].play();
            names[i].skipLength = 10;
            names[i].audioRate = 1;
        });
    }
  
}

function muteSound() {
    wavesurfer.toggleMute();
    if (document.getElementById("muteUnmute").innerHTML == "Mute") {
        document.getElementById("muteUnmute").innerHTML = "Unmute";
    } else {
        document.getElementById("muteUnmute").innerHTML = "Mute";
    }
}

function volumeChanged() {
    wavesurfer.setVolume(document.getElementById("volumeSlider").value);
}

function rewind() {
    wavesurfer.skipBackward(wavesurfer.skipLength);
}

function fastForward() {
    wavesurfer.skipForward(wavesurfer.skipLength);
}

function togglePlay() {
    wavesurfer.playPause();
    if (document.getElementById("playPause").innerHTML == "Play") {

        document.getElementById("playPause").innerHTML = "Pause";
    } else {
        document.getElementById("playPause").innerHTML = "Play";
    }
}

function speedUp() {
    wavesurfer.audioRate += .25;
    wavesurfer.setPlaybackRate(wavesurfer.audioRate);
}

function slowDown() {
    wavesurfer.audioRate -= .25;
    if (wavesurfer.audioRate <= 0) {
        wavesurfer.audioRate = .25;
    }
    wavesurfer.setPlaybackRate(wavesurfer.audioRate);
}
    
function restart(a) {
    console.log(a);
    wavesurfer.stop();
    document.getElementById("playPause").innerHTML = "Play";
}
var songPaths = new Array();
var songNames = new Array();
var names = [];

var list = document.getElementsByClassName("hiddenTitle");

var i;


window.onload = function () {

    for (i = 0; i < list.length; i++) {
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

    names[0].on('ready', function () {
        names[0].play();
        names[0].skipLength = 10;
        names[0].audioRate = 1;
    });

    //names[1].on('ready', function () {
    //    names[1].play();
    //    names[1].skipLength = 10;
    //    names[1].audioRate = 1;
    //});

    
  
}


function muteSound() {
    names[0].toggleMute();
    if (document.getElementById("muteUnmute").innerHTML == "Mute") {
        document.getElementById("muteUnmute").innerHTML = "Unmute";
    } else {
        document.getElementById("muteUnmute").innerHTML = "Mute";
    }
}

function volumeChanged() {
    names[0].setVolume(document.getElementById("volumeSlider").value);
}

function rewind() {
    names[0].skipBackward(names[0].skipLength);
}

function fastForward() {
    names[0].skipForward(names[0].skipLength);
}

function togglePlay() {
    names[0].playPause();
    if (document.getElementById("playPause").innerHTML == "Play") {

        document.getElementById("playPause").innerHTML = "Pause";
    } else {
        document.getElementById("playPause").innerHTML = "Play";
    }
}

function speedUp() {
    names[0].audioRate += .25;
    names[0].setPlaybackRate(names[0].audioRate);
}

function slowDown() {
    names[0].audioRate -= .25;
    if (names[0].audioRate <= 0) {
        names[0].audioRate = .25;
    }
    names[0].setPlaybackRate(names[0].audioRate);
}
    
function restart(a) {
    names[0].stop();
    document.getElementById("playPause").innerHTML = "Play";
}
window.onload = function () {
    var thing = document.getElementById("hiddenSong").innerHTML;
    console.log(thing);
    wave = WaveSurfer.create({
        container: '#songWave',
        waveColor: 'black',
        progressColor: 'white',
        height: 300,
        barWidth: 5,
        hideScrollbar: true,
        containerWidth: 200
    });
    getSong(0, thing);

    wave.on('ready', function () {
        wave.skipLength = 10;
        wave.audioRate = 1;
        wave.play();
    });

}

function getSong(id, thing2) {
    wave.load('/../Content/MP3/' + thing2);
}

function muteSound() {
    wave.toggleMute();
    if (document.getElementById("muteUnmute").innerHTML == "Mute") {
        document.getElementById("muteUnmute").innerHTML = "Unmute";
    } else {
        document.getElementById("muteUnmute").innerHTML = "Mute";
    }
}

function volumeChanged() {
    wave.setVolume(document.getElementById("volumeSlider").value);
}

function rewind() {
    wave.skipBackward(wave.skipLength);
}

function fastForward() {
    wave.skipForward(wave.skipLength);
}

function togglePlay() {
    wave.playPause();
    if (document.getElementById("playPause").innerHTML == "Play") {
        document.getElementById("playPause").innerHTML = "Pause";
    } else {
        document.getElementById("playPause").innerHTML = "Play";
    }
}

function speedUp() {
    wave.audioRate += .25;
    wave.setPlaybackRate(wave.audioRate);
}

function slowDown() {
    wave.audioRate -= .25;
    if (wave.audioRate <= 0) {
        wave.audioRate = .25;
    }
    wave.setPlaybackRate(wave.audioRate);
}

function restart() {
    wave.stop();
    wave.play();
    document.getElementById("playPause").innerHTML = "Play";
}



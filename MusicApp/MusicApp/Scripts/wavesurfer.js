var wavesurfer;

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

function restart() {
    wavesurfer.stop();
    document.getElementById("playPause").innerHTML = "Play";
}


var i;
var obj;
var wave;
var r;

window.onload = function () {
    console.log('hey');
    var thing = document.getElementById("hidden").innerHTML;
    console.log(thing)
    obj = eval("(" + thing + ")");

    console.log(obj);
    wave = WaveSurfer.create({
        container: '#wave',
        waveColor: 'black',
        progressColor: 'white',
        height: 300,
        barWidth: 5,
        hideScrollbar: true,
        containerWidth: 200
    });

    getSong(0);

    wave.on('ready', function () {
        wave.skipLength = 10;
        wave.audioRate = 1;
        wave.play();
    });

    createSongList();


}


function getSong(id) {
    console.log(id);
    wave.load('../Content/MP3/' + obj.Songs[id].SongFileName);
}

function createSongList() {
    console.log(obj);
    var item;
    for (var i = 0; i < obj.Songs.length; i++) {
        var songDiv = document.createElement('div');
        songDiv.className = 'songDiv';
        songDiv.style.width = '100%';
        songDiv.style.height = '200px';

        var songList = document.getElementById('songList');
        songList.appendChild(songDiv);
        

        var songName = document.createElement('h1');
        var aTag = document.createElement('a');
        aTag.setAttribute('href', 'Song/' + obj.Songs[i].Title)
        songName.className = 'songName';
        songName.textContent = obj.Songs[i].Title
        aTag.appendChild(songName);
        songDiv.appendChild(aTag);


        var songImage = document.createElement('img');
        songImage.className = 'songImage img-circle';
        songImage.src = '../Content/Images/' + obj.Songs[i].ImageFileName
        songImage.style.width = '100px';
        songImage.style.height = '100px';
        songDiv.appendChild(songImage);


       
        var play = document.createElement('button');
        play.textContent = 'Play Song';
        play.className = 'songPlay'
        var r = obj.Songs[i].SongFileName;
        (function (r) {
            play.onclick = function () {
                wave.load('../Content/MP3/' + r);
            };
        })(r);
        songDiv.appendChild(play);
    }
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



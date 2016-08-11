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
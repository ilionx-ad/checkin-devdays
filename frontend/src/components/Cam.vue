
<template>
<div class="cam-container">
  <div class="camera">
    <div v-if="streaming">
        <h2 class="counter" >{{timeLeft}}</h2>
    </div>
    <video id="video" ref="video">Video stream not available.</video>
    <canvas id="canvas" ref="canvas" hidden></canvas>
    <img id="photo" ref="photo">
  </div>
</div>
</template>

<script>
export default {
  name: 'Cam',
  data() {
      return{
        video: {},
        canvas: {},
        photo: {},
        width: 320,
        height: 0,
        streaming: null,
        timeLeft: null,
        intervalTimer: null
      }
  },
  mounted() {
      this.startup();
    },
    methods: { 
        startup(){
            this.video = this.$refs.video;
            this.canvas = this.$refs.canvas;
            this.photo = this.$refs.photo;

            if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia({ video: true }).then(stream => {
                    try{
                        this.video.srcObject = stream;
                        this.video.play();            
                    }catch(error){
                        console.log("An error occurred: " + error);
                        this.video.src = window.URL.createObjectURL(stream);
                    }
                });
            }else{
                console.log("Media device unavailable...");
            }

            this.video.addEventListener('canplay', (ev) => {
                this.height = this.video.videoHeight / (this.video.videoWidth/this.width);

                if (isNaN(this.height)) {
                this.height = this.width / (4/3);
                }

                this.video.setAttribute('width', this.width);
                this.video.setAttribute('height', this.height);

                this.setTime(3);

                setTimeout(() => {
                    this.takepicture(); 
                }, 3000);
            }, false);
        },
        takepicture() {
            const context = this.canvas.getContext('2d');

            this.canvas.width = this.width;
            this.canvas.height = this.height;
            context.drawImage(this.video, 0, 0, this.width, this.height);

            const data = this.canvas.toDataURL('image/png');
            this.displaypicture(data);
        },
        displaypicture(data) {
            this.photo.setAttribute('src', data);
            let tracks = this.video.srcObject.getTracks();

            tracks.forEach(function(track) {
                track.stop();
            });

            this.video.srcObject = null;

            this.canvas.hidden = true;
            this.video.hidden = true;
        },
        setTime(seconds) {
            this.streaming = true;
            clearInterval(this.intervalTimer);
            this.timer(seconds);
        },
        timer(seconds) {
            const now = Date.now();
            const end = now + seconds * 1000;
            this.displayTimeLeft(seconds);
            this.countdown(end);
        },
        countdown(end) {
            this.intervalTimer = setInterval(() => {
                const secondsLeft = Math.round((end - Date.now()) / 1000);

                if(secondsLeft === 0) {
                    this.streaming = false;
                }

                if(secondsLeft < 0) {
                    clearInterval(this.intervalTimer);
                    return;
                }
                this.displayTimeLeft(secondsLeft)
            }, 1000);
        },
        displayTimeLeft(secondsLeft) {
            const minutes = Math.floor((secondsLeft % 3600) / 60);
            const seconds = secondsLeft % 60;
            this.timeLeft = `${seconds}`;
        }
    }
}
</script>

<style lang="scss">
.camera {
    position: relative;
    width: 320px;
    margin:10px;
}
.counter {
    position: absolute;
    transform: translate(-50%, -50%);
    color: white;
    opacity: 0.8;
    top: 15%;
    right: 35%;
    z-index: 1;
    line-height: 1;
}
</style>

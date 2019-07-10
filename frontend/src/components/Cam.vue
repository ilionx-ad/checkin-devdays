
<template>
<div ref="mask" class="modal-mask">
    <div class="modal-wrapper">
    <div class="modal-container">

        <div class="modal-header">
        <slot name="header">
            <h3>Say Cheese!</h3>
        </slot>
        </div>

        <div class="modal-body">
        <slot name="body"></slot>
        <div ref="box" class="video-box">
            <h2 v-if="showTimer" class="counter">{{timeLeft}}</h2>
            <h2 v-if="!showTimer" class="counter-cheese">{{timeText}}</h2>
            <canvas ref="canvas" id="canvas" width="640" height="480"></canvas>
            <video ref="video" id="video" width="640" height="480" autoplay></video>
            <div class="video-controls" ref="button">
                <div class="video-snap">
                    <!-- Handmatig een selfie maken
                    <button id="snap" class="btn-snap" v-on:click="capture()">
                        <img src="../assets/baseline-photo_camera-24px.svg" />
                    </button>
                    -->
                </div>
                <a class="button button-close-modal" v-on:click="closeModal()">Annuleren</a>
            </div>
        </div>
        </div>

        <div class="modal-footer">
        <slot name="footer"></slot>
        </div>
    </div>
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
        captures: [],
        selectedTime: 0,
        timeText: 'Kaas!',
        showTimer: true,
        timeLeft: '0',
        endTime: '0',
        intervalTimer: null
      }
  },
  mounted() { 
        this.video = this.$refs.video;
        this.$refs.canvas.hidden = true;
        if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            navigator.mediaDevices.getUserMedia({ video: true }).then(stream => {
                try{
                    this.video.srcObject = stream
                }catch(error){
                    this.video.src = window.URL.createObjectURL(stream);
                }
                this.video.play();            
            });
        }
        this.capture();
        
    },
    methods: { 
    setTime(seconds) {
        clearInterval(this.intervalTimer);
        this.timer(seconds);
    },
    timer(seconds) {
        const now = Date.now();
        const end = now + seconds * 1000;
        this.displayTimeLeft(seconds);

        this.selectedTime = seconds;
        this.countdown(end);
    },
    countdown(end) {
        intervalTimer = setInterval(() => {
            const secondsLeft = Math.round((end - Date.now()) / 1000);

            if(secondsLeft === 0) {
                this.showTimer = false;
                this.endTime = 0;
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
    },
    closeModal(){
        this.$emit('close');
        this.video = this.$refs.video;
        let tracks = this.video.srcObject.getTracks();

        tracks.forEach(function(track) {
            track.stop();
        });

        this.video.srcObject = null;
    },
    capture() {
        this.setTime(3);
        
        this.canvas = this.$refs.canvas; 
                
        var context = this.canvas.getContext("2d").drawImage(this.video, 0, 0, 640, 480);
        this.captures.push(canvas.toDataURL("image/webp"));
        this.video = this.$refs.video;
        let tracks = this.video.srcObject.getTracks();

        tracks.forEach(function(track) {
            track.stop();
        });

        this.video.srcObject = null;
        this.$refs.canvas.hidden = false;
        this.video.hidden = true;
        this.$refs.button.hidden = true;
        this.$emit('close');  
        }
        
    }
}
</script>

<style lang="scss">
.counter-cheese{
    position: absolute;
    top: 15%;
    left: 35%;
    color: white;
}
.counter{
    position: absolute;
    top: 15%;
    left: 45%;
    color: white;
}
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  display: table;
  transition: opacity .3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 700px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
  transition: all .3s ease;
}

.modal-header h3 {
  margin-top: 0;
  text-align: center;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

svg{
    fill:white
}
.image-ul{
    list-style:none;
}
.video-box{
    position:relative;
}
.video-snap{
    text-align: center;
    padding-bottom:60px;
}
.video-controls{
    margin-top: -75px;
    padding-bottom: 80px;
}
.btn-snap{
    position: relative;
    color: white;
    background-color: rgba(6, 6, 6, 0.5);
    border: 0;
    height: 55px;
    width: 55px;
    border-radius: 50%;
}
a.button-close-modal{
    background: url(../assets/baseline_clear_white_18dp.png) center right 30px no-repeat #9c9c9c !important;
    float: right;
    padding-top: 20px;
    margin-top: 30px;
}
</style>

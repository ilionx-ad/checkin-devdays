
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
            <canvas ref="canvas" id="canvas" width="640" height="480"></canvas>
            <video ref="video" id="video" width="640" height="480" autoplay></video>
            <div class="video-controls" ref="button">
                <div class="video-snap">
                    <button id="snap" class="btn-snap" v-on:click="capture()">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><circle cx="12" cy="12" r="3.2"/><path d="M9 2L7.17 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2h-3.17L15 2H9zm3 15c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
                    </button>
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
        captures: []
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
    },
    methods: { 
        closeModal(){
            this.$emit('close');
            this.video = this.$refs.video;
            let tracks = this.video.srcObject.getTracks();

            tracks.forEach(function(track) {
                track.stop();
            });

            this.video.srcObject = null;
            //this.getElementById(snap).display = "none";
        },
        capture() {
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
        //getElementById(snap).display = "none";
        // this.video.srcObject.stop()
        }
        
    }
}
</script>

<style lang="scss">
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

.video-snap{
    text-align: center;
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

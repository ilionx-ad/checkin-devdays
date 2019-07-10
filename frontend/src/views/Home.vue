<template>
  <div class="home">
    <h1>Welkom bij de<br />ilionx DevDays 2019</h1>
    <h2>Laat weten dat je er bent!</h2>
    <a class="button">Check nu in!</a>
    <a class="button button-open-modal" id="show-modal" @click="showModal = true">Take a photo!</a>   
    <Cam v-if="showModal" @show="showModal = false" v-on:close="showModal = false"/> 
  </div>
</template>


<style lang="scss" scoped>
  h1, h2 {
    margin: 0;
  }

  h2 {
    color: $color-secondary;
    font-size: 60px;
  }
  a.button{
    -webkit-background-size: 20px 20px;
    background-size: 20px;
    border: 0;
    margin-left: 10px;
    padding: 20px 70px 20px 30px;
    color: #fff;
    -webkit-transition: all .2s ease;
    -o-transition: all .2s ease;
    transition: all .2s ease;
    text-decoration: none;
    font-size: 18px;
    font-size: 1.8rem;
    line-height: 30px;
    line-height: 3rem;
    cursor: pointer;
    float: left;
    min-width: 250px;
    text-align: left;
  }
  a.button-open-modal{
    background: url(../assets/baseline_tag_faces_white_18dp.png) center right 30px no-repeat #E41D29 !important;
  }
</style>


<script>
import Cam from '@/components/Cam.vue'

export default {
  name: 'home',
  components: {
    Cam
  },
  data(){
    return{
      showModal: false
    }
  }
}
</script>

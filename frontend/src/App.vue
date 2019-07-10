<template>
  <div class="check-in">
    <Header />
    <div class="check-in__inner">
      <div class="check-in__content">
        <router-view/>
      </div>
    </div>
  </div>
</template>

<style lang="scss">
  .check-in {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-image: url(assets/x-background.png);
    background-repeat: no-repeat;
    background-size: cover;
    display: flex;
    align-items: center;

    &__inner {
      max-width: $container;
      width: 100%;
      margin: 0 auto;
      padding: 70px 15px 0 15px;
    }

    &__content {
      width: 66.66667%;
    }
  }
</style>


<script>
import Header from '@/components/Header.vue'

export default {
  components: {
    Header
  }
}
</script>


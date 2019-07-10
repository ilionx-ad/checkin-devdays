<template>
  <header class="header">
    <div class="header__inner">
      <img class="header__image" src="../assets/ilionx-logo.svg" />
    </div>
  </header>
</template>

<script>
export default {
  name: 'Header'
}
</script>
<style scoped lang="scss">
  .header {
    position: fixed;
    top: 0;
    left: 0;

    &__inner {
      max-width: $container;
      width: 100%;
      margin: 0 auto;
      padding: 70px 15px 0 15px;
    }

    &__image {
      display: inline-block;
    }
  }
</style>

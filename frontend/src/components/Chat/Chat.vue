<template>
  <div>
    <div v-for="item in this.$store.getters.all" v-bind:key="item.id">
      <div class="wrapper">
        <div class="balloon">{{item.question}}</div>
      </div>
      <div class="wrapper right">
        <div class="balloon green">{{item.answer}}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Chat"
};
</script>

<style scoped lang="scss">
.balloon {
  border-radius: 20px;
  line-height: 20px;
  display: inline-block;
  background: blue;
  padding: 10px;
  color: white;
}

.balloon.green {
  background: green;
}

.wrapper {
  display: flex;
  flex-direction: column;
  margin: 15px;
  align-items: flex-start;
}

.wrapper.right {
  align-items: flex-end;
}
</style>

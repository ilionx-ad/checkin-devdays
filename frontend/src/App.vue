<template>
  <div class="check-in">
    <Header />
    <div class="check-in__inner">
      <div class="check-in__content">
        <router-view />
      </div>
    </div>
  </div>
</template>

<style lang="scss">
.check-in {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-image: url(assets/x-background.png);
  background-repeat: no-repeat;
  background-size: cover;
  &__inner {
    max-width: $container;
    width: 100%;
    height: 100%;
    margin: 0 auto;
    padding: 150px 15px;
  }

  &__content {
    display: flex;
    min-height: 100%;
  }
}
</style>


<script>
import Header from "@/components/Header.vue";
import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {
    messages: []
  },
  getters: {
    all: state => state.messages
  },
  mutations: {
    add(state, response) {
      const item = {
        id: Date.now(),
        question: response.queryResult.queryText,
        answer: response.queryResult.fulfillmentText
      };

      state.message = state.messages.push(item);
    }
  }
});

export default {
  store,
  components: {
    Header
  }
};
</script>


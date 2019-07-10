

<template>
  <div class="home">
    <h1>
      Welkom bij de
      <br />ilionx DevDays 2019
    </h1>
    <h2>Laat weten dat je er bent!</h2>
    <router-link @click="setIdInStorage()" class="button" to="/dialog">Check nu in!</router-link>
  </div>
</template>

<style lang="scss" scoped>
.home {
  align-self: center;
}
h1,
h2 {
  margin: 0;
}

h2 {
  color: $color-secondary;
  font-size: 60px;
}
</style>


<script>
import * as uuidv1 from "uuid/v1";

export default {
  name: "home",
  methods: {
    setIdInStorage: () => {
      sessionStorage.setItem("endBossId", uuidv1());
    }
  }
};
</script>

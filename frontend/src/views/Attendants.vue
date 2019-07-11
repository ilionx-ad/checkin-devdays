<template>
  <div class="attendants">
    <h1>Wie zijn er allemaal?</h1>
    
    <dl v-for="attendant in attendants" :key="attendant.id">
        <dd>TODO: {{get(attendant, 'queryResult.parameters.name') || get(attendant, 'data')}}</dd>
        <dd>{{attendant.photo}}</dd>
    </dl>

    <router-link class="button-small" to="/">terug</router-link>
  </div>
</template>

<style lang="scss" scoped>
  h1, h2 {
    margin: 0;
  }
</style>


<script>
import repo from '../repositories/endBossesRepository'
import get from 'lodash/get'

export default {
  name: 'attendants',
  data() {
    return {
        isLoading: true,
        attendants: [],
        get
    };

  },
  async created() {
    await this.getEndbosses();
  },
  methods: {
      async getEndbosses() {
          this.isLoading = true;
          const { data } = await repo.getAll();
          this.isLoading = false;
          this.attendants = data;
      }
  }
}
</script>
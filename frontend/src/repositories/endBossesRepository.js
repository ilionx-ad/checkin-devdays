import Repository from "../utils/Repository";

const resource = "/EndBosses";
const endBossID = sessionStorage.getItem("endBossID");

export default {
  get() {
    return Repository.get(`${resource}/${endBossID}`);
  },
  post(payload) {
    return Repository.post(`${resource}/${endBossID}`, payload);
  },
  put(payload) {
    return Repository.put(`${resource}/${endBossID}`, payload);
  },
  delete() {
    return Repository.delete(`${resource}/${endBossID}`);
  }
};

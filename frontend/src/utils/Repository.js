import axios from "axios";

const baseDomain = "https://ilionx-devdays-welcome-api.azurewebsites.net";
const baseApiUrl = `${baseDomain}/api`;

export default axios.create({
  baseURL: baseApiUrl
});

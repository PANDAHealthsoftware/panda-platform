import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:44372/api', // Update to match PANDA.Api's actual URL
});

export default api;
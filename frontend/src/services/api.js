import axios from 'axios';

// The API base url can be set through the Vite env var `VITE_API_BASE_URL`.
// If not set, we fall back to the local development default.
// The local backend dev server listens on HTTP 5219 and HTTPS 7164 by default.
// Use HTTPS (https://localhost:7164/api) by default so the browser does not trigger
// an HTTP -> HTTPS redirection that breaks CORS preflight requests.
// You can override with VITE_API_BASE_URL in frontend/.env.
const base = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7164/api';

// withCredentials:true is enabled so the client will send cookies (if you want
// to rely on cookie-based auth in the future). The backend CORS policy allows
// credentials for the dev origin.
const api = axios.create({
  baseURL: base,
  withCredentials: true,
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;

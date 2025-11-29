import api from './api';

const authService = {
  async login(credentials) {
    // Backend route in TravelBuddy.Api: POST /api/users/login
    const res = await api.post('/users/login', credentials);
      console.log("login:", res.data)
    return res.data; // expects { token }
  },

  async register(payload) {
    // Backend route in TravelBuddy.Api: POST /api/users/register
    const res = await api.post('/users/register', payload);
      console.log("register:", res.data)
    return res.data;
  },

  // Fetch a user by id using backend endpoint: GET /api/users/{id}
  async me(userId) {
    if (!userId) throw new Error('userId is required for me()');
    const res = await api.get(`/users/${userId}`);
    console.log("getUserInfo: ", res.data)
    return res.data;
  }
  ,

  async logout() {
    // Backend route in TravelBuddy.Api: POST /api/users/logout
    // This clears the HttpOnly cookie server-side, so it's important to call it.
    const res = await api.post('/users/logout');
    return res.data;
  }
};

export default authService;
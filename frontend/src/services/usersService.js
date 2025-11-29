import api from './api';

const usersService = {
  async getAll() {
    const res = await api.get('/users');
    console.log("getAllUsers: ", res.data)
    return res.data;
  }
}

export default usersService;
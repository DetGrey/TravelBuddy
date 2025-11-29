import api from './api';

const messagesService = {
  // Backend requires a query param userId for these endpoints
  async getConversations(userId) {
    const res = await api.get('/conversations', { params: { userId } });
    console.log("getConversations: ", res.data)
    return res.data;
  },

  async getConversation(id, userId) {
    // Fetch conversation detail first, then fetch messages for the conversation
    const [detailRes, messagesRes] = await Promise.all([
      api.get(`/conversations/${id}`, { params: { userId } }),
      api.get(`/conversations/${id}/messages`, { params: { userId } }),
    ]);
    console.log('getConversation detail:', detailRes.data);
    console.log('getConversation messages:', messagesRes.data);
    // Merge into a convenient structure the frontend expects
    return { ...detailRes.data, messages: messagesRes.data };
  },

  async sendMessage(conversationId, userId, payload) {
    // payload expected: { content: 'message text' }
    const res = await api.post(`/conversations/${conversationId}/messages`, payload, { params: { userId } });
    console.log("sendMessage: ", res.data)
    return res.data;
  },

  // TODO: backend does not implement a `POST /api/conversations` endpoint to create
  // new conversations. Once added, implement this helper (likely needs a userId body param).
  async startConversation(payload) {
    const res = await api.post('/conversations', payload);
    console.log("startConversation: ", res.data)
    return res.data; // { conversationId }
  }
}

export default messagesService;
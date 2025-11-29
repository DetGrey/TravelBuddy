import api from './api';

const tripsService = {
    // NOTE: Backend does not expose a create trip/trip-destination endpoint right now.
    // TODO: Add POST /api/trip-destinations on backend and enable this helper.
    async createTrip(payload) {
      const res = await api.post('/trip-destinations', payload);
      console.log("createTrip: ", res.data)
      return res.data;
    },

    // Search trips using backend endpoint: GET /api/trip-destinations/search
    async search(filters) {
      // 1. Create a URLSearchParams object from the filters object.
      // This correctly formats the key/value pairs into a query string (e.g., ?q=test&country=USA).
      const params = new URLSearchParams(filters);
      
      // 2. Use api.get, passing the parameters object which will be appended 
      // to the URL as a query string by the API client (e.g., Axios).
      // Note: We are using the 'params' key to pass the constructed query parameters.
      const res = await api.get('/trip-destinations/search', { 
          params: params
      });
      
      console.log("search: ", res.data);
      return res.data;
    },

    // Get trip overview/info by id: GET /api/users/{userId}/trips/{tripId}
    async getFullTripOverview(userId, tripId) {
      const res = await api.get(`/users/${userId}/trips/${tripId}`);
      console.log("getFullTripOverview: ", res.data)
      return res.data;
    },

    // If you need an endpoint for fetching one trip's details add GET /api/trip-destinations/{id}
    async getTripDestinationInfo(userId, id) {
      const res = await api.get(`/users/${userId}/trips/trip-destinations/${id}`);
      console.log("getTripDestinationInfo: ", res.data)
      return res.data;
    },

    // Get the trips for a specific user: GET /api/users/{userId}/trips/trip-destinations
    async getMyTrips(userId) {
      const res = await api.get(`/users/${userId}/trips/trip-destinations`);
      console.log("getMyTrips: ", res.data)
      return res.data;
    },

    // Request to join for a specific user: POST /api/users/{userId}/buddy-requests
    // Body should be a BuddyDto (backend will set the UserId from the route).
    async requestJoin(userId, tripDestinationId, payload = {}) {
      const body = { tripDestinationId, ...payload };
      const res = await api.post(`/users/${userId}/buddy-requests`, body);
      console.log("requestJoin: ", res.data)
      return res.data;
    },

    // Leave trip or remove buddy. Route: DELETE /api/users/{userId}/trips/trip-destinations/{tripDestinationId}/leave
    async leaveTrip(targetUserId, tripDestinationId, departureReason) {
      const res = await api.delete(`/users/${targetUserId}/trips/trip-destinations/${tripDestinationId}/leave`, { params: { departureReason } });
      console.log("leaveTrip: ", res.data)
      return res.data;
    },

    // Accept/reject map to: POST /api/users/{userId}/buddy-requests/update
    // Body: { userId, buddyId, newStatus }
    async acceptRequest(userId, buddyId) {
      const res = await api.post(`/users/${userId}/buddy-requests/update`, {
        userId,
        buddyId,
        newStatus: 'Accepted'
      });
      console.log("acceptRequest: ", res.data)
      return res.data;
    },

    async rejectRequest(userId, buddyId) {
      const res = await api.post(`/users/${userId}/buddy-requests/update`, {
        userId,
        buddyId,
        newStatus: 'Rejected'
      });
      console.log("rejectRequest: ", res.data)
      return res.data;
    },

    // Remove buddy is implemented by DELETE leave route for the target user.
    async removeBuddy(tripDestinationId, targetUserId) {
      // Frontend should pass the target user's id (the one to remove) and trip id.
      const res = await api.delete(`/users/${targetUserId}/trips/trip-destinations/${tripDestinationId}/leave`);
      console.log("removeBuddy: ", res.data)
      return res.data;
    },

    async getRequests(userId) {
      // Backend provides GET /api/users/{userId}/buddy-requests/pending (all pending for user)
      // The frontend will pass owner user id as the argument and then filter by trip id as needed.
      const res = await api.get(`/users/${userId}/buddy-requests/pending`);
      console.log("getRequests: ", res.data)
      return res.data;
    }
};

export default tripsService;
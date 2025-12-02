import api from './api';

const tripsService = {
    // Create a new trip for a user: POST /api/users/{userId}/trips
    async createTrip(userId, payload) {
      try {
        console.log("createTrip payload: ", payload);
        const res = await api.post(`/users/${userId}/trips`, payload);
        return;
      } catch (err) {
        console.error("Failed to create trip:", err.response?.data || err.message);
        throw err;
      }
    },

    async getDestinations() {
      const res = await api.get('/destinations');
      console.log("getDestinations: ", res.data)
      return res.data; // returns array of DestinationDto
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
    async getBuddyTrips(userId) {
      const res = await api.get(`/users/${userId}/trips/trip-destinations/buddy`);
      console.log("getBuddyTrips: ", res.data)
      return res.data;
    },
    async getOwnedTrips(userId) {
      const res = await api.get(`/users/${userId}/trips/trip-destinations/owned`);
      console.log("getOwnedTrips: ", res.data)
      return res.data;
    },

    // Request to join for a specific user: POST /api/users/{userId}/buddy-requests
    // Body should be a BuddyDto (backend will set the UserId from the route).
    async requestJoin(userId, tripDestinationId, personCount, note) {
        // 1. Construct the BuddyDto body structure
        const body = { 
            userId: userId,
            tripDestinationId: tripDestinationId,
            personCount: personCount,
            note: note.length > 0 ? note : null
        };
        console.log("requestJoin body: ", body);
        // 2. Make the POST request to the correct endpoint path
        try {
          const res = await api.post(`/users/${userId}/buddy-requests`, body);
          console.log("requestJoin status:", res.status);
          return res.status === 201;
        } catch (error) {
          // Axios error shape: error.response.data contains your server's message
          const status = error.response?.status;
          const data = error.response?.data;

          console.error("requestJoin failed:", {
            status,
            data,
            message: error.message,
          });

          // Return false or rethrow, depending on your flow
          return false;
        }

    },

    // Leave trip or remove buddy. Route: DELETE /api/users/{userId}/trips/trip-destinations/{tripDestinationId}/leave
    async leaveTrip(targetUserId, tripDestinationId, departureReason) {
      const res = await api.delete(`/users/${targetUserId}/trips/trip-destinations/${tripDestinationId}/leave`, { params: { departureReason } });
      console.log("leaveTrip: ", res.data)
      return res.data;
    },

    // Accept/reject map to: PATCH /api/users/{userId}/buddy-requests/update
    // Body: { userId, buddyId, newStatus }
    async acceptRequest(userId, buddyId) {
      console.log("acceptRequest called with userId:", userId, "buddyId:", buddyId);
      try {
        const res = await api.patch(`/users/${userId}/buddy-requests/update`, {
          buddyId,
          newStatus: 'accepted'
        });
        console.log("acceptRequest: ", res.data)
        return res.data;

      } catch (error) {
        const status = error.response?.status;
        const data = error.response?.data;

        console.error("acceptRequest failed with status:", status, "data:", data);
        return false;
      }
  },

    async rejectRequest(userId, buddyId) {
      console.log("rejectRequest called with userId:", userId, "buddyId:", buddyId);
      const res = await api.post(`/users/${userId}/buddy-requests/update`, {
        buddyId,
        newStatus: 'rejected'
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
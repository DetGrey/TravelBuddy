import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import NavBar from '../components/NavBar';
import tripsService from '../services/tripsService';
import { AuthContext } from '../context/AuthContext';
import DestinationStopOverview from '../components/TripDestinationOverview'; // Assumed component for itinerary

const TripDetailsPage = () => {
  // 'id' here is the TripDestinationId (e.g., /trip-destinations/123)
  const { id: tripDestinationId } = useParams(); 
  const { user } = useContext(AuthContext);

  // 1. State for the main destination stop details (V_TripDestinationInfo result)
  const [tripDestination, setTripDestination] = useState(null); 
  
  // 2. State for the overall trip summary (TripAggregateModel result)
  const [tripOverview, setTripOverview] = useState(null); 

  const [loadingMain, setLoadingMain] = useState(true);
  const [message, setMessage] = useState(null);

  // --- Action Handlers (Defined here for simplicity, typically in a Hook/Service) ---
  
  const fetchData = async () => {
    try {
      if (!user?.id) throw new Error('Missing user id — login first');
      
      // 1. Fetch the main, detailed stop info (Action-focused data)
      const destinationData = await tripsService.getTripDestinationInfo(user.id, tripDestinationId);
      setTripDestination(destinationData);
      setMessage(null);

      // 2. Once we have the destination, fetch the overall trip data using the TripId
      if (destinationData?.tripId) {
        const overviewData = await tripsService.getFullTripOverview(user.id, destinationData.tripId);
        setTripOverview(overviewData);
      }

    } catch (err) {
      setMessage(err?.response?.data?.message || err.message || 'Failed to load trip details.');
    } finally {
      setLoadingMain(false);
    }
  };

  const onRequest = async () => {
    try {
      if (!user?.id) throw new Error('Missing user id — login first');
      await tripsService.requestJoin(tripDestinationId, { personCount: 1 });
      setMessage('Join request sent! Awaiting owner approval.');
      await fetchData(); 
    } catch (e) {
      setMessage(e?.response?.data?.message || e.message || 'Failed to send join request.');
    }
  };

  const onLeave = async () => {
    try {
      if (!user?.id) throw new Error('Missing user id — login first');
      await tripsService.leaveTrip(tripDestinationId); 
      setMessage('You have successfully left this trip destination.');
      await fetchData();
    } catch (e) {
      setMessage(e?.response?.data?.message || e.message || 'Failed to leave trip destination.');
    }
  };

  const onHandleRequest = async (buddyId, action) => {
    try {
      if (action === 'accept') {
          await tripsService.acceptBuddy(buddyId);
          setMessage('Buddy request accepted!');
      } else {
          await tripsService.rejectBuddy(buddyId);
          setMessage('Buddy request rejected.');
      }
      await fetchData();
    } catch (e) {
      setMessage(e?.response?.data?.message || e.message || 'Failed to handle buddy request.');
    }
  };

  const onRemoveBuddy = async (buddyId) => {
    try {
      await tripsService.removeBuddy(buddyId); 
      setMessage('Buddy successfully removed from this destination.');
      await fetchData();
    } catch (e) {
      setMessage(e?.response?.data?.message || e.message || 'Failed to remove buddy.');
    }
  };
  // --- END Action Handlers ---

  useEffect(() => {
    let mounted = true;
    if (mounted && tripDestinationId) {
      setLoadingMain(true);
      fetchData();
    }
    return () => (mounted = false);
  }, [tripDestinationId, user?.id]);


  if (loadingMain) return (
    <div>
      <NavBar />
      <div className="container mt-5 text-center">
        <div className="spinner-border text-primary" role="status"></div>
        <p className="mt-2 text-muted">Loading trip stop and overview...</p>
      </div>
    </div>
  );

  if (!tripDestination) return (
    <div>
      <NavBar />
      <div className="container mt-5">
        <div className="alert alert-warning text-center" role="alert">
          <i className="fas fa-exclamation-triangle me-2"></i> Destination stop not found.
        </div>
      </div>
      <p className="text-center text-danger mt-3">{message}</p>
    </div>
  );

  const td = tripDestination;
  const currentUserId = String(user?.id);
  const isOwner = String(td.ownerUserId) === currentUserId;
  
  // Check if user is an accepted buddy for this specific destination stop
  const isBuddy = td.acceptedBuddies.some(b => String(b.buddyUserId) === currentUserId);
  
  // Check if user has a pending request for this specific destination stop
  const hasPendingRequest = td.pendingRequests.some(r => String(r.requesterUserId) === currentUserId);

  // Calculate the total number of people from accepted buddies by summing personCount
  const totalAcceptedPeople = td.acceptedBuddies.reduce(
    (sum, buddy) => sum + buddy.personCount, 
    0
  );

  // Location display logic
  const locationDisplay = [
    td.destinationCountry,
    td.destinationState ? ` (${td.destinationState})` : null 
  ].filter(Boolean).join('');


  return (
    <div>
      <NavBar />
      <div className="container mt-4">
        {/* --- EXISTING DESTINATION STOP HEADER SECTION --- */}
        <div className="text-center mb-4 pb-3 border-bottom">
          <h1 className="display-4 text-primary mb-1">{td.destinationName}</h1>
          <p className="lead text-muted small mb-1">
            <i className="fas fa-map-marker-alt me-2"></i>
            {locationDisplay}
          </p>
          <p className="lead mb-3">
            <i className="fas fa-user-circle me-2"></i>
            Trip Owner: <b>{td.ownerName}</b>
          </p>

          {/* --- Action Buttons --- */}
          <div className="d-flex justify-content-center flex-wrap">
            {/* Request to Join (If not owner, not accepted buddy, and no pending request) */}
            {!isOwner && !isBuddy && !hasPendingRequest && (
                <button className="btn btn-primary btn-lg me-3 mb-2" onClick={onRequest}>
                    <i className="fas fa-plus-circle me-2"></i> Request to Join
                </button>
            )}
            {/* Leave Stop (If accepted buddy) */}
            {!isOwner && isBuddy && (
                <button className="btn btn-outline-danger btn-lg me-3 mb-2" onClick={onLeave}>
                    <i className="fas fa-door-open me-2"></i> Leave Stop
                </button>
            )}
            {/* Request Pending Status */}
            {!isOwner && hasPendingRequest && (
                <span className="badge bg-warning text-dark p-3 me-3 mb-2 d-flex align-items-center">
                    <i className="fas fa-hourglass-half me-2"></i> Request Pending Owner Review
                </span>
            )}
            {/* Chat Link (If conversation exists) */}
            {td.groupConversationId && (
                <a href={`/chat/${td.groupConversationId}`} className="btn btn-success btn-lg mb-2">
                    <i className="fas fa-comments me-2"></i> Chat
                </a>
            )}
          </div>
        </div>

        {/* Message Alert */}
        {message && (
          <div className="alert alert-info alert-dismissible fade show" role="alert">
            {message}
            <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        )}

        {/* --- EXISTING MAIN CONTENT: Destination Info and Buddies --- */}
        <div className="row mb-5 mt-4">
          
          {/* Destination Details (Primary Focus) - 7/12 width */}
          <div className="col-lg-7">
            <div className="card shadow-sm mb-4">
              <div className="card-header bg-light text-dark">
                <i className="fas fa-calendar-check me-2"></i> <b>Destination Stop Details</b>
              </div>
              <div className="card-body">
                <div className="row mb-3">
                    <div className="col-md-6">
                        <p className="mb-0">
                            <b className="text-secondary">Start Date:</b> {td.destinationStartDate}
                        </p>
                    </div>
                    <div className="col-md-6">
                        <p className="mb-0">
                            <b className="text-secondary">End Date:</b> {td.destinationEndDate}
                        </p>
                    </div>
                </div>
                
                <h6 className="mt-3 text-primary">Description:</h6>
                <p className="card-text">{td.destinationDescription || <em>No specific description for this stop.</em>}</p>

                {td.longitude && td.latitude && (
                  <p className="card-text text-muted mt-3 small">
                    <i className="fas fa-globe me-2"></i>
                    <b>Coordinates:</b> {td.latitude}, {td.longitude}
                  </p>
                )}
              </div>
            </div>
          </div>

          {/* Buddies and Requests (Side Column) - 5/12 width */}
          <div className="col-lg-5">
            
            {/* Accepted Buddies */}
            <div className="card shadow-sm mb-4">
              <div className="card-header bg-success text-white">
                <i className="fas fa-user-friends me-2"></i> <b>Accepted Buddies</b> ({totalAcceptedPeople} {td.maxBuddies ? `/ ${td.maxBuddies}` : ''})
              </div>
              <ul className="list-group list-group-flush">
                {td.acceptedBuddies?.length ? td.acceptedBuddies.map((b) => (
                  <li key={b.buddyId} className="list-group-item d-flex justify-content-between align-items-center">
                    <div>
                      <b>{b.buddyName}</b>
                      {b.personCount > 1 && <span className="badge bg-primary ms-2">{b.personCount} people</span>}
                      {b.buddyNote && <small className="text-muted d-block"><em>Note: "{b.buddyNote}"</em></small>}
                    </div>
                    {/* Owner removal button */}
                    {isOwner && String(b.buddyUserId) !== currentUserId && (
                      <button className="btn btn-sm btn-outline-danger" onClick={() => onRemoveBuddy(b.buddyId)} title="Remove Buddy">
                        <i className="fas fa-user-times"></i>
                      </button>
                    )}
                  </li>
                )) : (
                  <li className="list-group-item text-muted">No accepted buddies yet.</li>
                )}
              </ul>
            </div>

            {/* Pending Requests (Owner View) */}
            {isOwner && (
              <div className="card shadow-sm">
                <div className="card-header bg-warning text-dark">
                  <i className="fas fa-bell me-2"></i> <b>Pending Requests</b> ({td.pendingRequests.length})
                </div>
                <ul className="list-group list-group-flush">
                  {td.pendingRequests?.length ? td.pendingRequests.map((r) => (
                    <li key={r.buddyId} className="list-group-item d-flex justify-content-between align-items-center">
                      <div>
                        <b>{r.requesterName}</b>
                        {r.personCount > 1 && <span className="badge bg-secondary ms-2">{r.personCount} people</span>}
                        {r.buddyNote && <small className="text-muted d-block"><em>Note: "{r.buddyNote}"</em></small>}
                      </div>
                      <div>
                        <button
                          className="btn btn-sm btn-success me-2"
                          onClick={() => onHandleRequest(r.buddyId, 'accept')}
                          title="Accept Request"
                        >
                          <i className="fas fa-check"></i>
                        </button>
                        <button
                          className="btn btn-sm btn-danger"
                          onClick={() => onHandleRequest(r.buddyId, 'reject')}
                          title="Reject Request"
                        >
                          <i className="fas fa-times"></i>
                        </button>
                      </div>
                    </li>
                    )) : (
                      <li className="list-group-item text-muted">No pending requests.</li>
                    )}
                </ul>
              </div>
            )}
          </div>
        </div>
        
        {/* --- NEW: OVERALL TRIP OVERVIEW SECTION (at the bottom) --- */}
        <hr className="my-5" />
        <h2 className="mb-4 text-secondary text-center">
            <i className="fas fa-route me-2"></i> Full Trip Itinerary: {tripOverview?.tripName || 'Loading...'}
        </h2>

        {tripOverview ? (
            <DestinationStopOverview 
                tripOverview={tripOverview} 
                currentDestinationId={td.tripDestinationId} 
            />
        ) : (
             <div className="alert alert-info text-center">Loading trip itinerary context...</div>
        )}
      </div>
    </div>
  );
};

export default TripDetailsPage;
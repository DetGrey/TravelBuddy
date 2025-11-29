import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import NavBar from '../components/NavBar';
import tripsService from '../services/tripsService';
import { AuthContext } from '../context/AuthContext';
import DestinationStopOverview from '../components/TripDestinationOverview'; // Assumed component for itinerary
import BuddyActionModal from '../components/BuddyActionModal';
import BuddyList from '../components/BuddyList';
import PendingRequestList from '../components/PendingRequestList';
import JoinRequestModal from '../components/JoinRequestModal';

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

  // -------------------------------------------------------------
  // --- STATE FOR LEAVE CONFIRMATION MODAL ---
  // -------------------------------------------------------------
  const [showBuddyActionModal, setShowBuddyActionModal] = useState(false);
  const [modalMode, setModalMode] = useState("leave"); // "leave" or "remove"
  const [targetBuddy, setTargetBuddy] = useState(null); // buddy object if removing
  const [isProcessing, setIsProcessing] = useState(false);
  const [departureReason, setDepartureReason] = useState('');
  // -------------------------------------------------------------

  // -------------------------------------------------------------
  // --- STATE FOR JOIN REQUEST MODAL (NEW) ---
  // -------------------------------------------------------------
  const [showJoinModal, setShowJoinModal] = useState(false);
  const [joinPersonCount, setJoinPersonCount] = useState(1);
  const [joinNote, setJoinNote] = useState('');
  const [isSendingRequest, setIsSendingRequest] = useState(false);
  const [buddiesLeft, setBuddiesLeft] = useState(0);

  // -------------------------------------------------------------

  // --- Action Handlers (Defined here for simplicity, typically in a Hook/Service) ---
  
  const fetchData = async () => {
    try {
      if (!user?.id) throw new Error('Missing user id — login first');
      
      // 1. Fetch the main, detailed stop info (Action-focused data)
      const destinationData = await tripsService.getTripDestinationInfo(user.id, tripDestinationId);
      setTripDestination(destinationData);
      setMessage(null);
      // Calculate buddies left for join requests
      const acceptedPeople = Array.isArray(destinationData.acceptedBuddies)
        ? destinationData.acceptedBuddies.reduce((sum, buddy) => sum + (buddy.personCount || 1), 0)
        : 0;
      setBuddiesLeft(destinationData.maxBuddies - acceptedPeople);

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

  const onRequest = () => {
    setShowJoinModal(true);
    setJoinPersonCount(1); // Reset to default
    setJoinNote(''); // Reset note
  };

  // Handle Join Request Confirmation (new service call logic)
  const handleJoinConfirm = async () => {
    if (!user?.id) {
      setMessage('Missing user id — please log in.');
      setShowJoinModal(false);
      return;
    }

    if (joinPersonCount < 1 || joinPersonCount > buddiesLeft) {
        setMessage(`Person count must be between 1 and ${buddiesLeft}.`);
        return;
    }

    try {
      setIsSendingRequest(true);
      // Call the updated tripsService.requestJoin with all necessary DTO fields
      await tripsService.requestJoin(user.id, tripDestinationId, joinPersonCount, joinNote); 
      setMessage('Join request sent! Awaiting owner approval.');
      setShowJoinModal(false);
      await fetchData(); 
    } catch (e) {
      setMessage(e?.response?.data?.message || e.message || 'Failed to send join request.');
    } finally {
      setIsSendingRequest(false);
    }
  };

  // 1. Function to open the modal
  const onLeave = () => {
    setModalMode("leave");
    setTargetBuddy(null);
    setDepartureReason("");
    setShowBuddyActionModal(true);
  };

  const onRemoveBuddy = (buddy) => {
    setModalMode("remove");
    setTargetBuddy(buddy);
    setDepartureReason("");
    setShowBuddyActionModal(true);
  };

  // 2. Function to handle the actual service call after confirmation
  const handleLeaveConfirm = async () => {
    if (!user?.id) {
      setMessage("Missing user id — please log in.");
      setShowBuddyActionModal(false);
      return;
    }

    try {
      setIsProcessing(true);

      // For both leave and remove, call the same endpoint
      await tripsService.leaveTrip(
        modalMode === "leave" ? user.id : targetBuddy.buddyUserId,
        tripDestinationId,
        reason
      );

      setMessage(
        modalMode === "leave"
          ? "You have successfully left this trip destination."
          : `You have removed ${targetBuddy.buddyName}.`
      );

      setShowBuddyActionModal(false);
      setDepartureReason("");
      await fetchData();
    } catch (e) {
      setMessage(
        e?.response?.data?.message ||
          e.message ||
          "Failed to update buddy status."
      );
    } finally {
      setIsProcessing(false);
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

  // Check if capacity is reached
  const isCapacityReached = td.maxBuddies > 0 && totalAcceptedPeople >= td.maxBuddies;

  // NEW: Check if the user is authorized to see the chat button
  const isChatMember = isOwner || isBuddy;


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
                // Conditional Rendering based on capacity
                isCapacityReached ? (
                    <span className="badge bg-danger p-3 me-3 mb-2 d-flex align-items-center">
                        <i className="fas fa-users-slash me-2"></i> Capacity Reached
                    </span>
                ) : (
                    <button className="btn btn-primary btn-lg me-3 mb-2" onClick={onRequest}>
                        <i className="fas fa-plus-circle me-2"></i> Request to Join
                    </button>
                )
            )}

            {/* Leave Stop (If accepted buddy) */}
            {!isOwner && isBuddy && (
                // UPDATED: Click opens the modal instead of calling the service directly
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
            {/* Chat Link (If conversation exists AND user is an owner or accepted buddy) */}
            {td.groupConversationId && isChatMember && (
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
            <BuddyList 
                acceptedBuddies={td.acceptedBuddies}
                isOwner={isOwner}
                currentUserId={currentUserId}
                onRemoveBuddy={onRemoveBuddy}
                maxBuddies={td.maxBuddies}
                totalAcceptedPeople={totalAcceptedPeople}
            />

            {/* Pending Requests (Owner View) */}
            <PendingRequestList 
                pendingRequests={td.pendingRequests}
                onHandleRequest={onHandleRequest}
                isOwner={isOwner}
            />
          </div>
        </div>
        
        {/* --- NEW: OVERALL TRIP OVERVIEW SECTION (at the bottom) --- */}
        <hr className="my-5" />
        <h2 className="mb-4 text-secondary text-center">
            <i className="fas fa-route me-2"></i> Full Trip Itinerary: {tripOverview?.tripName || 'Loading...'}
        </h2>
        <p className="card-text">{tripOverview?.tripDescription}</p>


        {tripOverview ? (
            <DestinationStopOverview 
                tripOverview={tripOverview} 
                currentDestinationId={td.tripDestinationId} 
            />
        ) : (
             <div className="alert alert-info text-center">Loading trip itinerary context...</div>
        )}
      </div>


      {/* -------------------------------------------------------------------------------------- */}
      {/* BuddyActionModal */}
      {/* -------------------------------------------------------------------------------------- */}
      <BuddyActionModal
          show={showBuddyActionModal}
          onClose={() => setShowBuddyActionModal(false)}
          onConfirm={handleLeaveConfirm}
          reason={departureReason}
          setReason={setDepartureReason}
          isProcessing={isProcessing}
          mode={modalMode}
          buddyName={targetBuddy?.buddyName}
        />

      {/* -------------------------------------------------------------------------------------- */}
      {/* NEW JOIN REQUEST MODAL */}
      {/* -------------------------------------------------------------------------------------- */}
      <JoinRequestModal
          show={showJoinModal}
          onClose={() => setShowJoinModal(false)}
          onConfirm={handleJoinConfirm}
          joinPersonCount={joinPersonCount}
          setJoinPersonCount={setJoinPersonCount}
          joinNote={joinNote}
          setJoinNote={setJoinNote}
          isSendingRequest={isSendingRequest}
          maxAvailable={buddiesLeft}
      />

    </div>
  );
};

export default TripDetailsPage;
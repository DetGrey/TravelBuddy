import React, { useEffect, useState, useContext } from 'react';
import NavBar from '../components/NavBar';
import { Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import tripsService from '../services/tripsService';

const categorizeAndSortTrips = (allTrips) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Normalize time to midnight

    const futureTrips = [];
    const archivedTrips = [];
    
    (allTrips || []).forEach(trip => {
        const tripEndDate = new Date(trip.endDate + 'T00:00:00');

        // Check for two conditions for archiving: 
        // 1. Explicitly marked as archived (isArchived property)
        // 2. The trip end date has passed (tripEndDate < today)
        if (trip.isArchived || tripEndDate < today) {
            archivedTrips.push(trip);
        } else {
            futureTrips.push(trip);
        }
    });

    // Sort future trips by start date (soonest first/ascending)
    futureTrips.sort((a, b) => 
        new Date(a.startDate + 'T00:00:00') - new Date(b.startDate + 'T00:00:00')
    );

    // Sort archived trips by end date (most recent archived first/descending)
    archivedTrips.sort((a, b) => 
        new Date(b.endDate + 'T00:00:00') - new Date(a.endDate + 'T00:00:00')
    );

    return { futureTrips, archivedTrips };
};

// --- Date Formatting Helper (Re-used for consistency) ---
const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    try {
        return new Date(dateString + 'T00:00:00').toLocaleDateString('en-US', {
            month: 'short', 
            day: 'numeric', 
            year: 'numeric'
        });
    } catch {
        return dateString;
    }
};

const TripCard = ({ t }) => (
    <div key={t.tripDestinationId} className="col-sm-12 col-md-6 col-lg-4">
        <div className="card shadow-sm h-100">
            <div className="card-body d-flex flex-column">
                
                <h5 className="card-title mb-1">
                    <Link to={`/trip-destinations/${t.tripDestinationId}`} className="text-decoration-none text-primary">
                        {t.destinationName || t.tripId}
                    </Link>
                </h5>
                <h6 className="card-subtitle text-muted mb-3">
                    {t.tripDescription && <small>{t.tripDescription.substring(0, 50)}...</small>}
                </h6>
                
                <div className="mb-3 p-2 bg-light rounded small flex-grow-1">
                    <div className="d-flex justify-content-between">
                        <span className="text-secondary"><strong>Start:</strong></span>
                        <span className="fw-bold">{formatDate(t.startDate)}</span>
                    </div>
                    <div className="d-flex justify-content-between mt-1">
                        <span className="text-secondary"><strong>End:</strong></span>
                        <span className="fw-bold">{formatDate(t.endDate)}</span>
                    </div>
                    {/* Assuming capacity data is still available on the trip object */}
                    {t.maxBuddies !== undefined && (
                        <div className="d-flex justify-content-between mt-2 pt-2 border-top">
                            <span className="text-secondary"><strong>Capacity:</strong></span>
                            <span>{t.acceptedPersons} accepted of <strong>{t.maxBuddies}</strong> total</span>
                        </div>
                    )}
                </div>
                
                <div className="mt-auto pt-2">
                    <Link to={`/trip-destinations/${t.tripDestinationId}`} className="btn btn-sm btn-primary w-100">
                        View / Manage Trip
                    </Link>
                </div>
            </div>
        </div>
    </div>
);


const MyTripsPage = () => {
    const [trips, setTrips] = useState([]);
    const [pendingRequests, setPendingRequests] = useState([]);

    const { user } = useContext(AuthContext);
    
    // --------------------------------------------------------

    const refreshPendingRequests = async () => {
        if (!user?.id) return;
        try {
            const pending = await tripsService.getRequests(user.id);
            setPendingRequests(pending || []);
        } catch (err) {
            console.error("Error refreshing pending requests:", err);
        }
    };

    useEffect(() => {
        let mounted = true;
        (async () => {
            try {
                if (!user?.id) return;
                // NOTE: Changed to getMyTrips endpoint (assuming it exists based on service structure)
                const data = await tripsService.getMyTrips(user.id); 
                if (mounted) setTrips(data || []);
                
                const pending = await tripsService.getRequests(user.id);
                if (mounted) setPendingRequests(pending || []);
            } catch (err) {
                console.error(err);
            }
        })();
        return () => (mounted = false);
    }, [user]);

    const onAccept = async (tripId, buddyId) => {
        if (!user?.id) return;
        await tripsService.acceptRequest(buddyId, tripId); 
        await refreshPendingRequests();
    };

    const onReject = async (tripId, buddyId) => {
        if (!user?.id) return;
        await tripsService.rejectRequest(buddyId, tripId); 
        await refreshPendingRequests();
    };
    
    // Create a lookup for trip details (maxBuddies, acceptedPersons) by tripId
    const tripDetailsLookup = trips.reduce((acc, trip) => {
        acc[trip.tripDestinationId] = { maxBuddies: trip.maxBuddies, acceptedPersons: trip.acceptedPersons };
        return acc;
    }, {});

    const groupedRequests = pendingRequests.reduce((acc, p) => {
        (acc[p.tripId] = acc[p.tripId] || { tripId: p.tripId, destinationName: p.destinationName, requests: [] }).requests.push(p);
        return acc;
    }, {});

    // --- NEW: Categorize and sort trips for rendering ---
    const { futureTrips, archivedTrips } = categorizeAndSortTrips(trips);

    return (
        <div>
            <NavBar />
            <div className="container py-4">
                
                {/* PENDING REQUESTS SECTION */}
                <div className="card mb-4 p-3 mx-auto" style={{ maxWidth: 900 }}>
                    <h5>Pending requests</h5>
                    
                    {pendingRequests.length === 0 ? (
                        <div className="text-muted">No pending requests.</div>
                    ) : (
                        Object.entries(groupedRequests).map(([tripId, group]) => {
                            const details = tripDetailsLookup[tripId];
                            // Determine if capacity is reached
                            const isCapacityReached = details && details.acceptedPersons >= details.maxBuddies;
                            
                            return (
                                <div key={tripId} className="mb-2">
                                    <div className="fw-bold">{group.destinationName} ({group.tripId})</div>
                                    <div>
                                        {(group.requests || []).map((r) => (
                                            <div key={r.buddyId || r.BuddyId} className="d-flex justify-content-between align-items-center border rounded p-2 mb-2">
                                                <div>{r.buddyName || r.BuddyName} <small className="text-muted">({r.buddyNote || r.BuddyNote})</small></div>
                                                <div>
                                                    {/* LOGIC CHANGE: Check if capacity is reached */}
                                                    {isCapacityReached ? (
                                                        <span className="text-danger small me-2">Capacity Reached</span>
                                                    ) : (
                                                        <button 
                                                            className="btn btn-sm btn-success me-2" 
                                                            onClick={() => onAccept(group.tripId, r.buddyId || r.BuddyId)}
                                                        >
                                                            Accept
                                                        </button>
                                                    )}
                                                    
                                                    <button 
                                                        className="btn btn-sm btn-danger" 
                                                        onClick={() => onReject(group.tripId, r.buddyId || r.BuddyId)}
                                                    >
                                                        Reject
                                                    </button>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            );
                        })
                    )}
                </div>

                {/* --- 1. FUTURE TRIPS SECTION (Sorted soonest first) --- */}
                <h3 className="text-center mt-5">Upcoming Trips</h3>
                <div className="row g-4 justify-content-center">
                    {futureTrips.length === 0 && <div className="text-muted text-center py-3">No upcoming trips planned.</div>}
                    
                    {futureTrips.map(t => <TripCard key={t.tripDestinationId} t={t} />)}
                </div>
                
                <hr className="my-5" />
                
                {/* --- 2. ARCHIVED TRIPS SECTION --- */}
                <h3 className="text-center mb-4">Archived Trips</h3>
                <div className="row g-4 justify-content-center">
                    {archivedTrips.length === 0 && <div className="text-muted text-center py-3">No past or archived trips found.</div>}
                    
                    {archivedTrips.map(t => <TripCard key={t.tripDestinationId} t={t} />)}
                </div>

            </div>
        </div>
    );
};

export default MyTripsPage;
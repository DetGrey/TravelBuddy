import React, { useEffect, useState, useContext } from 'react';
import NavBar from '../components/NavBar';
import { AuthContext } from '../context/AuthContext';
import tripsService from '../services/tripsService';
import categorizeAndSortTrips from '../utils/categorizeAndSortTrips';
import TripCard from '../components/TripCard';
import PendingRequestsCard from '../components/PendingRequestCard';


const MyTripsPage = () => {
    const [buddyTrips, setBuddyTrips] = useState([]);
    const [ownedTrips, setOwnedTrips] = useState([]);
    const [pendingRequests, setPendingRequests] = useState([]);

    const { user } = useContext(AuthContext);
    
    // --------------------------------------------------------
    const refreshTripsAndRequests = async () => {
        if (!user?.id) return;
        try {
            // 1. Refresh Trips to get the latest acceptedPersons count
            const buddyData = await tripsService.getBuddyTrips(user.id); 
            setBuddyTrips(buddyData || []);
            const ownedData = await tripsService.getOwnedTrips(user.id);
            setOwnedTrips(ownedData || []);

            // 2. Refresh Pending Requests
            const pending = await tripsService.getRequests(user.id);
            setPendingRequests(pending || []);
        } catch (err) {
            console.error("Error refreshing data:", err);
        }
    };

    useEffect(() => {
        let mounted = true;
        (async () => {
            refreshTripsAndRequests();
        })();
        return () => (mounted = false);
    }, [user]);

    const onAccept = async (buddyId) => {
        if (!user?.id) return;
        try {
            await tripsService.acceptRequest(user.id, buddyId); 
            await refreshTripsAndRequests();
        } catch (error) {
            console.error("Error accepting request:", error);
        }
    };

    const onReject = async (buddyId) => {
        if (!user?.id) return;
        try {
            await tripsService.rejectRequest(user.id, buddyId); 
            await refreshTripsAndRequests();
        } catch (error) {
            console.error("Error rejecting request:", error);
        }
    };

    // Create a robust lookup for trip details (maxBuddies, acceptedPersons) by tripDestinationId
    const tripDetailsLookup = buddyTrips.reduce((acc, trip) => {
        // Ensure that acceptedPersons and maxBuddies are indeed numbers for comparison
        acc[trip.tripDestinationId] = { 
            maxBuddies: Number(trip.maxBuddies), 
            acceptedPersons: Number(trip.acceptedPersons) 
        };
        return acc;
    }, {});

    const groupedRequests = pendingRequests.reduce((acc, p) => {
        (acc[p.tripId] = acc[p.tripId] || { tripId: p.tripId, destinationName: p.destinationName, requests: [] }).requests.push(p);
        return acc;
    }, {});

    // --- NEW: Categorize and sort trips for rendering ---
    // Add a type flag so you can still distinguish later in TripCard
    const allTrips = [
    ...buddyTrips.map(t => ({ ...t, type: 'buddy' })),
    ...ownedTrips.map(t => ({ ...t, type: 'owned' }))
    ];

    // Now categorize once
    const { futureTrips, archivedTrips } = categorizeAndSortTrips(allTrips);
    return (    
        <div>
            <NavBar />
            <div className="container py-4">
                
                {/* PENDING REQUESTS SECTION */}
                <PendingRequestsCard
                pendingRequests={pendingRequests}
                groupedRequests={groupedRequests}
                tripDetailsLookup={tripDetailsLookup}
                onAccept={onAccept}
                onReject={onReject}
                />
                <hr className="my-5" />

                {/* --- FUTURE TRIPS SECTION (Sorted soonest first) --- */}
                <h3 className="text-center mt-5">Upcoming Trips</h3>
                <div className="row g-4 justify-content-center">
                    {futureTrips.length === 0 && <div className="text-muted text-center py-3">No upcoming trips planned.</div>}
                    
                    {futureTrips.map(t => (
                    <TripCard
                        key={`${t.tripId}-${t.tripDestinationId ?? 'owned'}`}
                        t={t}
                    />
                    ))}
                </div>
                
                <hr className="my-5" />
                
                {/* --- ARCHIVED TRIPS SECTION --- */}
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
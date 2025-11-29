import React from 'react';
import { Link } from 'react-router-dom';

/**
 * Renders the full list of destinations (the itinerary) for the trip overview section.
 * @param {object} tripOverview - The TripAggregateModel/TripInfoDTO result.
 * @param {number} currentDestinationId - The ID of the destination stop currently being viewed.
 */
const TripDestinationOverview = ({ tripOverview, currentDestinationId }) => {
    if (!tripOverview?.destinations?.length) {
        return <p className="text-center text-muted">This trip has no destinations set up yet.</p>;
    }

    return (
        <div className="list-group">
            {tripOverview.destinations.map((dest, index) => {
                const isActive = dest.tripDestinationId === currentDestinationId;
                // Optional: Calculate capacity ratio for visual feedback
                const capacityRatio = dest.acceptedBuddiesCount / dest.maxBuddies;
                const capacityClass = capacityRatio >= 1 ? 'bg-danger' : capacityRatio > 0.75 ? 'bg-warning text-dark' : 'bg-success';
                
                return (
                    <div 
                        key={dest.tripDestinationId} 
                        className={`list-group-item list-group-item-action ${isActive ? 'list-group-item-primary border border-2 border-primary' : ''}`}
                        aria-current={isActive ? 'true' : 'false'}
                    >
                        <div className="d-flex w-100 justify-content-between align-items-center">
                            <h5 className="mb-1">
                                <span className="badge bg-secondary me-2">{index + 1}</span> 
                                {dest.destinationName} 
                                {isActive && <span className="badge bg-primary ms-2">You Are Here</span>}
                            </h5>
                            <small className={`badge ${capacityClass} p-2`}>
                                {dest.acceptedBuddiesCount} / {dest.maxBuddies} Buddies
                            </small>
                        </div>
                        <p className="mb-1 small text-muted">
                            <i className="fas fa-map-marker-alt me-1"></i> {dest.destinationCountry}
                            <span className="ms-3"><i className="fas fa-calendar-alt me-1"></i> {new Date(dest.destinationStartDate).toLocaleDateString()} — {new Date(dest.destinationEndDate).toLocaleDateString()}</span>
                        </p>
                        {/* Link to the detailed page for this stop */}
                        {!isActive && (
                            <Link to={`/trip-destinations/${dest.tripDestinationId}`} className="btn btn-sm btn-outline-secondary mt-2">
                                View Details <i className="fas fa-angle-right ms-1"></i>
                            </Link>
                        )}
                    </div>
                );
            })}
        </div>
    );
};

export default TripDestinationOverview;
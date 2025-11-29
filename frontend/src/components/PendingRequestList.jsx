import React from 'react';

/**
 * Component to display the list of pending requests, visible only to the trip owner.
 * * @param {object} props
 * @param {Array<object>} props.pendingRequests - List of pending join requests.
 * @param {function} props.onHandleRequest - Handler function to accept or reject a request.
 * @param {boolean} props.isOwner - True if the current user is the trip owner.
 */
const PendingRequestList = ({ pendingRequests, onHandleRequest, isOwner }) => {
    if (!isOwner) return null;

    return (
        <div className="card shadow-sm">
            <div className="card-header bg-warning text-dark">
                <i className="fas fa-bell me-2"></i> <b>Pending Requests</b> ({pendingRequests.length})
            </div>
            <ul className="list-group list-group-flush">
                {pendingRequests?.length ? pendingRequests.map((r) => (
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
    );
};

export default PendingRequestList;
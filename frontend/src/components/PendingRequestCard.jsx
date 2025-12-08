// PendingRequestsCard.jsx
import React from "react";
import { Link } from "react-router-dom";
import formatDate from '../utils/FormatDate';

const PendingRequestsCard = ({
  pendingRequests,
  groupedRequests,
  onAccept,
  onReject,
}) => {
  return (
    <div className="card mb-4 p-3 mx-auto shadow-sm" style={{ maxWidth: 900 }}>
      <h5 className="mb-3 fw-bold">Pending Requests</h5>

      {pendingRequests.length === 0 ? (
        <div className="text-muted fst-italic">No pending requests.</div>
      ) : (
        Object.entries(groupedRequests).map(([tripId, group]) => (
          <div key={tripId} className="mb-4">
            {/* Destination header with link */}
            <div className="fw-bold mb-2">
              <Link
                to={`/trip-destinations/${group.tripDestinationId}`}
                className="text-decoration-none fw-bold text-primary"
              >
                {group.destinationName}
              </Link>{" "}
            </div>

            {/* Requests list */}
            <div>
              {(group.requests || []).map((r) => (
                <div
                  key={r.buddyId || r.BuddyId}
                  className="border rounded p-3 mb-2 bg-light d-flex justify-content-between align-items-center"
                >
                  {/* Requester info */}
                  <div>
                    <div className="fw-semibold">
                      {r.requesterName}{" "}
                    </div>
                    <div className="small text-muted">
                      {r.personCount} person(s) • {formatDate(r.destinationStartDate)} → {formatDate(r.destinationEndDate)}
                    </div>
                    <div className="fst-italic">{r.buddyNote}</div>
                  </div>

                  {/* Action buttons */}
                  <div>
                    <button
                      className="btn btn-sm btn-success me-2"
                      onClick={() => onAccept(r.buddyId)}
                    >
                      Accept
                    </button>
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => onReject(r.buddyId)}
                    >
                      Reject
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default PendingRequestsCard;
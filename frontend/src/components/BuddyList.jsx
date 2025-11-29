// BuddyList.jsx
import React from "react";

/**
 * Component to display the list of accepted buddies for a trip destination.
 */
const BuddyList = ({
  acceptedBuddies,
  isOwner,
  currentUserId,
  onRemoveBuddy,
  maxBuddies,
  totalAcceptedPeople,
}) => {
  return (
    <div className="card shadow-sm mb-4">
      <div className="card-header bg-success text-white d-flex justify-content-between align-items-center">
        <div>
          <i className="fas fa-user-friends me-2"></i>
          <b>Accepted Buddies</b>
        </div>
        <small className="fw-light">
          {totalAcceptedPeople} {maxBuddies ? `/ ${maxBuddies}` : ""}
        </small>
      </div>

      <ul className="list-group list-group-flush">
        {acceptedBuddies?.length ? (
          acceptedBuddies.map((b) => (
            <li
              key={b.buddyId}
              className="list-group-item d-flex justify-content-between align-items-start"
            >
              {/* Buddy info */}
              <div>
                <div className="fw-semibold">
                  {b.buddyName}
                  {b.personCount > 1 && (
                    <span className="badge bg-primary ms-2">
                      {b.personCount} people
                    </span>
                  )}
                </div>
                {b.buddyNote && (
                  <small className="text-muted d-block fst-italic">
                    “{b.buddyNote}”
                  </small>
                )}
              </div>

              {/* Owner actions */}
              {isOwner && String(b.buddyUserId) !== currentUserId && (
                <div className="text-end">
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => onRemoveBuddy(b)}
                  >
                    <i className="fas fa-user-times me-1"></i> Remove
                  </button>
                </div>
              )}
            </li>
          ))
        ) : (
          <li className="list-group-item text-muted">
            No accepted buddies yet.
          </li>
        )}
      </ul>
    </div>
  );
};

export default BuddyList;
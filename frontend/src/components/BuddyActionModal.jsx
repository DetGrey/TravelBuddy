import React from "react";

/**
 * A modal component for confirming either:
 * - a buddy leaving a trip destination
 * - an owner removing a buddy
 *
 * @param {object} props
 * @param {boolean} props.show - Controls the visibility of the modal.
 * @param {function} props.onClose - Function to call when the modal is closed/canceled.
 * @param {function} props.onConfirm - Function to call when the action is confirmed.
 * @param {string} props.reason - The current value of the reason input.
 * @param {function} props.setReason - Setter function for the reason state.
 * @param {boolean} props.isProcessing - Loading state to disable buttons during the API call.
 * @param {"leave"|"remove"} props.mode - Determines whether this is a leave or remove action.
 * @param {string} [props.buddyName] - Name of the buddy being removed (only relevant in remove mode).
 */
const BuddyActionModal = ({
  show,
  onClose,
  onConfirm,
  reason,
  setReason,
  isProcessing,
  mode,
  buddyName,
}) => {
  if (!show) return null;

  const isLeave = mode === "leave";

  return (
    <div
      className="modal fade show d-block"
      tabIndex="-1"
      role="dialog"
      style={{ backgroundColor: "rgba(0,0,0,0.5)", zIndex: 1050 }}
    >
      <div className="modal-dialog modal-dialog-centered" role="document">
        <div className="modal-content shadow-lg rounded-xl">
          <div className="modal-header bg-danger text-white rounded-t-xl">
            <h5 className="modal-title">
              {isLeave ? "Confirm Departure" : "Remove Buddy"}
            </h5>
            <button
              type="button"
              className="btn-close btn-close-white"
              aria-label="Close"
              onClick={onClose}
              disabled={isProcessing}
            ></button>
          </div>

          <div className="modal-body p-4">
            <p className="mb-4">
              {isLeave
                ? "Are you sure you want to leave this trip destination?"
                : (
                  <>
                    Are you sure you want to remove{" "}
                    <strong>{buddyName}</strong> from this trip destination?
                  </>
                )}
            </p>

            <div className="mb-3">
              <label htmlFor="reason" className="form-label">
                <i className="fas fa-pencil-alt me-1"></i>{" "}
                {isLeave ? "Departure Reason (Optional)" : "Removal Reason (Optional)"}
              </label>
              <textarea
                className="form-control rounded-lg"
                id="reason"
                rows="3"
                value={reason}
                onChange={(e) => setReason(e.target.value)}
                placeholder={
                  isLeave
                    ? "E.g., My plans changed, or I found a different group."
                    : "E.g., Capacity reached, scheduling conflict..."
                }
                disabled={isProcessing}
              ></textarea>
              <small className="form-text text-muted">
                {isLeave
                  ? "This reason will be shared with the trip owner."
                  : "This reason may be shared with the buddy you remove."}
              </small>
            </div>
          </div>

          <div className="modal-footer d-flex justify-content-between">
            <button
              type="button"
              className="btn btn-secondary rounded-lg"
              onClick={onClose}
              disabled={isProcessing}
            >
              Cancel
            </button>
            <button
              type="button"
              className="btn btn-danger rounded-lg"
              onClick={onConfirm}
              disabled={isProcessing}
            >
              {isProcessing ? (
                <>
                  <span
                    className="spinner-border spinner-border-sm me-2"
                    role="status"
                    aria-hidden="true"
                  ></span>
                  {isLeave ? "Leaving..." : "Removing..."}
                </>
              ) : (
                isLeave ? "Confirm Leave" : "Confirm Remove"
              )}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BuddyActionModal;
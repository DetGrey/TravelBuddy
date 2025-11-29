const JoinRequestModal = ({
    show,
    onClose,
    onConfirm,
    joinPersonCount,
    setJoinPersonCount,
    joinNote,
    setJoinNote,
    isSendingRequest,
    maxAvailable
}) => {
    if (!show) return null;

    return (
        <div className="modal fade show d-block" tabIndex="-1" role="dialog"
             style={{ backgroundColor: 'rgba(0,0,0,0.5)', zIndex: 1050 }}>
            <div className="modal-dialog modal-dialog-centered" role="document">
                <div className="modal-content shadow-lg rounded-xl">
                    <div className="modal-header bg-primary text-white rounded-t-xl">
                        <h5 className="modal-title">Request to Join Destination</h5>
                        <button type="button"
                                className="btn-close btn-close-white"
                                aria-label="Close"
                                onClick={onClose}
                                disabled={isSendingRequest}></button>
                    </div>
                    <div className="modal-body p-4">
                        <p className="text-lg mb-4">Specify the details for your join request.</p>

                        {/* Person Count Input */}
                        <div className="mb-3">
                            <label htmlFor="personCount" className="form-label font-semibold text-gray-700">
                                <i className="fas fa-users me-1"></i> Number of People
                            </label>
                            <input
                                type="number"
                                className="form-control rounded-lg"
                                id="personCount"
                                min="1"
                                max={maxAvailable} // enforce max
                                value={joinPersonCount}
                                onChange={(e) => {
                                    const val = parseInt(e.target.value, 10) || 1;
                                    setJoinPersonCount(Math.min(Math.max(1, val), maxAvailable));
                                }}
                                disabled={isSendingRequest}
                                required
                            />
                            <small className="form-text text-muted">
                                Must be between 1 and {maxAvailable}.
                            </small>
                        </div>

                        {/* Note Input */}
                        <div className="mb-3">
                            <label htmlFor="joinNote" className="form-label font-semibold text-gray-700">
                                <i className="fas fa-sticky-note me-1"></i> Note (Optional)
                            </label>
                            <textarea
                                className="form-control rounded-lg"
                                id="joinNote"
                                rows="3"
                                value={joinNote}
                                onChange={(e) => setJoinNote(e.target.value)}
                                placeholder="E.g., Introduce yourself or mention why you want to join."
                                disabled={isSendingRequest}
                            ></textarea>
                            <small className="form-text text-muted">
                                This note will be reviewed by the trip owner.
                            </small>
                        </div>
                    </div>
                    <div className="modal-footer d-flex justify-content-between">
                        <button type="button"
                                className="btn btn-secondary rounded-lg"
                                onClick={onClose}
                                disabled={isSendingRequest}>
                            Cancel
                        </button>
                        <button type="button"
                                className="btn btn-primary rounded-lg"
                                onClick={onConfirm}
                                disabled={isSendingRequest || joinPersonCount < 1}>
                            {isSendingRequest ? (
                                <>
                                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                    Sending...
                                </>
                            ) : (
                                'Send Request'
                            )}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};
export default JoinRequestModal;
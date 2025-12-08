import { Link } from 'react-router-dom';
import formatDate from '../utils/FormatDate';

const TripCard = ({ t }) => {
  const isOwned = t.destinations !== undefined; // owned trips have destinations array

  return (
    <div key={isOwned ? t.tripId : t.tripDestinationId} className="col-sm-12 col-md-6 col-lg-4">
      <div className="card shadow-sm h-100">
        <div className="card-body d-flex flex-column">

          {/* Title */}
          <h5 className="card-title mb-4"> {/* extra spacing */}
            {isOwned ? (
              <span className="text-primary fw-bold">{t.tripName}</span>
            ) : (
              <Link
                to={`/trip-destinations/${t.tripDestinationId}`}
                className="text-decoration-none text-primary fw-bold"
              >
                {t.destinationName}
              </Link>
            )}
          </h5>

          {/* Subtitle */}
          <h6 className="card-subtitle text-muted mb-4"> {/* more spacing */}
            {t.tripDescription && <small>{t.tripDescription}</small>}
          </h6>

          {/* Trip Details */}
          <div className="mb-3 p-2 bg-light rounded small flex-grow-1">
            <div className="d-flex justify-content-between">
              <span className="text-secondary"><strong>Start:</strong></span>
              <span className="fw-bold">
                {formatDate(isOwned ? t.tripStartDate : t.startDate)}
              </span>
            </div>
            <div className="d-flex justify-content-between mt-1">
              <span className="text-secondary"><strong>End:</strong></span>
              <span className="fw-bold">
                {formatDate(isOwned ? t.tripEndDate : t.endDate)}
              </span>
            </div>

            {isOwned && (
              <>
                <div className="d-flex justify-content-between mt-3 pt-2 border-top">
                  <span className="text-secondary"><strong>Owner:</strong></span>
                  <span>{t.ownerName}</span>
                </div>

                {/* Styled destinations list */}
                {t.destinations.length > 0 && (
                  <div className="mt-3 pt-2 border-top">
                    <span className="text-secondary"><strong>Destinations:</strong></span>
                    <div className="mt-3 d-flex flex-column gap-2">
                      {t.destinations.map(d => (
                        <Link
                          key={d.tripDestinationId}
                          to={`/trip-destinations/${d.tripDestinationId}`}
                          className="btn btn-sm btn-primary text-center" // centered text
                        >
                          <div className="fw-bold">{d.destinationName}</div>
                          <div className="small">
                            {formatDate(d.destinationStartDate)} – {formatDate(d.destinationEndDate)}
                          </div>
                        </Link>
                      ))}
                    </div>
                  </div>
                )}
              </>
            )}
          </div>

          {/* Action Button (only for buddy trips) */}
          {!isOwned && (
            <div className="mt-auto pt-2">
              <Link
                to={`/trip-destinations/${t.tripDestinationId}`}
                className="btn btn-sm btn-primary w-100"
              >
                View Destination
              </Link>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default TripCard;
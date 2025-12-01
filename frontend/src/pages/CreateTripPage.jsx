import React, { useState, useEffect, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import tripsService from '../services/tripsService';
import NavBar from '../components/NavBar';

// Helper function to create a new Trip Destination object structure
const createNewTripDestination = (destinationId = '', name = '', state = '', country = '', longitude = '', latitude = '', sequenceNumber = 1) => ({
  // Fields for an existing destination
  destinationId: destinationId,
  // Fields for a new destination
  name: name,
  state: state,
  country: country,
  longitude: longitude,
  latitude: latitude,
  // Fields for the trip destination relationship
  destinationStartDate: '', // Specific start date for this destination
  destinationEndDate: '', // Specific end date for this destination
  sequenceNumber: sequenceNumber,
  description: '' // Specific description for this destination
});

const CreateTripPage = () => {

  const { user } = useContext(AuthContext);

  // --- Trip Fields ---
  const [tripName, setTripName] = useState('');
  const [description, setDescription] = useState(''); // Trip-level description
  const [maxBuddies, setMaxBuddies] = useState(1);
  const [startDate, setStartDate] = useState(''); // Trip-level start date
  const [endDate, setEndDate] = useState(''); // Trip-level end date

  // --- Destination Management State ---
  const [allDestinations, setAllDestinations] = useState([]); // All fetched destinations
  const [searchTerm, setSearchTerm] = useState('');

  // **New State:** Array to hold all the destinations added to this specific trip
  const [tripDestinations, setTripDestinations] = useState([
    createNewTripDestination(null, '', '', '', '', '', 1) // Start with one empty destination
  ]);
  
  // **New State:** Used to temporarily hold the ID of an existing destination selected via search/dropdown
  const [selectedDestinationId, setSelectedDestinationId] = useState(''); 

  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  const filteredDestinations = allDestinations.filter((d) => {
    const term = searchTerm.toLowerCase();
    return (
      d.name.toLowerCase().includes(term) ||
      (d.state && d.state.toLowerCase().includes(term)) ||
      d.country.toLowerCase().includes(term) ||
      String(d.latitude).includes(term) ||
      String(d.longitude).includes(term)
    );
  });

  // Fetch destinations from API
  useEffect(() => {
    const fetchDestinations = async () => {
      try {
        const data = await tripsService.getDestinations();
        setAllDestinations(data || []);
      } catch (err) {
        console.error('Failed to fetch destinations', err);
      }
    };
    fetchDestinations();
  }, []);

  // --- Handlers for Trip Destinations ---

  // Handler to update fields for a specific trip destination (e.g., dates, description)
  const handleTripDestinationChange = (index, field, value) => {
    const updatedDestinations = tripDestinations.map((dest, i) => {
      if (i === index) {
        return { ...dest, [field]: value };
      }
      return dest;
    });
    setTripDestinations(updatedDestinations);
  };

  // Handler to add a new destination slot
  const addTripDestination = () => {
    setTripDestinations([
      ...tripDestinations,
      createNewTripDestination(null, '', '', '', '', '', tripDestinations.length + 1)
    ]);
    // Reset temporary selection states after adding a new slot
    setSelectedDestinationId('');
    setSearchTerm('');
  };

  // Handler to remove a destination slot
  const removeTripDestination = (index) => {
    if (tripDestinations.length > 1) {
      const updatedDestinations = tripDestinations
        .filter((_, i) => i !== index)
        .map((dest, i) => ({ ...dest, sequenceNumber: i + 1 })); // Recalculate sequence numbers
      setTripDestinations(updatedDestinations);
    }
  };
  
  // Handler to select an existing destination, populating the first empty slot or adding a new one
  const handleSelectExistingDestination = (e, index) => {
    const selectedId = e.target.value;
    const selected = allDestinations.find(d => String(d.destinationId) === selectedId);

    const updatedDestinations = tripDestinations.map((dest, i) => {
      if (i === index) {
        if (selected) {
          // Selecting an existing destination
          return {
            ...dest,
            destinationId: parseInt(selectedId, 10),
            name: selected.name,
            state: selected.state,
            country: selected.country,
            longitude: selected.longitude,
            latitude: selected.latitude,
            // Keep existing trip-specific fields (dates, description)
          };
        } else {
          // Choosing "-- Create New Destination --" (value is empty string)
          // Clear destination fields, but keep trip-specific fields
          return {
            ...dest,
            destinationId: null, // Ensure ID is null for new destination
            name: '',
            state: '',
            country: '',
            longitude: '',
            latitude: '',
          };
        }
      }
      return dest;
    });
    setTripDestinations(updatedDestinations);
  };

  // Handler to update the fields for a *new* destination within a slot
  const handleNewDestinationFieldChange = (index, field, value) => {
    // Only update if no existing destination is selected for this slot
    if (!tripDestinations[index]?.destinationId) {
      handleTripDestinationChange(index, field, value);
    }
  };

  const onSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);
    if (!user?.id) {
      setError('User not logged in.');
      return;
    }
    // Filter out any completely empty destination slots if the user didn't fill them out
    const finalTripDestinations = tripDestinations.filter(d => 
        (d.destinationId || d.name) // Require either an existing ID or a name for a new one
    ).map((d, index) => {
      // Ensure all fields are in the correct format for the API payload
      const baseDestination = {
        destinationStartDate: d.destinationStartDate,
        destinationEndDate: d.destinationEndDate,
        sequenceNumber: index + 1, // Recalculate sequence number just before submission
        description: d.description
      };

      if (d.destinationId) {
        // Existing Destination
        return {
          ...baseDestination,
          destinationId: parseInt(d.destinationId, 10),
        };
      } else {
        // New Destination
        return {
          ...baseDestination,
          destinationId: null,
          name: d.name,
          state: d.state || null, // Allow state to be null/empty
          country: d.country,
          longitude: parseFloat(d.longitude),
          latitude: parseFloat(d.latitude),
        };
      }
    });

    if (finalTripDestinations.length === 0) {
        return setError('Please add at least one trip destination.');
    }

    try {
      const payload = {
        createTrip: {
          ownerId: user.id,
          tripName,
          maxBuddies: parseInt(maxBuddies, 10),
          startDate,
          endDate,
          description, // Trip-level description
          changedBy: user.id
        },
        tripDestinations: finalTripDestinations,
      };
      await tripsService.createTrip(user.id, payload);
      setSuccess('Trip created successfully!');
      // Reset form
      setTripName('');
      setDescription('');
      setMaxBuddies(1);
      setStartDate('');
      setEndDate('');
      setTripDestinations([createNewTripDestination(null, '', '', '', '', '', 1)]);
    } catch (err) {
      setError('Creation failed');
    }
  };

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        <div className="mx-auto card shadow-sm p-4" style={{ maxWidth: 720 }}>
          <h3 className="text-center mb-4">Create a New Trip</h3>
          <form onSubmit={onSubmit}>
            {/* Trip Info Section */}
            <h5 className="mb-3">Trip Information</h5>
            <div className="row mb-3">
              <div className="col-md-6">
                <label className="form-label">Trip Name</label>
                <input className="form-control" value={tripName} onChange={(e) => setTripName(e.target.value)} required />
              </div>
              <div className="col-md-6">
                <label className="form-label">Max Buddies</label>
                <input type="number" className="form-control" value={maxBuddies} min="1" onChange={(e) => setMaxBuddies(e.target.value)} required />
              </div>
            </div>

            <div className="row mb-3">
              <div className="col-md-6">
                <label className="form-label">Trip Start Date</label>
                <input type="date" className="form-control" value={startDate} onChange={(e) => setStartDate(e.target.value)} required />
              </div>
              <div className="col-md-6">
                <label className="form-label">Trip End Date</label>
                <input type="date" className="form-control" value={endDate} onChange={(e) => setEndDate(e.target.value)} required />
              </div>
            </div>

            <div className="mb-3">
              <label className="form-label">Trip Description</label>
              <textarea className="form-control" value={description} onChange={(e) => setDescription(e.target.value)} rows={3} />
            </div>

            <hr />

            {/* Trip Destination Section */}
            <h5 className="mt-4 mb-3">Trip Destinations</h5>
            <h6 className='text-muted mb-4'><small>Please only make a new destination if it does not already exist in the list.</small></h6>
            {/* Destination Search/Filter moved outside the loop for simplicity, but the main state is now an array */}
            <div className="mb-4">
              <label className="form-label">Search Destinations (for selection below)</label>
              <input
                type="text"
                className="form-control"
                placeholder="Search by name, state, country, or coordinates..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
              {searchTerm && (
                <small className="text-muted">
                  {filteredDestinations.length > 0
                    ? `${filteredDestinations.length} destination(s) found.`
                    : 'No destinations match your search'}
                </small>
              )}
            </div>

            {/* Loop through all Trip Destinations */}
            {tripDestinations.map((tripDestination, index) => (
              <div key={index} className="border rounded p-3 mb-4 bg-light">
                <div className="d-flex justify-content-between align-items-center mb-3">
                    <h6 className="mb-0">Destination {index + 1} ({tripDestination.destinationId ? 'Existing' : 'New'})</h6>
                    {tripDestinations.length > 1 && (
                        <button 
                            type="button" 
                            className="btn btn-sm btn-outline-danger" 
                            onClick={() => removeTripDestination(index)}
                        >
                            Remove
                        </button>
                    )}
                </div>

                {/* Existing Destination Selector */}
                <div className="mb-3">
                  <label className="form-label">Choose Existing Destination</label>
                  <select
                    className="form-select"
                    value={tripDestination.destinationId || ''}
                    onChange={(e) => handleSelectExistingDestination(e, index)}
                  >
                    <option value="">-- Create New Destination --</option>
                    {filteredDestinations.map((d) => (
                      <option key={d.destinationId} value={d.destinationId}>
                        {d.name} ({d.country})
                      </option>
                    ))}
                  </select>
                  {tripDestination.destinationId && (
                    <small className="text-muted">
                        Selected: {tripDestination.name} ({tripDestination.state}, {tripDestination.country}) [Lat: {tripDestination.latitude}, Lng: {tripDestination.longitude}]
                    </small>
                  )}
                </div>

                {/* New Destination Details (Only visible if no existing destination is selected) */}
                {!tripDestination.destinationId && (
                  <div className="mt-3 p-3 border rounded bg-white">
                    <h6 className="mb-3">New Destination Details</h6>
                    <div className="mb-2">
                      <label className="form-label">Name</label>
                      <input className="form-control" value={tripDestination.name} onChange={(e) => handleNewDestinationFieldChange(index, 'name', e.target.value)} required={!tripDestination.destinationId} />
                    </div>
                    <div className="mb-2">
                      <label className="form-label">State</label>
                      <input className="form-control" value={tripDestination.state} onChange={(e) => handleNewDestinationFieldChange(index, 'state', e.target.value)} />
                    </div>
                    <div className="mb-2">
                      <label className="form-label">Country</label>
                      <input className="form-control" value={tripDestination.country} onChange={(e) => handleNewDestinationFieldChange(index, 'country', e.target.value)} required={!tripDestination.destinationId} />
                    </div>
                    <div className="row">
                      <div className="col-md-6 mb-2">
                        <label className="form-label">Latitude</label>
                        <input type="number" step="0.0000001" className="form-control" value={tripDestination.latitude} onChange={(e) => handleNewDestinationFieldChange(index, 'latitude', e.target.value)} required={!tripDestination.destinationId} />
                      </div>
                      <div className="col-md-6 mb-2">
                        <label className="form-label">Longitude</label>
                        <input type="number" step="0.0000001" className="form-control" value={tripDestination.longitude} onChange={(e) => handleNewDestinationFieldChange(index, 'longitude', e.target.value)} required={!tripDestination.destinationId} />
                      </div>
                    </div>
                  </div>
                )}
                
                {/* Trip Destination Specific Details */}
                <h6 className="mt-3 mb-3">Destination Schedule & Details</h6>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label className="form-label">Start Date</label>
                    <input 
                        type="date" 
                        className="form-control" 
                        value={tripDestination.destinationStartDate} 
                        onChange={(e) => handleTripDestinationChange(index, 'destinationStartDate', e.target.value)} 
                        required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">End Date</label>
                    <input 
                        type="date" 
                        className="form-control" 
                        value={tripDestination.destinationEndDate} 
                        onChange={(e) => handleTripDestinationChange(index, 'destinationEndDate', e.target.value)} 
                        required 
                    />
                  </div>
                </div>
                <div className="mb-3">
                    <label className="form-label">Description</label>
                    <textarea 
                        className="form-control" 
                        value={tripDestination.description} 
                        onChange={(e) => handleTripDestinationChange(index, 'description', e.target.value)} 
                        rows={2} 
                    />
                </div>
              </div>
            ))}
            
            <button type="button" className="btn btn-outline-secondary w-100 mb-4" onClick={addTripDestination}>
                + Add Another Trip Destination
            </button>

            {error && <div className="alert alert-danger mt-3">{error}</div>}
            {success && <div className="alert alert-success mt-3">{success}</div>}

            <button className="btn btn-primary w-100 mt-4">Create Trip</button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CreateTripPage;
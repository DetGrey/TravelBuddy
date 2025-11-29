import React, { useState, useEffect } from 'react';
import tripsService from '../services/tripsService';
import NavBar from '../components/NavBar';
import { Link } from 'react-router-dom';

// Keys for localStorage
const SEARCH_FILTERS_KEY = 'tripSearchFilters';
const SEARCH_RESULTS_KEY = 'tripSearchResults';

// --- Date Formatting Helper ---
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
// -----------------------------

// Function to safely parse state from localStorage
const loadState = (key, defaultValue) => {
  try {
    const serializedState = localStorage.getItem(key);
    if (serializedState === null) {
      return defaultValue;
    }
    return JSON.parse(serializedState);
  } catch (e) {
    console.error("Could not load state from localStorage", e);
    return defaultValue;
  }
};

// Define the default empty state for filters
const defaultFilters = {
  q: '',
  reqStart: '',
  reqEnd: '',
  country: '',
  state: '',
  partySize: '',
};

const TripSearchPage = () => {
  // Load initial state from localStorage or use defaults
  const initialFilters = loadState(SEARCH_FILTERS_KEY, defaultFilters);

  const [q, setQ] = useState(initialFilters.q);
  const [reqStart, setReqStart] = useState(initialFilters.reqStart);
  const [reqEnd, setReqEnd] = useState(initialFilters.reqEnd);
  const [country, setCountry] = useState(initialFilters.country);
  const [state, setState] = useState(initialFilters.state);
  const [partySize, setPartySize] = useState(initialFilters.partySize);
  
  const [results, setResults] = useState(loadState(SEARCH_RESULTS_KEY, []));
  const [loading, setLoading] = useState(false);
  const [searched, setSearched] = useState(!!results.length);

  // --- Effect to save filters and results when they change ---
  useEffect(() => {
    const filtersToSave = { q, reqStart, reqEnd, country, state, partySize };
    localStorage.setItem(SEARCH_FILTERS_KEY, JSON.stringify(filtersToSave));
    localStorage.setItem(SEARCH_RESULTS_KEY, JSON.stringify(results));
  }, [q, reqStart, reqEnd, country, state, partySize, results]);


  const onSearch = async (e) => {
    e?.preventDefault();
    setLoading(true);
    setSearched(true);

    const params = {
      q,
      reqStart: reqStart || undefined,
      reqEnd: reqEnd || undefined,
      country: country || undefined,
      state: state || undefined,
      partySize: partySize ? parseInt(partySize, 10) : undefined,
    };

    const filteredParams = Object.fromEntries(
      Object.entries(params).filter(([, v]) => v !== undefined && v !== '')
    );
    
    try {
      const data = await tripsService.search(filteredParams); 
      setResults(data || []);
    } catch (err) {
      console.error(err);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };
  
  const onReset = () => {
    setQ('');
    setReqStart('');
    setReqEnd('');
    setCountry('');
    setState('');
    setPartySize('');
    setResults([]);
    setSearched(false);
  };

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        <h3>Search Trips</h3>
        
        {/* --- Filters Form --- */}
        <div className="mx-auto" style={{ maxWidth: 900 }}>
          <form onSubmit={onSearch} className="mb-4 border rounded shadow-sm bg-light">
            
            {/* 1. Keyword Search (Full Width) */}
            <div className="p-4 border-bottom">
              <label htmlFor="q-search" className="form-label visually-hidden">Keyword Search</label>
              <input 
                id="q-search"
                className="form-control form-control-lg" 
                value={q} 
                onChange={(e) => setQ(e.target.value)} 
                placeholder="Search by destination name or trip title (Keyword)" 
              />
            </div>

            {/* 2. Grouped Filters (Dates & Location) */}
            <div className="p-4 border-bottom">
              <div className="row g-3">
                {/* Dates Group */}
                <div className="col-lg-6">
                  <p className="fw-bold mb-2">Trip Dates</p>
                  <div className="row g-3">
                    <div className="col-md-6">
                      <label htmlFor="reqStart" className="form-label">Start Date (On/After)</label>
                      <input 
                        id="reqStart"
                        type="date"
                        className="form-control" 
                        value={reqStart} 
                        onChange={(e) => setReqStart(e.target.value)} 
                      />
                    </div>
                    <div className="col-md-6">
                      <label htmlFor="reqEnd" className="form-label">End Date (On/Before)</label>
                      <input 
                        id="reqEnd"
                        type="date"
                        className="form-control" 
                        value={reqEnd} 
                        onChange={(e) => setReqEnd(e.target.value)} 
                      />
                    </div>
                  </div>
                </div>

                {/* Location Group */}
                <div className="col-lg-6">
                  <p className="fw-bold mb-2">Location</p>
                  <div className="row g-3">
                    <div className="col-md-6">
                      <label htmlFor="country" className="form-label">Country</label>
                      <input 
                        id="country"
                        className="form-control" 
                        value={country} 
                        onChange={(e) => setCountry(e.target.value)} 
                        placeholder="e.g., USA" 
                      />
                    </div>
                    <div className="col-md-6">
                      <label htmlFor="state" className="form-label">State</label>
                      <input 
                        id="state"
                        className="form-control" 
                        value={state} 
                        onChange={(e) => setState(e.target.value)} 
                        placeholder="e.g., CA" 
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            
            {/* 3. Capacity and Buttons */}
            <div className="p-4">
              <div className="row g-3 align-items-end">
                <div className="col-md-4">
                  <label htmlFor="partySize" className="form-label">Party Size Needed</label>
                  <input 
                    id="partySize"
                    type="number"
                    min="1"
                    className="form-control" 
                    value={partySize} 
                    onChange={(e) => setPartySize(e.target.value)} 
                    placeholder="1"
                  />
                </div>
                
                {/* Action Buttons */}
                <div className="col-md-8 d-flex justify-content-end gap-3">
                  <button className="btn btn-outline-secondary px-4" type="button" onClick={onReset} disabled={loading}>
                    Reset Filters
                  </button>
                  <button className="btn btn-primary px-5" type="submit" disabled={loading}>
                    {loading ? 'Searching...' : 'Search Trips'}
                  </button>
                </div>
              </div>
            </div>
            
          </form>
        </div>
        {/* --- End Filters Form --- */}

        <hr/>

        {/* --- Search Results --- */}
        <div className="mx-auto">
          <h4>Results ({results.length})</h4>
          
          {loading && <div className="text-center my-4">Loading results...</div>}

          {!loading && searched && results.length === 0 && (
            <div className="alert alert-info" role="alert">
              No trips match your search criteria. Try broadening your filters!
            </div>
          )}

          <div className="row g-4 mt-2">
            {results.map((t) => (
              // CRITICAL FIX: Changed col-lg-3 back to col-lg-4 for 3 broader cards per row
              <div key={t.tripDestinationId} className="col-sm-12 col-md-6 col-lg-4">
                <div className="card shadow-sm h-100">
                  <div className="card-body d-flex flex-column">
                    <h5 className="card-title mb-1">
                      <Link to={`/trip-destinations/${t.tripDestinationId}`} className="text-decoration-none text-primary">
                        {t.destinationName}
                      </Link>
                    </h5>
                    <h6 className="card-subtitle text-muted mb-3">
                      {t.country}{t.state ? `, ${t.state}` : ''}
                    </h6>
                    
                    {/* --- Cleaner Display Block with formatted dates --- */}
                    <div className="mb-3 p-2 bg-light rounded small">
                      <div className="d-flex justify-content-between">
                        <span className="text-secondary"><strong>Start Date:</strong></span>
                        <span className="fw-bold">{formatDate(t.destinationStart)}</span>
                      </div>
                      <div className="d-flex justify-content-between mt-1">
                        <span className="text-secondary"><strong>End Date:</strong></span>
                        <span className="fw-bold">{formatDate(t.destinationEnd)}</span>
                      </div>
                      <div className="d-flex justify-content-between mt-2 pt-2 border-top">
                        <span className="text-secondary"><strong>Capacity:</strong></span>
                        <span>{t.acceptedPersons} accepted of <strong>{t.maxBuddies}</strong> total</span>
                      </div>
                    </div>
                    
                    <div className="d-flex justify-content-between align-items-center mb-3">
                      <span className="fw-bold text-success fs-5">
                        <strong>{t.remainingCapacity} Open Spots</strong>
                      </span>
                      <Link to={`/trip-destinations/${t.tripDestinationId}`} className="btn btn-sm btn-outline-primary">
                        View Trip
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TripSearchPage;
import React, { useState } from 'react';
import tripsService from '../services/tripsService';
import NavBar from '../components/NavBar';

const CreateTripPage = () => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [destination, setDestination] = useState('');
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  const onSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      // NOTE: backend currently has no POST /api/trip-destinations endpoint
      // TODO: add server-side create trip endpoint then wire up fully.
      await tripsService.createTrip({ title, description, destination });
      setSuccess('Trip created successfully');
      setTitle(''); setDescription(''); setDestination('');
    } catch (err) {
      setError(err?.response?.data?.message || err.message || 'Creation failed');
    }
  };

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        
        {/* Layout Fix: Center the form using mx-auto */}
        <div className="mx-auto" style={{ maxWidth: 640 }}>
            <h3 className="text-center mb-4">Create Trip</h3>
            <form onSubmit={onSubmit}>
                <div className="mb-3">
                    <label className="form-label">Title</label>
                    <input className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} required />
                </div>

                <div className="mb-3">
                    <label className="form-label">Destination</label>
                    <input className="form-control" value={destination} onChange={(e) => setDestination(e.target.value)} required />
                </div>

                <div className="mb-3">
                    <label className="form-label">Description</label>
                    <textarea className="form-control" value={description} onChange={(e) => setDescription(e.target.value)} rows={5} />
                </div>

                {error && <div className="alert alert-danger">{error}</div>}
                {success && <div className="alert alert-success">{success}</div>}

                <button className="btn btn-primary w-100">Create</button>
            </form>
        </div>
      </div>
    </div>
  );
};

export default CreateTripPage;
import React, { useState } from 'react';
import NavBar from '../components/NavBar';

const NewConversationPage = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState(null);
  // navigation not needed — creating conversations isn't supported server-side yet

  const onSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    // NOTE: the backend currently does not implement POST /api/conversations to create a new conversation.
    // TODO: add an endpoint to create a conversation (server-side) and then implement this function.
    setError('Starting a new conversation is not available - backend endpoint missing (TODO)');
  };

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        
        {/* Layout Fix: Center the form using mx-auto and contain the header */}
        <div className="mx-auto" style={{ maxWidth: 640 }}>
          <h3 className="text-center mb-4">Start a new conversation</h3>
          <form onSubmit={onSubmit}>
            <div className="mb-3">
              <label className="form-label">User email</label>
              <input className="form-control" value={email} onChange={(e) => setEmail(e.target.value)} type="email" required />
            </div>
            <div className="mb-3">
              <label className="form-label">Message</label>
              <textarea className="form-control" value={message} onChange={(e) => setMessage(e.target.value)} rows={4} required />
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <button className="btn btn-primary w-100">Start</button>
          </form>
        </div>
        
      </div>
    </div>
  );
};

export default NewConversationPage;
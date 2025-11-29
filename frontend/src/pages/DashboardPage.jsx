import React, { useContext, useEffect, useState } from 'react';
import NavBar from '../components/NavBar';
import { AuthContext } from '../context/AuthContext';
import authService from '../services/authService';

const DashboardPage = () => {
  const { user, logout } = useContext(AuthContext);
  const [me, setMe] = useState(null);

  // Helper function to format the DateOnly string (YYYY-MM-DD)
  const formatBirthdate = (dateString) => {
    if (!dateString) return 'N/A';
    try {
      // Add T00:00:00 to ensure correct timezone interpretation for simple dates
      return new Date(dateString + 'T00:00:00').toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      });
    } catch {
      return dateString;
    }
  };

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        if (!user?.id) return;
        // Fetch user profile data
        const data = await authService.me(user.id);
        if (mounted) setMe(data);
      } catch (err) {
        console.error('Fetching user profile failed, forcing logout', err?.response?.data || err.message || err);
        await logout();
        if (mounted) setMe(null);
      }
    })();
    return () => (mounted = false);
  }, [user, logout]);

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        {/* Centered Content Wrapper with max-width */}
        <div className="mx-auto" style={{ maxWidth: 680 }}>
          
          <h3 className="text-center mb-4">Welcome, {me?.name || 'Traveler'}! 👋</h3>
          <p className="lead text-center mb-5">
            Use the navigation to create trips, search, manage buddies and message other users.
          </p>
          
          <hr />

          {/* User Profile Card */}
          <div className="card p-4 shadow-sm">
            <h5 className="card-title mb-3">👤 Your Profile Details</h5>
            
            {me ? (
              <ul className="list-group list-group-flush">
                <li className="list-group-item d-flex justify-content-between align-items-center">
                  <strong>Name</strong>
                  <span>{me.name}</span>
                </li>
                <li className="list-group-item d-flex justify-content-between align-items-center">
                  <strong>Email</strong>
                  <span>{me.email}</span>
                </li>
                <li className="list-group-item d-flex justify-content-between align-items-center">
                  <strong>Birthdate</strong>
                  <span>{formatBirthdate(me.birthdate)}</span>
                </li>
                <li className="list-group-item d-flex justify-content-between align-items-center">
                  <strong>Role</strong>
                  <span className="badge bg-secondary">{user?.role || 'User'}</span>
                </li>
                <li className="list-group-item d-flex justify-content-between align-items-center text-muted small">
                  User ID
                  <span>{me.userId}</span>
                </li>
              </ul>
            ) : (
              <div className="text-center text-muted">Loading user data...</div>
            )}
            
          </div>
          {/* End User Profile Card */}

        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
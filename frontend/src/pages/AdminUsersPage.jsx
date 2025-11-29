import React, { useEffect, useState } from 'react';
import usersService from '../services/usersService';
import NavBar from '../components/NavBar';

const AdminUsersPage = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data = await usersService.getAll();
        if (mounted) setUsers(data || []);
      } catch (err) { console.error(err); }
    })();
    return () => (mounted = false);
  }, []);

  return (
    <div>
      <NavBar />
      <div className="container">
        <h3>All users (admin)</h3>
        <div style={{ maxWidth: 900 }}>
          <div className="list-group">
            {users.length === 0 && <div className="text-muted">No users</div>}
            {users.map(u => (
              <div key={u.userId} className="list-group-item">
                <div className="d-flex justify-content-between">
                  <div>
                    <strong>{u.name || u.email}</strong>
                    <div className="text-muted small">{u.email}</div>
                  </div>
                    <div>{u.role ?? '—'}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminUsersPage;

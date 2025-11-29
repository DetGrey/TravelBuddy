import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const NavBar = () => {
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const onLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar navbar-expand navbar-dark bg-dark fixed-top mb-4">
      <div className="container-fluid px-3">
        <Link className="navbar-brand" to="/">TravelBuddy</Link>

        <div className="navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link className="nav-link" to="/">Dashboard</Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link" to="/search">Search Trips</Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link" to="/create">Create Trip</Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link" to="/my-trips">My Trips</Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link" to="/messages">Messages</Link>
            </li>

          </ul>

          <ul className="navbar-nav">
            {user?.role === 'ADMIN' && (
              <li className="nav-item">
                <Link className="nav-link text-danger" to="/admin">Admin: Users</Link>
              </li>
            )}

            {user ? (
              <>
                <li className="nav-item nav-link">Role: <strong>{String(user.role)}</strong></li>
                <li className="nav-item">
                  <button className="btn btn-outline-secondary" onClick={onLogout}>Logout</button>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/login">Login</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/register">Register</Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default NavBar;

import React, { useState, useContext } from 'react';
import authService from '../services/authService';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const RegisterPage = () => {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');
  const [birthdate, setBirthdate] = useState('');
  const [success, setSuccess] = useState(null);
  const [error, setError] = useState(null);
  const nav = useNavigate();
  const { login: contextLogin } = useContext(AuthContext);
  // if already logged in, send to dashboard
  // (AuthContext not required here but redirect helps when a user is already signed in)

  const onSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      await authService.register({ email, password, name, birthdate });
      setSuccess('Account created! Logging you in...');
      
      // Auto-login after registration
      const loginResult = await authService.login({ email, password });
      
      // Store the token in context and localStorage
      if (loginResult.token) {
        contextLogin(loginResult.token);
      }
      
      // Redirect to dashboard
      setTimeout(() => nav('/'), 800);
    } catch (err) {
      setError(err?.response?.data?.message || err.message || 'Registration failed');
    }
  };

  return (
    <div className="container">
      <h2>Register</h2>
      <form onSubmit={onSubmit} style={{ maxWidth: 480 }}>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <div className="mb-3">
          <label className="form-label">Email</label>
          <input className="form-control" value={email} onChange={(e) => setEmail(e.target.value)} type="email" required />
        </div>
        <div className="mb-3">
          <label className="form-label">Password</label>
          <input className="form-control" value={password} onChange={(e) => setPassword(e.target.value)} type="password" required />
        </div>
        <div className="mb-3">
          <label className="form-label">Birthdate</label>
          <input className="form-control" value={birthdate} onChange={(e) => setBirthdate(e.target.value)} type="date" required />
        </div>

        {error && <div className="alert alert-danger">{error}</div>}
        {success && <div className="alert alert-success">{success}</div>}

        <button className="btn btn-primary">Create account</button>
      </form>
    </div>
  );
};

export default RegisterPage;

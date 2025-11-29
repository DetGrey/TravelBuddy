import React, { useState, useContext, useEffect } from 'react';
import authService from '../services/authService';
import { AuthContext } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const { login, user } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    // Redirect if the user is already logged in
    if (user) {
        navigate('/');
    }
  }, [user, navigate]);

  const onSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      const result = await authService.login({ email, password });
      if (result?.token) {
        login(result.token);
        navigate('/');
      } else {
        setError('Login did not return a token');
      }
    } catch (err) {
      setError(err?.response?.data?.message || err.message || 'Login failed');
    }
  };

  return (
    // The main container
    <div className="container py-5"> 
      {/* Bootstrap row for centering content in the middle of the screen */}
      <div className="row justify-content-center">
        {/* Define column size for smaller width, allowing mx-auto on the form to center it */}
        <div className="col-12 col-sm-10 col-md-8 col-lg-6"> 
          
          <h2 className="text-center mb-4">Login</h2>

          <form 
            onSubmit={onSubmit} 
            // The key change: mx-auto centers the block element
            className="p-4 border rounded shadow-sm bg-white mx-auto" 
            style={{ maxWidth: '480px' }} // Keep max-width for consistency
          >
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                type="email"
                required
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Password</label>
              <input
                className="form-control"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                type="password"
                required
              />
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <button className="btn btn-primary w-100">Login</button>
          </form>

        </div>
      </div>
    </div>
  );
};

export default LoginPage;
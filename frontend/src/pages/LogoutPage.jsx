import React, { useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const LogoutPage = () => {
  const { logout } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        await logout();
      } catch (err) {
        // ignore
        console.error('Logout failed', err);
      }
      if (mounted) navigate('/login');
    })();
    return () => {
      mounted = false;
    };
  }, [logout, navigate]);

  return null;
};

export default LogoutPage;

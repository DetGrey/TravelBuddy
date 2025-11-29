/* eslint-disable react-refresh/only-export-components */
import React, { createContext, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import authService from '../services/authService';

export const AuthContext = createContext();

// --- Define C# Claim Constants ---
const USER_ID_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const EMAIL_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
// ---------------------------------

// --- Search Persistence Keys ⬅️ ADDED THESE CONSTANTS
const SEARCH_FILTERS_KEY = 'tripSearchFilters';
const SEARCH_RESULTS_KEY = 'tripSearchResults';

/**
 * Processes the JWT token to extract user details (id, role, email) using 
 * specific C# claim URLs and checks for expiration.
 */
const decodeAndMapUser = (token) => {
    if (!token) return null;
    
    try {
        const decodedToken = jwtDecode(token);
        
        // Expiration Check
        const exp = decodedToken.exp || decodedToken.expiration;
        if (exp && Date.now() >= exp * 1000) {
            localStorage.removeItem('token');
            return null;
        }

        return {
            token,
            // Use C# claims first, fallback to standard JWT claims
            role: decodedToken[ROLE_CLAIM] || decodedToken.role || decodedToken.roles,
            id: decodedToken[USER_ID_CLAIM] || decodedToken.sub || decodedToken.userId || decodedToken.id,
            email: decodedToken[EMAIL_CLAIM] || decodedToken.email
        };
    } catch (err) {
        console.error('Invalid token during decode:', err);
        localStorage.removeItem('token');
        return null;
    }
}


export const AuthProvider = ({ children }) => {
    // 1. Initial State: Load token from localStorage and process it
    const [user, setUser] = useState(() => {
        const token = localStorage.getItem('token');
        return decodeAndMapUser(token);
    });

    // 2. Login Function: Saves token and immediately processes it to update state
    const login = (token) => {
        localStorage.setItem('token', token);
        
        const newUser = decodeAndMapUser(token);
        
        if (newUser) {
            setUser(newUser);
        } else {
            // If the token was invalid or expired during the login attempt
            setUser(null);
            localStorage.removeItem('token');
        }
    };

    const logout = async () => {
        try {
            // Clear server-side session (cookie/token)
            await authService.logout();
        } catch (err) {
            console.error('Logout API failed or returned error', err?.response?.data || err.message || err);
        }
        
        // Clear local authentication state
        localStorage.removeItem('token');
        setUser(null);
        
        // Clear saved search filters and results
        localStorage.removeItem(SEARCH_FILTERS_KEY);
        localStorage.removeItem(SEARCH_RESULTS_KEY);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};
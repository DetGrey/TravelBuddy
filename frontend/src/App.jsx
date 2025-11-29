import { HashRouter, Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import CreateTripPage from './pages/CreateTripPage';
import TripSearchPage from './pages/TripSearchPage';
import TripDetailsPage from './pages/TripDetailsPage';
import MyTripsPage from './pages/MyTripsPage';
import MessagesPage from './pages/MessagesPage';
import ConversationPage from './pages/ConversationPage';
import NewConversationPage from './pages/NewConversationPage';
import AdminUsersPage from './pages/AdminUsersPage';
import LogoutPage from './pages/LogoutPage';
import ProtectedRoute from './components/ProtectedRoute';
import './App.css';

function App() {
  return (
    <HashRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route element={<ProtectedRoute />}> 
          <Route path="/" element={<DashboardPage />} />
          <Route path="/create" element={<CreateTripPage />} />
          <Route path="/search" element={<TripSearchPage />} />
          <Route path="/trip-destinations/:id" element={<TripDetailsPage />} />
          <Route path="/my-trips" element={<MyTripsPage />} />
          <Route path="/messages" element={<MessagesPage />} />
          <Route path="/messages/new" element={<NewConversationPage />} />
          <Route path="/messages/:id" element={<ConversationPage />} />
        </Route>

        {/* Public logout route: call logout and redirect - handy for forcing a clean logout */}
        <Route path="/logout" element={<LogoutPage />} />

        <Route element={<ProtectedRoute requiredRole={'ADMIN'} />}>
          <Route path="/admin" element={<AdminUsersPage />} />
        </Route>
      </Routes>
    </HashRouter>
  );
}

export default App;

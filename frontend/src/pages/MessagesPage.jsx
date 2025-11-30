import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import messagesService from '../services/messagesService';
import NavBar from '../components/NavBar';
import { Link } from 'react-router-dom';

const MessagesPage = () => {
  const [conversations, setConversations] = useState([]);
  const { user } = useContext(AuthContext);

  // Helper to format the time
  const formatTime = (dateString) => {
    if (!dateString) return '';
    try {
      return new Date(dateString).toLocaleTimeString('en-US', {
        hour: 'numeric', 
        minute: '2-digit', 
        day: 'numeric', 
        month: 'short'
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
        const data = await messagesService.getConversations(user.id);
        if (mounted) setConversations(data || []);
      } catch (err) { console.error(err); }
    })();
    return () => (mounted = false);
  }, [user]);

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        
        {/* Centered Content Wrapper */}
        <div className="mx-auto" style={{ maxWidth: 900 }}>
          
          <h3 className="text-center mb-4">Messages</h3>
          
          {/* Action Button */}
          <Link to="/messages/new" className="btn btn-primary mb-3">Start new conversation</Link>

          <div className="list-group shadow-sm">
            {conversations.length === 0 && (
                <div className="list-group-item text-muted text-center py-4">
                    You have no active conversations. Start one now!
                </div>
            )}
            
            {conversations.map(c => {
              const displayName = c.isGroup
                  ? c.conversationName || `Group Chat (${c.participantCount})`
                  : c.conversationName || `Conversation (${c.participantCount})`;

              return (
                  <Link
                      key={c.conversationId}
                      to={`/messages/${c.conversationId}`}
                      className="list-group-item list-group-item-action py-3"
                  >
                      <div className="d-flex w-100 justify-content-between">
                          {/* Main Title and Archived Status */}
                          <h5 className="mb-1 fw-bold">
                              {displayName}
                              <span className="ms-2">({c.participantCount})</span>
                              {c.isArchived && (
                                  <span className="badge bg-secondary ms-2">Archived</span>
                              )}
                          </h5>
                          <small className="text-muted">
                              {formatTime(c.lastMessageAt || c.createdAt)}
                          </small>
                      </div>

                      {/* Participants, Destination, Last Message Preview */}
                      <div className="d-flex w-100 justify-content-between">
                          <div>
                              <p className="mb-1 text-muted small">
                                  {c.lastMessagePreview || 'No messages yet'}
                              </p>
                          </div>
                          {c.unreadCount > 0 && (
                              <span className="badge bg-danger rounded-pill">{c.unreadCount}</span>
                          )}
                      </div>
                  </Link>
              );
          })}
          </div>
        </div>
        
      </div>
    </div>
  );
};

export default MessagesPage;
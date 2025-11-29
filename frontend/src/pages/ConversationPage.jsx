import React, { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import messagesService from '../services/messagesService';
import { AuthContext } from '../context/AuthContext';
import NavBar from '../components/NavBar';

const ConversationPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [conversation, setConversation] = useState(null);
  const { user } = useContext(AuthContext);
  const [msg, setMsg] = useState('');

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        if (!user?.id) return;
        const data = await messagesService.getConversation(id, user.id);
        if (mounted) setConversation(data);
      } catch (e) {
        console.error(e);
      }
    })();
    return () => (mounted = false);
  }, [id, user]);

  const onSend = async (e) => {
    e.preventDefault();
    if (!msg) return;
    try {
      if (!user?.id) throw new Error('Missing user id');
      await messagesService.sendMessage(id, user.id, { content: msg });
      setMsg('');
      // Optimistic update or scroll to bottom logic could be added here
      const updated = await messagesService.getConversation(id, user.id);
      setConversation(updated);
    } catch (e) { console.error(e); }
  };

  if (!conversation) return <div><NavBar /><div className="container py-4">Loading…</div></div>;
  
  // Use a constant for max-width to apply centering consistently
  const MAX_WIDTH = 800; 

  return (
    <div>
      <NavBar />
      <div className="container py-4">
        
        {/* Centered Content Wrapper */}
        <div className="mx-auto" style={{ maxWidth: MAX_WIDTH }}>
          
          <button className="btn btn-link mb-2" onClick={() => navigate('/messages')}>← Back to Messages</button>
          
          <h4>
            {conversation.isGroup 
              ? `Group Chat` 
              : `Conversation with ${conversation.participants?.find(p => p.id !== user.id)?.name || 'Participant'} (${conversation.participantCount || '2'})`
            }
          </h4>

          {/* Message Display Area - Centered */}
          <div className="border p-3 mb-3 rounded bg-light" style={{ minHeight: 300, maxHeight: 500, overflowY: 'auto' }}>
            {conversation.messages?.length === 0 && (
                <div className="text-center text-muted py-5">Start the conversation!</div>
            )}
            {conversation.messages?.map(m => (
              // Enhanced message bubble display
              <div 
                key={m.id} 
                className={`mb-3 p-2 rounded shadow-sm ${m.senderId === user.id ? 'ms-auto bg-primary text-white' : 'me-auto bg-white border'}`}
                style={{ maxWidth: '85%' }}
              >
                <div className="fw-bold mb-1 d-flex justify-content-between">
                    <span>{m.senderName}</span> 
                    <small style={{ opacity: 0.8 }}>{new Date(m.sentAt).toLocaleString('en-US', { hour: 'numeric', minute: '2-digit' })}</small>
                </div>
                <div>{m.content}</div>
              </div>
            ))}
          </div>

          {/* Message Input Form - Centered */}
          <form onSubmit={onSend}>
            <div className="input-group">
              <input 
                className="form-control" 
                value={msg} 
                onChange={(e) => setMsg(e.target.value)} 
                placeholder="Type your message..."
                aria-label="New Message"
              />
              <button className="btn btn-primary" type="submit" disabled={!msg}>Send</button>
            </div>
          </form>
        </div>
        
      </div>
    </div>
  );
};

export default ConversationPage;
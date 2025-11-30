import React, { useEffect, useState, useContext, useRef } from 'react';
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
  const messagesEndRef = useRef(null);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        if (!user?.id) return;
        const data = await messagesService.getConversation(id, user.id);
        if (mounted) setConversation(data);
      } catch (e) {
        console.error('getConversation error:', e);
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
      const updated = await messagesService.getConversation(id, user.id);
      setConversation(updated);
    } catch (e) {
      console.error('sendMessage error:', e);
    }
  };

  // Auto-scroll to bottom on updates
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [conversation]);

  if (!conversation) {
    return (
      <div>
        <NavBar />
        <div className="container py-4 text-center">Loading…</div>
      </div>
    );
  }

  return (
    <div className="d-flex flex-column min-vh-100">
      <NavBar />

      <div className="flex-grow-1 d-flex justify-content-center py-3">
        <div className="w-100 d-flex flex-column" style={{ maxWidth: 700 }}>
          
          {/* Header */}
          <div className="d-flex align-items-center mb-3">
            <button 
              className="btn btn-outline-secondary btn-sm me-2" 
              onClick={() => navigate('/messages')}
              aria-label="Back to messages"
              title="Back"
            >
              ← Back
            </button>
            <h5 className="mb-0">
              {conversation.isGroup 
                ? `Group Chat (${conversation.participantCount})`
                : `Chat with ${conversation.participants?.find(p => String(p.id) !== String(user.id))?.name || 'Participant'}`
              }
            </h5>
          </div>

          {/* Messages */}
          <div 
            className="flex-grow-1 border rounded bg-light p-3 mb-3 overflow-auto"
            style={{ minHeight: 0 }}
          >
            {conversation.messages?.length === 0 && (
              <div className="text-muted text-center py-5">Start the conversation!</div>
            )}

            {conversation.messages?.map((m) => {
              const isMine = String(m.senderId) === String(user.id);

              // Debug logs to trace alignment issues
              console.log('[Message debug]', {
                messageId: m.id,
                senderId: m.senderId,
                userId: user.id,
                isMine,
                content: m.content,
              });

              return (
                <div
                  key={m.id}
                  className={`d-flex mb-3 ${isMine ? 'justify-content-end' : 'justify-content-start'}`}
                >
                  <div
                    className={`message-bubble p-2 rounded shadow-sm ${isMine ? 'bg-primary text-white' : 'bg-white border'}`}
                    style={{ maxWidth: '75%' }}
                  >
                    {/* Top row: name (top-left) and time (top-right) */}
                    <div className="d-flex justify-content-between align-items-start mb-1 gap-2">
                      <span className={`fw-bold ${isMine ? 'text-white' : ''}`}>{m.senderName}</span>
                      <small className={isMine ? 'text-white-50' : 'text-muted'}>
                        {new Date(m.sentAt).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' })}
                      </small>
                    </div>

                    {/* Message content (left-aligned) */}
                    <div className="text-start">{m.content}</div>
                  </div>
                </div>
              );
            })}
            <div ref={messagesEndRef} />
          </div>

          {/* Input (sticks to bottom of column) */}
          <form onSubmit={onSend} className="mt-auto">
            <div className="input-group">
              <input
                className="form-control"
                value={msg}
                onChange={(e) => setMsg(e.target.value)}
                placeholder="Type your message..."
                aria-label="Type your message"
              />
              <button className="btn btn-primary" type="submit" disabled={!msg}>
                Send
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default ConversationPage;
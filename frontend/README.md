# TravelBuddy — Frontend (Vite + React)

This is a minimal React frontend used to exercise, test and verify backend APIs for the TravelBuddy project (authentication, trips, buddy requests and messaging).

It was implemented to be intentionally small and usable for E2E verification — not a production UX. The UI covers:

- User authentication (login, register, JWT persistence)
- Trip CRUD / owner flows (create/search/details, pending requests, accept/reject)
- Buddy actions (request join / leave / owner remove)
- Messaging (list conversations, view messages, send, start new convos)
- Role-aware UI (Admin-only pages and role-based element hiding)

---

## Quick start — run locally (dev)

1. From the `frontend` folder install dependencies (if needed):

```bash
cd frontend
npm install
```

2. Start the dev server:

```bash
npm run dev
```

3. Open: http://localhost:5173

> Note: by default the frontend expects a backend API at `https://localhost:7164/api` (the default https dev port from TravelBuddy.Api). For local development you should trust the ASP.NET dev certificate (see below). You can also change the URL via environment variables (recommended).

---

## Build & preview (production build)

```bash
npm run build
npm run preview      # check locally that static bundle serves correctly
```

---

## ESLint — what's included and how to run it

This project has ESLint set up and the `lint` command is available in `package.json`:

```json
{
  "scripts": { "lint": "eslint ." }
}
```

- Config: `eslint.config.js` (flat config using `@eslint/js` with React Hooks and Vite-friendly rules via `eslint-plugin-react-hooks` and `eslint-plugin-react-refresh`)
- Run the linter:

```bash
npm run lint
```

If you want to make linting part of pre-commit or CI, run `npm run lint` as part of your checks and fix issues with the linter output.

---

## Where to change the backend API URL

The frontend reads the API base URL from the Vite environment variable `VITE_API_BASE_URL` and falls back to `https://localhost:7164/api`.

- File: `src/services/api.js`
- Default: `https://localhost:7164/api` (recommended)

### Using HTTPS (recommended)

The backend ships with a development HTTPS endpoint on port 7164. Browsers will block preflight/POST requests that are redirected from HTTP→HTTPS, so using HTTPS avoids that class of CORS errors.

To make HTTPS work smoothly on your dev machine follow these steps (Windows/macOS):

1. Trust the .NET dev certificate if you haven't already:

```bash
dotnet dev-certs https --trust
```

2. Set the frontend to use the HTTPS backend (optional — the app defaults to this):

```bash
# in frontend/.env
VITE_API_BASE_URL=https://localhost:7164/api
```

3. Restart the dev server after changing `.env`.

Note: axios in this project is configured with `withCredentials: true` so it will include cookies if your backend sets them. The backend CORS policy already allows credentials from the Vite dev origin (`http://localhost:5173`).
- Override: create a local `.env` file (or CI variable) with:

```
VITE_API_BASE_URL=https://your-backend.example.com/api
```

Note: Vite exposes env vars starting with `VITE_` to the client app. After changing `.env` you typically need to restart the dev server.

---

## Project structure (high level)

- `src/context/AuthContext.jsx` — JWT and role handling, persisted in localStorage
- `src/services/*` — small helpers for backend endpoints (`api.js`, `authService.js`, `tripsService.js`, `messagesService.js`, `usersService.js`)
- `src/components/*` — shared components (NavBar, ProtectedRoute)
- `src/pages/*` — pages used by the app (Login, Register, Dashboard, Search, Create, Details, Owner, Messaging, Admin)

---

## Backend endpoints (what the API currently exposes)

The frontend has been updated to call endpoints present in the current backend (TravelBuddy.Api). The main routes the UI expects are:

- Users & auth
  - POST /api/users/login  (login)
  - POST /api/users/register  (register)
  - POST /api/users/logout
  - DELETE /api/users/{id}/delete-user
  - POST /api/users/{id}/change-password
  - GET /api/users  (admin only)

- Trip destinations / trips
  - GET /api/trip-destinations/search?q=...  (search)

- Buddy requests / owner flows (user scoped)
  - POST /api/users/{userId}/buddy-requests  (create a request for user {userId})
  - GET /api/users/{userId}/buddy-requests/pending  (pending requests)
  - POST /api/users/{userId}/buddy-requests/update  (accept/reject)

- User trips / leave a trip
  - GET /api/users/{userId}/trip-destinations/trip-destinations  (user's trips)
  - DELETE /api/users/{userId}/trip-destinations/trip-destinations/{tripDestinationId}/leave

- Messaging
  - GET /api/conversations?userId={userId}
  - GET /api/conversations/{conversationId}?userId={userId}
  - GET /api/conversations/{conversationId}/messages?userId={userId}
  - POST /api/conversations/{conversationId}/messages?userId={userId}  (body: { content: string })

If your backend changes or has additional routes, I can adapt the helper functions in `src/services/*` to match.

---

## Missing endpoints

- POST /api/conversations  — create a new direct conversation with a user (used by `src/pages/NewConversationPage.jsx` / `src/services/messagesService.js`)
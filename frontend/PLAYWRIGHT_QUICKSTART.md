# Playwright E2E Testing - Quick Start Guide

## 🚀 Quick Start (5 minutes)

### 1. Install Playwright

```bash
cd frontend
npm install
npx playwright install
```

### 2. Start Your App & Backend

**Terminal 1 - Frontend:**
```bash
cd frontend
npm run dev
```

**Terminal 2 - Backend:**
```bash
# In your backend directory
dotnet run
```

### 3. Run Tests

```bash
# In frontend directory
npm test                    # Run all tests
npm run test:ui           # Interactive mode (best for development)
npm run test:debug        # Debug mode
npm run test:report       # View last report
```

## 📝 Test Files Created

| File | Tests | Purpose |
|------|-------|---------|
| `auth.spec.js` | 5 | Login, register, logout flows |
| `navigation.spec.js` | 6 | Navigation and dashboard |
| `trips.spec.js` | 6 | Trip creation and search |
| `messaging.spec.js` | 6 | Messaging features |
| `responsive.spec.js` | 3 | Mobile and tablet views |
| **Total** | **26** | Full coverage of main features |

## ⚙️ Prerequisites

Before running tests, ensure:

1. ✅ **Backend is running** on the correct port
2. ✅ **Test user exists** in database:
   - Email: `testuser@example.com`
   - Password: `password123`
3. ✅ **Frontend runs on** `http://localhost:5173`
4. ✅ **Database is populated** with test data (or tests handle empty state)

## 🎯 Most Useful Commands

```bash
# Interactive test runner (RECOMMENDED for development)
npm run test:ui

# Run a specific test file
npx playwright test tests/e2e/auth.spec.js

# Run tests matching a pattern
npx playwright test --grep "login"

# Run with headed browser (see the test happening)
npx playwright test --headed

# Debug a specific test
npx playwright test tests/e2e/auth.spec.js --debug

# View the HTML report
npm run test:report
```

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| "Connection refused" | Start backend and frontend dev servers |
| "Login failed" | Create test user or update credentials in tests |
| "Element not found" | Use `npm run test:ui` to inspect elements |
| "Timeout" | Check if backend API is responding |
| "Port already in use" | Change port in vite.config.js or stop other services |

## 📚 Next Steps

1. **Update test user credentials** - Change `testuser@example.com` to match your test user
2. **Add more tests** - Follow the pattern in existing test files
3. **Set up CI/CD** - Create GitHub Actions workflow (see `tests/README.md`)
4. **Customize selectors** - Update selectors to match your HTML structure

## 📖 Resources

- [Playwright Documentation](https://playwright.dev)
- [Playwright Best Practices](https://playwright.dev/docs/best-practices)
- [Test Fixtures Guide](https://playwright.dev/docs/test-fixtures)
- [Debugging Guide](https://playwright.dev/docs/debug)

## 💡 Pro Tips

- Use `test:ui` mode while developing tests - it's much faster
- Take screenshots during tests: `await page.screenshot({ path: 'screenshot.png' })`
- Use `test.only()` to run a single test: `test.only('...', async ({...}) => {...})`
- Use `test.skip()` to skip a test temporarily
- Create `.env` file for environment variables if needed

---

Happy testing! 🎉

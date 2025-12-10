# Playwright E2E Tests

This directory contains end-to-end tests for the TravelBuddy frontend using Playwright.

## Setup

### 1. Install Dependencies

```bash
cd frontend
npm install
```

This will install Playwright and its dependencies.

### 2. Install Browsers

```bash
npx playwright install
```

This downloads the browser binaries needed for testing.

## Running Tests

### Run All Tests

```bash
npm test
```

### Run Tests in UI Mode (Recommended for Development)

```bash
npm run test:ui
```

This opens an interactive test runner where you can:
- Run individual tests
- Step through tests
- See live preview of the application
- Debug failing tests

### Run Tests in Debug Mode

```bash
npm run test:debug
```

This opens the Playwright Inspector for detailed debugging.

### View Test Report

After running tests, view the HTML report:

```bash
npm run test:report
```

### Run Specific Test File

```bash
npx playwright test tests/e2e/auth.spec.js
```

### Run Tests in Headed Mode (See Browser)

```bash
npx playwright test --headed
```

### Run Tests with Specific Browser

```bash
npx playwright test --project=chromium
npx playwright test --project=firefox
npx playwright test --project=webkit
```

## Test Structure

### Test Files

- **`auth.spec.js`** - Authentication and login flows
- **`navigation.spec.js`** - Navigation and dashboard
- **`trips.spec.js`** - Trip creation and searching
- **`messaging.spec.js`** - Messaging and conversations
- **`responsive.spec.js`** - Responsive design testing

### Fixtures

The `fixtures.js` file provides custom helper functions:

- `login(page, email, password)` - Helper to login a user
- `register(page, name, email, password)` - Helper to register new user
- `isAuthenticated(page)` - Check if user is authenticated
- `logout(page)` - Helper to logout

## Configuration

The `playwright.config.js` file contains:

- Base URL: `http://localhost:5173` (your dev server)
- Test directory: `tests/e2e`
- Browsers: Chromium, Firefox, WebKit
- Screenshots on failure
- Trace recording on first retry

## Requirements

### Before Running Tests

1. **Backend API must be running** on `http://localhost:5173` (or update baseURL in config)
2. **Test user credentials** - Make sure the test user exists:
   - Email: `testuser@example.com`
   - Password: `password123`
   - Or update credentials in test files

3. **Database** - Backend must have access to a database with test data

## Best Practices

1. **Use meaningful test names** - Describe what the test does
2. **Use `test.describe()`** - Group related tests together
3. **Use `test.beforeEach()`** - Setup common state before each test
4. **Use locators wisely** - Prefer visible text or IDs over complex selectors
5. **Handle async operations** - Use `waitForURL()`, `waitForNavigation()`, etc.
6. **Make tests independent** - Each test should be able to run alone

## Common Issues

### Tests Timeout
- Increase timeout: `await page.goto('/', { timeout: 10000 })`
- Check if backend is running
- Check network tab in report

### Element Not Found
- Use `test:ui` mode to inspect selectors
- Check if element is visible before interacting
- Use `.first()` when multiple elements match

### Authentication Issues
- Verify test user exists in database
- Check token storage in localStorage
- Ensure backend auth is working

## CI/CD Integration

For GitHub Actions, create `.github/workflows/e2e.yml`:

```yaml
name: E2E Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: '18'
      - run: npm install
        working-directory: frontend
      - run: npx playwright install --with-deps
        working-directory: frontend
      - run: npm test
        working-directory: frontend
```

## Debugging Tips

1. **Use `page.pause()`** to pause test execution at any point
2. **Check screenshots** - Saved on failure in `test-results/`
3. **View traces** - HTML traces of test execution
4. **Use console logs** - `console.log()` works in tests
5. **Inspect network** - Check API calls in trace viewer

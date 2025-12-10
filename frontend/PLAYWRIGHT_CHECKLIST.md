# ✅ Playwright E2E Setup Checklist

## Installation

- [ ] Navigated to `frontend` directory
- [ ] Ran `npm install` to install dependencies
- [ ] Ran `npx playwright install` to install browsers
- [ ] Verified Playwright installed: `npx playwright --version`

## Preparation

- [ ] Backend API is running (check correct port)
- [ ] Frontend dev server is running: `npm run dev`
- [ ] Test user exists in database:
  - Email: `testuser@example.com`
  - Password: `password123`
- [ ] Database has test data (or tests handle empty state)
- [ ] Updated `playwright.config.js` if backend port is different

## Running Tests

### First Run
- [ ] Ran `npm run test:ui` to see interactive test runner
- [ ] Saw the frontend load in test UI
- [ ] Can click through tests and see them execute

### All Tests
- [ ] Ran `npm test` to run all tests headless
- [ ] Checked test results
- [ ] Reviewed any failures

### Debug Mode
- [ ] Ran `npx playwright test --debug` 
- [ ] Used Playwright Inspector to step through a test
- [ ] Viewed console logs during execution

### View Report
- [ ] Ran `npm run test:report`
- [ ] Opened HTML report in browser
- [ ] Reviewed test results and traces

## Test Files Review

- [ ] Read `tests/README.md` for complete documentation
- [ ] Reviewed test structure in `tests/e2e/` directory
- [ ] Checked `tests/fixtures.js` for available helpers
- [ ] Understood different test categories

## Customization (if needed)

- [ ] Updated test user credentials if using different user
- [ ] Updated backend port in `playwright.config.js` if needed
- [ ] Added custom selectors matching your HTML structure
- [ ] Modified test data to match your database

## CI/CD Setup (Optional)

- [ ] Reviewed `.github/workflows/playwright.yml`
- [ ] Adjusted workflow for your backend setup if needed
- [ ] Tested workflow by pushing to GitHub
- [ ] Verified tests run in CI/CD pipeline
- [ ] Checked that test reports appear as artifacts

## Documentation

- [ ] Read `PLAYWRIGHT_QUICKSTART.md`
- [ ] Read `PLAYWRIGHT_COMPLETE_SETUP.md`
- [ ] Bookmarked `tests/README.md` for reference
- [ ] Understood how to run different test commands
- [ ] Know how to debug failing tests

## Advanced (Optional)

- [ ] Set up local `.env` file if needed
- [ ] Added custom test helpers to `tests/fixtures.js`
- [ ] Created additional test files for new features
- [ ] Configured video recording for tests
- [ ] Set up test result notifications

## Verification

- [ ] All 26 tests pass or show expected failures
- [ ] Can run tests multiple times with same results
- [ ] UI mode works smoothly
- [ ] HTML report generates successfully
- [ ] Can debug failing tests using available tools

## Documentation

- [ ] Shared `PLAYWRIGHT_COMPLETE_SETUP.md` with team
- [ ] Shared `PLAYWRIGHT_QUICKSTART.md` with team
- [ ] Team members can run `npm run test:ui` successfully
- [ ] Team knows how to debug failing tests
- [ ] Team understands test structure and can add new tests

## Ready to Go!

Once all checkboxes are ticked:

✅ Your E2E testing infrastructure is ready!

**Quick Commands:**
```bash
npm run dev              # Start frontend
npm run test:ui         # Interactive tests (BEST)
npm test                # Run all tests
npm run test:report     # View results
```

**Next Steps:**
1. Integrate tests into your development workflow
2. Run tests before merging PRs
3. Add CI/CD pipeline to GitHub
4. Expand tests as you add features
5. Keep tests updated with code changes

---

**Questions?** See:
- `tests/README.md` - Detailed guide
- `PLAYWRIGHT_QUICKSTART.md` - Quick reference
- `PLAYWRIGHT_COMPLETE_SETUP.md` - Full documentation

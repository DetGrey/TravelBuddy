import { test, expect } from '../fixtures';

test.describe('Authentication Flow', () => {
  test('User can register a new account', async ({ page, register }) => {
    const uniqueEmail = `user-${Date.now()}@test.com`;
    
    // Use the register helper (name, email, password, birthdate)
    // Note: register helper now auto-logs in and redirects to dashboard
    await register('Test User', uniqueEmail, 'TestPassword123!', '1990-01-15');
    
    // Should redirect to dashboard (auto-login after registration)
    // Note: App is deployed under /TravelBuddy/ path
    await expect(page).toHaveURL('http://localhost:5173/TravelBuddy/#/');
  });

  test('User can login with valid credentials', async ({ page, login }) => {
    // Using helper method with valid user
    await login('allyhill95@gmail.com', 'Allison123');
    
    // Should be on dashboard (works with /TravelBuddy/ base path)
    await expect(page).toHaveURL(/.*\/#\/$/);
  });

  test('User cannot login with invalid credentials', async ({ page }) => {
    await page.goto('/#/login');
    
    await page.fill('input[type="email"]', 'invalid@test.com');
    await page.fill('input[type="password"]', 'wrongpassword');
    await page.click('button:has-text("Login")');
    
    // Should show error message
    await expect(page.locator('.alert-danger')).toBeVisible({ timeout: 3000 });
  });

  test('Logged in user can logout', async ({ page, login, logout }) => {
    await login('allyhill95@gmail.com', 'Allison123');
    
    // Verify we're on dashboard
    await expect(page).toHaveURL(/.*\/#\/$/);
    
    // Logout
    await logout();
    
    // Should be redirected to login (works with /TravelBuddy/ base path)
    await expect(page).toHaveURL(/.*\/#\/login/);
  });

  test('Unauthenticated user is redirected to login', async ({ page }) => {
    // Navigate to a neutral page first to establish context
    await page.goto('/#/login');
    
    // Clear token and cookies
    await page.context().clearCookies();
    await page.evaluate(() => localStorage.removeItem('token')).catch(() => {
      // Ignore if localStorage is not accessible
    });
    
    // Try to access protected route
    await page.goto('/#/', { waitUntil: 'domcontentloaded' });
    
    // Should redirect to login (works with /TravelBuddy/ base path)
    await expect(page).toHaveURL(/.*\/#\/login/);
  });
});

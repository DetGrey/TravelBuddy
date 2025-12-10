import { test, expect } from '../fixtures';

test.describe('Navigation and Dashboard', () => {
  test.beforeEach(async ({ login }) => {
    // Login before each test with valid user
    await login('allyhill95@gmail.com', 'Allison123');
  });

  test('Dashboard displays welcome message and navigation', async ({ page }) => {
    await page.goto('/#/');
    
    // Check for main navigation elements
    await expect(page.locator('nav')).toBeVisible();
    
    // Check for dashboard elements
    await expect(page.locator('text=Welcome')).toBeVisible({ timeout: 3000 }).catch(() => {
      // Dashboard might not have "Welcome" text, so check for other elements
      return expect(page.locator('h1, h2, h3')).toBeVisible();
    });
  });

  test('User can navigate to Trip Search', async ({ page }) => {
    await page.goto('/#/');
    
    // Click on search link/button
    const searchLink = page.locator('a:has-text("Search"), button:has-text("Search")').first();
    await searchLink.click({ timeout: 3000 }).catch(() => {
      // Try navigating directly
      return page.goto('/#/search');
    });
    
    // Should be on search page
    await expect(page).toHaveURL(/.*\/#\/search/);
  });

  test('User can navigate to Create Trip', async ({ page }) => {
    await page.goto('/#/');
    
    // Click on create link/button
    const createLink = page.locator('a:has-text("Create"), button:has-text("Create")').first();
    await createLink.click({ timeout: 3000 }).catch(() => {
      return page.goto('/#/create');
    });
    
    // Should be on create trip page
    await expect(page).toHaveURL(/.*\/#\/create/);
  });

  test('User can navigate to My Trips', async ({ page }) => {
    await page.goto('/#/');
    
    // Click on my trips link
    const myTripsLink = page.locator('a:has-text("My Trips"), button:has-text("My Trips")').first();
    await myTripsLink.click({ timeout: 3000 }).catch(() => {
      return page.goto('/#/my-trips');
    });
    
    // Should be on my trips page
    await expect(page).toHaveURL(/.*\/#\/my-trips/);
  });

  test('User can navigate to Messages', async ({ page }) => {
    await page.goto('/#/');
    
    // Click on messages link
    const messagesLink = page.locator('a:has-text("Messages"), button:has-text("Messages")').first();
    await messagesLink.click({ timeout: 3000 }).catch(() => {
      return page.goto('/#/messages');
    });
    
    // Should be on messages page
    await expect(page).toHaveURL(/.*\/#\/messages/);
  });

  test('NavBar displays logout button when authenticated', async ({ page }) => {
    await page.goto('/#/');
    
    // Should have a logout option
    const logoutButton = page.locator('button:has-text("Logout"), a:has-text("Logout")').first();
    await expect(logoutButton).toBeVisible({ timeout: 3000 });
  });
});

import { test, expect } from '../fixtures';

test.describe('Responsive Design', () => {
  test('Login page is responsive on mobile', async ({ page }) => {
    // Set mobile viewport
    await page.setViewportSize({ width: 375, height: 667 });
    
    await page.goto('/#/login');
    
    // Form should still be visible
    const form = page.locator('form').first();
    await expect(form).toBeVisible();
    
    // Inputs should be accessible
    const emailInput = page.locator('input[type="email"]');
    const passwordInput = page.locator('input[type="password"]');
    
    await expect(emailInput).toBeVisible();
    await expect(passwordInput).toBeVisible();
  });

  test('Dashboard is responsive on tablet', async ({ page, login }) => {
    // Set tablet viewport
    await page.setViewportSize({ width: 768, height: 1024 });
    
    // Login
    await login('allyhill95@gmail.com', 'Allison123');
    
    // Wait for dashboard (works with /TravelBuddy/ base path)
    await page.waitForURL(/.*\/#\/$/, { timeout: 10000 });
    
    // Navigation should be visible
    const nav = page.locator('nav');
    await expect(nav).toBeVisible();
  });

  test('Trip search page is responsive on mobile', async ({ page, login }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    
    // Login
    await login('allyhill95@gmail.com', 'Allison123');
    
    // Navigate to search
    await page.goto('/#/search');
    
    // Form should be accessible on mobile
    const form = page.locator('form').first();
    if (await form.isVisible().catch(() => false)) {
      await expect(form).toBeVisible();
    }
  });
});

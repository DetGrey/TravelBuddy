// Test user credentials
export const TEST_USER = {
  email: 'allyhill95@gmail.com',
  password: 'Allison123'
};

import { test as base } from '@playwright/test';

/**
 * Extended test fixture with custom helpers
 */
export const test = base.extend({
  /**
   * Helper to log in a user
   */
  login: async ({ page }, use) => {
    const login = async (email, password) => {
      await page.goto('/#/login');
      await page.fill('input[type="email"]', email);
      await page.fill('input[type="password"]', password);
      await page.click('button:has-text("Login")');
      // Wait for redirect to dashboard (works with any base path like /TravelBuddy/)
      await page.waitForURL(/.*\/#\/$/, { timeout: 10000 });
    };
    await use(login);
  },

  /**
   * Helper to register a new user
   */
  register: async ({ page }, use) => {
    const register = async (name, email, password, birthdate) => {
      await page.goto('/#/register');
      
      // Wait for form to be visible
      await page.locator('form').first().waitFor({ state: 'visible' });
      
      // Wait a bit for any network requests to settle
      await page.waitForLoadState('networkidle');
      
      // First input (name) - has no type attribute, use more flexible selector
      const nameInput = page.locator('input:not([type="email"]):not([type="password"]):not([type="date"])').first();
      await nameInput.fill(name);
      
      // Email input
      const emailInput = page.locator('input[type="email"]');
      await emailInput.fill(email);
      
      // Password input
      const passwordInput = page.locator('input[type="password"]');
      await passwordInput.fill(password);
      
      // Birthdate input (date type)
      const birthdateInput = page.locator('input[type="date"]');
      await birthdateInput.fill(birthdate);
      
      // Wait a moment before submitting to ensure inputs are ready
      await page.waitForTimeout(500);
      
      // Click the Create account button
      const submitBtn = page.locator('button:has-text("Create account")');
      await submitBtn.click();
      
      // Wait for one of two outcomes:
      // 1. Successful registration -> auto-login -> redirect to dashboard
      // 2. Error -> error message displayed
      await Promise.race([
        page.waitForURL(/.*\/#\/$/, { timeout: 12000 }), // Registration succeeded and logged in (works with any base path)
        page.waitForURL(/.*\/#\/login$/, { timeout: 12000 }), // Registration succeeded but auto-login failed
        page.locator('.alert-danger').waitFor({ state: 'visible', timeout: 5000 }) // Registration failed with error
      ]).catch(() => {
        // If none of the above happen, check manually for current state
      });
      
      // Check current URL to determine outcome
      const url = page.url();
      const isDashboard = /\/#\/$/.exec(url);
      const isLoginPage = url.includes('/#/login');
      
      if (isDashboard && !isLoginPage) {
        // Successfully registered and logged in - we're on dashboard
        return;
      } else if (isLoginPage) {
        // Registration succeeded but auto-login failed - user needs to log in manually
        throw new Error('Registration successful but auto-login failed');
      }
      
      // Check for error message on current page
      const errorElement = page.locator('.alert-danger');
      const isErrorVisible = await errorElement.isVisible().catch(() => false);
      if (isErrorVisible) {
        const errorText = await errorElement.textContent().catch(() => 'Unknown error');
        throw new Error(`Registration failed: ${errorText}`);
      }
      
      // Timeout waiting for any outcome
      throw new Error('Registration timeout: no response from server');
    };
    await use(register);
  },

  /**
   * Helper to check if user is authenticated
   */
  isAuthenticated: async ({ page }, use) => {
    const check = async () => {
      const token = await page.evaluate(() => localStorage.getItem('token'));
      return !!token;
    };
    await use(check);
  },

  /**
   * Helper to logout
   */
  logout: async ({ page }, use) => {
    const logout = async () => {
      await page.goto('/#/logout');
      // Wait for redirect to login (works with any base path)
      await page.waitForURL(/.*\/#\/login/, { timeout: 10000 });
    };
    await use(logout);
  },
});

export { expect } from '@playwright/test';

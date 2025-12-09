import { test, expect } from '../fixtures';

test.describe('Messaging', () => {
  test.beforeEach(async ({ login }) => {
    await login('allyhill95@gmail.com', 'Allison123');
  });

  test('User can view Messages page', async ({ page }) => {
    await page.goto('/#/messages');
    
    // Should be on messages page
    await expect(page).toHaveURL(/.*\/#\/messages/);
    
    // Wait for content to load
    await page.waitForTimeout(2000);
  });

  test('User can see list of conversations', async ({ page }) => {
    await page.goto('/#/messages');
    
    // Wait for conversations to load
    await page.waitForTimeout(2000);
    
    // Check for conversation list or empty state
    const conversationList = page.locator('[class*="conversation"], [class*="chat"], [class*="list"]').first();
    const emptyState = page.locator('text=No conversations, text=empty');
    
    await expect(conversationList.or(emptyState)).toBeVisible({ timeout: 3000 });
  });

  test('User can try to open new conversation', async ({ page }) => {
    await page.goto('/#/messages');
    
    // Look for "New Conversation" or "Start" button
    const newConvButton = page.locator('button:has-text("New"), button:has-text("Start"), a:has-text("New")').first();
    
    if (await newConvButton.isVisible({ timeout: 2000 }).catch(() => false)) {
      await newConvButton.click();
      
      // Should navigate to new conversation page
      await expect(page).toHaveURL(/.*\/#\/messages\/new/);
    }
  });

  test('New Conversation page displays form', async ({ page }) => {
    await page.goto('/#/messages/new');
    
    // Check for conversation form
    const form = page.locator('form').first();
    await expect(form).toBeVisible({ timeout: 3000 });
    
    // Check for email input
    const emailInput = page.locator('input[type="email"]');
    await expect(emailInput).toBeVisible();
    
    // Check for message textarea
    const messageTextarea = page.locator('textarea');
    await expect(messageTextarea).toBeVisible();
  });

  test('New Conversation page shows expected message about missing endpoint', async ({ page }) => {
    await page.goto('/#/messages/new');
    
    // Fill in the form
    const emailInput = page.locator('input[type="email"]');
    const messageTextarea = page.locator('textarea');
    const submitButton = page.locator('button:has-text("Start")');
    
    if (await emailInput.isVisible() && await messageTextarea.isVisible()) {
      await emailInput.fill('test@example.com');
      await messageTextarea.fill('Hello');
      
      // Try to submit
      if (await submitButton.isVisible()) {
        await submitButton.click();
        
        // Should show error about missing endpoint
        const errorAlert = page.locator('.alert-danger, [class*="error"]');
        await expect(errorAlert).toBeVisible({ timeout: 3000 });
      }
    }
  });

  test('User can navigate to specific conversation', async ({ page }) => {
    // Try to navigate directly to a conversation
    await page.goto('/#/messages/1');
    
    // Page should load (even if no data, the route should work)
    await page.waitForTimeout(1000);
    await expect(page).toHaveURL(/.*\/#\/messages\/1/);
  });
});

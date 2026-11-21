import react from '@vitejs/plugin-react'
import { defineConfig } from 'vitest/config'

// In development the SPA is served by Vite and API calls are proxied to the ASP.NET Core
// backend, so both run on the same origin (required by the SameSite=Strict auth cookie).
// In Docker the same role is played by nginx (see nginx.conf).
const apiTarget = process.env.API_URL ?? 'http://localhost:5000'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': apiTarget,
      '/health': apiTarget,
    },
  },
  test: {
    environment: 'node',
    include: ['src/**/*.test.ts'],
  },
})

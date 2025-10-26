import tailwindcss from "@tailwindcss/vite";
import { version } from "./package.json";

export default defineNuxtConfig({
  ssr: false,
  compatibilityDate: "2025-07-15",
  devtools: { enabled: true },
  modules: ["@nuxt/eslint", "shadcn-nuxt", "@nuxtjs/color-mode"],
  css: ["~/assets/css/tailwind.css"],
  vite: {
    plugins: [tailwindcss()],
  },
  shadcn: {
    /**
     * Prefix for all the imported component
     */
    prefix: "",
    /**
     * Directory that the component lives in.
     * @default "./app/components/ui"
     */
    componentDir: "./app/components/ui",
  },
  colorMode: {
    preference: "dark", // default
    fallback: "light",
    classSuffix: "",
  },
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.NUXT_API_BASE_URL || "http://localhost:5000",
      appVersion: version,
    },
    app: {
      head: {
        link: [{ rel: "icon", type: "image/svg+xml", href: "/favicon.svg" }],
      },
    },
  },
});

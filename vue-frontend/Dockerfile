# Stage 1: Build Nuxt
FROM node:24-alpine AS build
ARG NUXT_API_BASE_URL
ENV NUXT_API_BASE_URL=${NUXT_API_BASE_URL:-/}
WORKDIR /app
COPY . .
RUN npm ci
RUN npm run generate

# Stage 2: Serve with Nginx
FROM nginx:mainline-alpine
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/.output/public /usr/share/nginx/html
EXPOSE 80

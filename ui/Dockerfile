FROM node:22.9-alpine3.20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build -- --configuration production

FROM nginx:alpine3.20
COPY --from=build /app/dist/ui/browser /usr/share/nginx/html
EXPOSE 80
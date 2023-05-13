### STAGE 1: Build ###
FROM node:18.12-alpine AS build
WORKDIR /usr/src/app
COPY . .
RUN npm install
ARG baseHref=/
RUN echo $baseHref
RUN npm run build -- --configuration production --base-href=$baseHref

### STAGE 2: Run ###
FROM nginx:1.23-alpine
## Remove default Nginx website
RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /usr/src/app/dist/member /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf

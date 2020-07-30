FROM node:12
RUN mkdir -p /app
WORKDIR /app
COPY VideoplayerApp/package.json VideoplayerApp/package-lock.json ./
RUN npm install
COPY VideoplayerApp/ .
EXPOSE 4200
CMD ["npm", "start"]

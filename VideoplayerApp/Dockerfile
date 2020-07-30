FROM node:12.8.2
RUN mkdir -p /app
WORKDIR /app
COPY /VideoplayerApp/package*.json app/
RUN npm install
COPY . app/
EXPOSE 4200
CMD ["npm", "start"]

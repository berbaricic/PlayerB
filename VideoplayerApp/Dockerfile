FROM node:12
RUN mkdir /app
WORKDIR /app
COPY VideoplayerApp/package*.json /app/angular-app/
RUN npm install --prefix angular-app
COPY . /app
EXPOSE 4200
CMD ["npm", "start"]

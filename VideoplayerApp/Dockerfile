FROM node:12
WORKDIR /helou
COPY VideoplayerApp/package*.json /helou/
RUN npm install
COPY . /helou
EXPOSE 4200
CMD ["npm", "start"]

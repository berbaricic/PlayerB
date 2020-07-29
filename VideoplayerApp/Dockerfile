FROM node:12
RUN mkdir -p /helou
WORKDIR /helou
COPY VideoplayerApp/package*.json /helou/
RUN npm install
COPY . /helou/
EXPOSE 4200
CMD ["npm", "start"]

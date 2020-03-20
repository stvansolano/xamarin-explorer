FROM node:lts-alpine

RUN apk update && apk upgrade && \
    apk add --no-cache bash git openssh

RUN git clone https://github.com/stvansolano/todomvc.git mvc

# install project dependencies
#RUN npm install -g @vue/cli
#RUN npm install -g @vue/cli-service
#RUN npm install -g @vue/cli-service-global

WORKDIR /mvc

RUN npm install -g nuxt
RUN npm install

# install simple http server for serving static content

RUN npm install -g http-server

#RUN npm run generate
RUN npm run build

# expose 5000 on container
EXPOSE 8000

# set app serving to permissive / assigned
ENV NUXT_HOST=0.0.0.0
# set app port
ENV NUXT_PORT=8000

#CMD [ "http-server", "dist" ]
CMD [ "npm", "run", "start" ]

# docker build --rm -f "WebFrontend/Dockerfile" -t todomvc-vuejs:latest "WebFrontend"
# docker run --rm -p 3000:3000/tcp todomvc-vuejs:latest
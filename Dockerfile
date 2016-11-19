# FROM debian:jessie

# MAINTAINER NGINX Docker Maintainers "docker-maint@nginx.com"

# ENV NGINX_VERSION 1.10.2-1~jessie

# RUN apt-key adv --keyserver hkp://pgp.mit.edu:80 --recv-keys 573BFD6B3D8FBC641079A6ABABF5BD827BD9BF62 \
# 	&& echo "deb http://nginx.org/packages/debian/ jessie nginx" >> /etc/apt/sources.list \
# 	&& apt-get update \
# 	&& apt-get install --no-install-recommends --no-install-suggests -y \
# 						ca-certificates \
# 						nginx=${NGINX_VERSION} \
# 						nginx-module-xslt \
# 						nginx-module-geoip \
# 						nginx-module-image-filter \
# 						nginx-module-perl \
# 						nginx-module-njs \
# 						gettext-base \
# 	&& rm -rf /var/lib/apt/lists/*

# # forward request and error logs to docker log collector
# RUN ln -sf /dev/stdout /var/log/nginx/access.log \
# 	&& ln -sf /dev/stderr /var/log/nginx/error.log

# EXPOSE 80 443

# CMD ["nginx", "-g", "daemon off;"]
# -----------------------------------------------------------------------------
# http://putaindecode.io/fr/articles/docker/dockerfile/
# https://www.digitalocean.com/community/tutorials/docker-explained-how-to-containerize-and-use-nginx-as-a-proxy
# https://www.digitalocean.com/community/tutorials/how-to-configure-the-nginx-web-server-on-a-virtual-private-server
FROM debian:jessie
MAINTAINER cornell@netcourrier.com
ENV NGINX_VERSION 1.10.1-1~jessie
ENV DEST_APROPOS home/apropos

# installation nginx
RUN apt-key adv --keyserver hkp://pgp.mit.edu:80 --recv-keys 573BFD6B3D8FBC641079A6ABABF5BD827BD9BF62 \ 
    && echo "deb http://nginx.org/packages/debian/ jessie nginx" >> /etc/apt/sources.list \
    && apt-get update \
    && apt-get install --no-install-recommends \
    nginx -y \
    vim -y \
    lynx -y
    
# install .net core
RUN apt-get install curl -y \ 
    libunwind8 -y \
    gettext -y \
    libicu52 -y \
    && curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?LinkID=827530 \
    # nettoie le gestionnaire de paquets afin que notre image soit un peu plus légère.
    && rm -rf /var/lib/apt/lists/* \

    && mkdir -p /opt/dotnet \
    && tar -zxf dotnet.tar.gz -C /opt/dotnet \
    && ln -s /opt/dotnet/dotnet /usr/local/bin

# Répertoires NGINX
RUN mkdir etc/nginx/sites-available \
    && mkdir etc/nginx/sites-enabled \
    && mkdir ${DEST_APROPOS}

COPY apropos.conf etc/nginx/conf.d
COPY src/Apropos.Web/bin/Release/netcoreapp1.0/publish ${DEST_APROPOS} 

WORKDIR ${DEST_APROPOS}

# ouvre les ports 80 (nginx default) et 81 (apropos) du container
EXPOSE 80 81

# désactive le démon en tant que directive globale
CMD ["nginx", "-g", "daemon off;"]

# -----------------------------------------------------------------------------
# contruit une image 'mynginx' en lisant le fichier 'dockerfile' du répertoire courant
# docker build -t mynginx .

# # construit un container 'mywebserver' à partir de l'image 'mynginx'
# # en liant le port '82'' du serveur local au port '80' du container
# # le paramètre '-t' permet d'avoir accès variable d'environnement comme 'clear'
# # le paramètre '-d' permet de récupérer la ligne de commande
#> docker run -td -p 82:80 -p 5000:81 --name mywebserver mynginx

# attache un terminal au container en mode interactif
#> docker exec -it mywebserver /bin/bash
# lance l'application
#> dotnet Apropos.Web.dll
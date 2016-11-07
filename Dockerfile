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
# et nettoie le gestionnaire de paquets afin que notre image soit un peu plus légère.
RUN apt-key adv --keyserver hkp://pgp.mit.edu:80 --recv-keys 573BFD6B3D8FBC641079A6ABABF5BD827BD9BF62 \ 
    && echo "deb http://nginx.org/packages/debian/ jessie nginx" >> /etc/apt/sources.list \
    && apt-get update \
    && apt-get install --no-install-recommends \
    nginx -y \
    vim -y \
    lynx -y \
    && rm -rf /var/lib/apt/lists/*

# install .net core
RUN apt-get update \
    && apt-get install -y curl \ 
    libunwind8 \
    gettext \
    libicu52 \
    && curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?LinkID=827530 \
    && rm -rf /var/lib/apt/lists/*

# RUN mkdir -p /usr/share/dotnet \
#     && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
#     && rm dotnet.tar.gz \
#     && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

RUN mkdir -p /opt/dotnet \
    && tar -zxf dotnet.tar.gz -C /opt/dotnet \
    && ln -s /opt/dotnet/dotnet /usr/local/bin

EXPOSE 80
EXPOSE 81

# Répertoires NGINX 
RUN mkdir etc/nginx/sites-available \
    && mkdir etc/nginx/sites-enabled

COPY apropos.conf etc/nginx/conf.d

RUN mkdir ${DEST_APROPOS}
COPY src/Apropos.Web/bin/release/netcoreapp1.0/publish\ ${DEST_APROPOS} 
WORKDIR ${DEST_APROPOS}

# désactive le démon en tant que directive globale
CMD ["nginx", "-g", "daemon off;"]

# -----------------------------------------------------------------------------
# contruit une image 'mynginx' en lisant le fichier 'dockerfile' du répertoire courant
# docker build -t mynginx .

# # construit un container 'mywebserver' à partir de l'image 'mynginx'
# # en liant le port '82'' du serveur local au port '80' du container
# # le paramètre '-t' permet d'avoir accès variable d'environnement comme 'clear'
# # le paramètre '-d' permet de récupérer la ligne de commande
#> docker run -td -p 82:80 --name mywebserver mynginx

# attache un terminal au container en mode interactif
#> docker exec -it mywebserver /bin/bash

#> mkdir etc/nginx/sites-available
#> mkdir etc/nginx/sites-enabled
#> mkdir -p /opt/dotnet && tar zxf dotnet.tar.gz -C /opt/dotnet
#> ln -s /opt/dotnet/dotnet /usr/local/bin

#> cd home
#> mkdir hwapp
#> cd hwapp
#> dotnet new && dotnet restore && dotnet run



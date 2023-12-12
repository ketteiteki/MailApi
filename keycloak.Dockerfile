FROM quay.io/keycloak/keycloak:22.0.5 as builder
COPY keycloakConfig/realm.json /opt/keycloak/data/import/
ENTRYPOINT /opt/keycloak/bin/kc.sh start-dev --import-realm
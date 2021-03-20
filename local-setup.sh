#!/bin/sh

echo "Docker login for neuralm.net"
read -p 'Username: ' username
read -sp 'Password: ' password
# Setup minikube
minikube start --cpus 6 --memory 8192 --embed-certs
sleep 5
minikube ssh -- sudo mkdir /mnt/data
minikube ssh -- sudo chown 10001:0 /mnt/data
# Setup kubectl and kwetter namespaces
kubectl apply -f ./K8s/kwetter-namespace.yaml
kubectl config set-context kwetter --namespace=kwetter --cluster=minikube --user=minikube
kubectl config use-context kwetter
# Setup docker registry
kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyC0m9l&xP@ssw0rd"
kubectl create secret docker-registry mydockercredentials --docker-server neuralm.net:7676 --docker-username $username --docker-password $password
sleep 5
# Install istio
istioctl install -y
sleep 3
kubectl label namespace kwetter istio-injection=enabled
# Install metallb
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.9.5/manifests/namespace.yaml
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.9.5/manifests/metallb.yaml
kubectl create secret generic -n metallb-system memberlist --from-literal=secretkey="$(openssl rand -base64 128)"
# Setup load balancer
kubectl apply -f ./K8s/MetalLB/kwetter-metal-loadbalancer-layer-2-config.yaml
kubectl apply -f ./K8s/Istio/kwetter-istio-gateway.yaml
kubectl apply -f ./K8s/kwetter-storage-class.yaml
kubectl apply -f ./K8s/kwetter-storage-persistent-volume.yaml
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-service.deployment.yaml
sleep 8
# Analyse with istio
istioctl analyze
# Sanity check
kubectl rollout restart deployment

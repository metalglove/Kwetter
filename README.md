# Kwetter
[![Solution code coverage](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml) [![KweetService](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml) [![FollowService](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml) [![UserService](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml) [![AuthorizationService](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=alert_status)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=coverage)](https://sonarcloud.io/component_measures?id=metalglove_Kwetter&metric=coverage&view=list) [![SonarCloud Security Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=security_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=ncloc)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter)


The Kwetter platform.
## Development environment setup
Install kubectl
```
sudo apt install kubectl
```

### Install minikube
Checkout the minikube install page https://minikube.sigs.k8s.io/docs/start/

Install minikube
```
curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
sudo install minikube-linux-amd64 /usr/local/bin/minikube
```

Copy root CA to `~/.minikube/certs/`.

Create minikube cluster:
```
minikube start --cpus 6 --memory 8192 --embed-certs
```
Prepare data directory for persistent volume
```
minikube ssh -- sudo mkdir /mnt/data
minikube ssh -- sudo chown 10001:0 /mnt/data
```
Create kwetter namespace for kubernetes
```
kubectl apply -f ./K8s/kwetter-namespace.yaml
```
Create a kwetter context for kubernetes
```
kubectl config set-context kwetter --namespace=kwetter --cluster=minikube --user=minikube
```
And let's also use that context from now on
```
kubectl config use-context kwetter
```
Add credentials to store
```
kubectl create secret docker-registry mydockercredentials --docker-server neuralm.net:7676 --docker-username <USERNAME> --docker-password <PASSWORD>
kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyC0m9l&xP@ssw0rd"
```
> **_NOTE:_** We might want to make a difference between development and production contexts in the same way.

### Install Istio
Download Istio from the Istio https://istio.io/latest/docs/setup/getting-started/#download
and add the `/bin` folder to the environment variables.
```
curl -L https://istio.io/downloadIstio | sh -
mv istio-1.9.1/ ~/
export PATH=~/istio-1.9.1/bin:$PATH
```
Afterwards, install Istio into the minikube cluster
```
istioctl install -y
```
Label the kwetter namespace with `istio-injection`
```
kubectl label namespace kwetter istio-injection=enabled
```

### Install MetalLB
Reference https://metallb.universe.tf/installation/
```
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.9.5/manifests/namespace.yaml
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.9.5/manifests/metallb.yaml
# On first install only
kubectl create secret generic -n metallb-system memberlist --from-literal=secretkey="$(openssl rand -base64 128)"
```
Then apply the MetalLB config
```
kubectl apply -f ./K8s/MetalLB/kwetter-metal-loadbalancer-layer-2-config.yaml
```

### Deploy the services
Let's start deploying those services!
Starting with the gateway, storage class and volume:
```
kubectl apply -f ./K8s --recursive
```

### Validate deployment
Let Istio validate the deployment
```
istioctl analyze
```
if there are any warnings or errors, restart the deployment
```
kubectl rollout restart deployment
```
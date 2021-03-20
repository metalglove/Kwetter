# Kwetter
The Kwetter platform.
## Development environment setup
Tools required....

### Install minikube
https://minikube.sigs.k8s.io/docs/start/
Add binaries to path (kubectl.exe)

Copy root CA to `~/.minikube/certs/`.

Add credentials to store
```
kubectl create secret docker-registry mydockercredentials --docker-server <DOCKER-REGSITRY>:<PORT> --docker-username <USERNAME> --docker-password <PASSWORD>
kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyC0m9l&xP@ssw0rd"
```
Create minikube cluster:
```
minikube start --cpus 6 --memory 8192 --embed-certs
```
> **_NOTE:_** If there is an existing minikube cluster and it can't be changed, we have to first delete the cluster using `minikube delete` and then try running the above command again.

Prepare data directory for persistent volume
```
minikube ssh
sudo mkdir /mnt/data
sudo chown 10001:0 /mnt/data
```
Create kwetter namespace for kubernetes
```
kubectl apply -f .\kwetter-namespace.yaml
```
Create a kwetter context for kubernetes
```
kubectl config set-context kwetter --namespace=kwetter --cluster=minikube --user=minikube
```
And let's also use that context from now on
```
kubectl config use-context kwetter
```
> **_NOTE:_** We might want to make a difference between development and production contexts in the same way.

### Install Istio
Download Istio from the Istio website https://istio.io/latest/docs/setup/getting-started/#download
Add the `/bin` folder to the environment variables.
Afterwards, install Istio into the minikube cluster
```
istioctl install -y
```
Label the namespace with `istio-injection`
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
kubectl apply -f .\MetalLB\kwetter-metal-loadbalancer-layer-2-config.yaml
```
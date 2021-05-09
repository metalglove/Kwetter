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
minikube start --cpus 12 --memory 18192 --embed-certs
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
```
Apply more secrets.
```
kubectl apply -f ./K8s/secrets/
```

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

### Prepare storage volumes
Prepare data directory for persistent volumes
```
minikube ssh -- sudo mkdir /mnt/data
minikube ssh -- sudo mkdir /mnt/data/kwetter-authorization-db
minikube ssh -- sudo mkdir /mnt/data/kwetter-follow-db
minikube ssh -- sudo mkdir /mnt/data/kwetter-user-db
minikube ssh -- sudo mkdir /mnt/data/kwetter-kweet-db
minikube ssh -- sudo mkdir /mnt/data/rabbit-store
minikube ssh -- sudo mkdir /mnt/data/rabbit-store-1
minikube ssh -- sudo mkdir /mnt/data/rabbit-store-2
minikube ssh -- sudo mkdir /mnt/data/rabbit-store-3
minikube ssh -- sudo mkdir /mnt/data/eventstore
minikube ssh -- sudo mkdir /mnt/data/neo4j
minikube ssh -- sudo chown -R 10001:0 /mnt/data
```

Install the kwetter storage class
```
kubectl apply -f ./K8s/kwetter-storage-class.yaml
```
Setup the volume and volume claims
```
kubectl apply -f ./K8s/Minikube
```

### Install rabbitmq
Install rabbitmq
```
kubectl apply -f ./K8s/rabbitmq/rabbit-rbac.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-configmap.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-statefulset.yaml
```

### Install EventStore
```
kubectl apply -f ./K8s/EventStore/eventstore.deployment.yaml
```

### Install neo4j
```
kubectl apply -f ./K8s/neo4j/neo4j.deployment.yaml
```

### Deploy services
First, we have to start up the isto gateway
```
kubectl apply -f ./K8s/Istio/kwetter-istio-gateway.yaml
```

Spin up user service!
```
kubectl apply -f ./K8s/services/user-service/kwetter-user-db.deployment.yaml
kubectl apply -f ./K8s/services/user-service/kwetter-user-service.deployment.yaml
```

Spin up follow service!
```
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-db.deployment.yaml
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-service.deployment.yaml
```

Spin up kweet service!
```
kubectl apply -f ./K8s/services/kweet-service/kwetter-kweet-db.deployment.yaml
kubectl apply -f ./K8s/services/kweet-service/kwetter-kweet-service.deployment.yaml
```

Spin up authorization service!
```
kubectl apply -f ./K8s/services/authorization-service/kwetter-authorization-db.deployment.yaml
kubectl apply -f ./K8s/services/authorization-service/kwetter-authorization-service.deployment.yaml
```

Spin up timeline service!
```
kubectl apply -f ./K8s/services/timeline-service/kwetter-timeline-service.deployment.yaml
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

Also, if the kiali dashboard is required, navigate to the istioctl directory
```
kubectl apply -f ./samples/addons
```
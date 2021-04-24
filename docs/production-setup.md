# Production Kubernetes installation
VM specs: 6 cpu cores, 8192 MB ram, SWAP disabled

## Installation
Installation guide from https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/install-kubeadm/
```
cat <<EOF | sudo tee /etc/modules-load.d/k8s.conf
br_netfilter
EOF

cat <<EOF | sudo tee /etc/sysctl.d/k8s.conf
net.bridge.bridge-nf-call-ip6tables = 1
net.bridge.bridge-nf-call-iptables = 1
EOF
sudo sysctl --system
```

Install a container runtime.
I used Docker https://docs.docker.com/engine/install/ubuntu/.
```
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
```
Install docker engine
```
sudo apt-get update
sudo apt-get install docker-ce docker-ce-cli containerd.io
```

Install needed libraries
```
sudo apt-get update
sudo apt-get install -y apt-transport-https ca-certificates curl
```
Add gpg key
```
sudo curl -fsSLo /usr/share/keyrings/kubernetes-archive-keyring.gpg https://packages.cloud.google.com/apt/doc/apt-key.gpg
echo "deb [signed-by=/usr/share/keyrings/kubernetes-archive-keyring.gpg] https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee /etc/apt/sources.list.d/kubernetes.list
```
Install kubeadm, kubelet and kubectl
```
sudo apt-get update
sudo apt-get install -y kubelet kubeadm kubectl
sudo apt-mark hold kubelet kubeadm kubectl
```

Initialize Kubernetes cluster
```
kubeadm init --pod-network-cidr=192.168.0.0/16
```

kubectl for non-root user
```
mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config
```
Install calico https://docs.projectcalico.org/getting-started/kubernetes/quickstart
```
kubectl create -f https://docs.projectcalico.org/manifests/tigera-operator.yaml
kubectl create -f https://docs.projectcalico.org/manifests/custom-resources.yaml
```
Remove taints on master
```
kubectl taint nodes --all node-role.kubernetes.io/master-
```

Add insecure registry to the `/etc/docker/daemon.json`.
Only work around for private registry, can't seem to add certificate to kubernetes...


Create kwetter namespace for kubernetes
```
kubectl apply -f ./K8s/kwetter-namespace.yaml
```
Create a kwetter context for kubernetes
```
kubectl config set-context kwetter --namespace=kwetter --cluster=kubernetes --user=kubernetes-admin
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

Download Istio from the Istio https://istio.io/latest/docs/setup/getting-started/#download
and add the `/bin` folder to the environment variables.
```
curl -L https://istio.io/downloadIstio | sh -
mv istio-1.9.1/ ~/
export PATH=~/istio-1.9.1/bin:$PATH
```
Afterwards, install Istio into the cluster
```
istioctl install --set values.gateways.istio-egressgateway.enabled=true \
--set meshConfig.outboundTrafficPolicy.mode=ALLOW_ANY \
--set components.egressGateways[0].name=istio-egressgateway \
--set components.egressGateways[0].enabled=true
```
Label the kwetter namespace with `istio-injection`
```
kubectl label namespace kwetter istio-injection=enabled
```

To ensure that the egress gateway works properly on baremetal clusters, sometimes it is necesarry to configure the CoreDNS <br/>
See: https://crt.the-mori.com/2020-03-18-coredns-connection-timeout-external-domain-name <br/>
Store the ConfigMap of the coredns into a yaml file.
```
kubectl get configmap coredns -n kube-system -o yaml > coredns.yaml
```
Change the forward block: forward . /etc/resolv.conf (including the { ... }) to forward .  8.8.8.8 8.8.4.4 (be aware of 2 spaces after .) 
```
vim coredns.yaml
```
Deploy the new ConfigMap
```
kubectl apply -f coredns.yaml 
```
Redploy the coredns pods by deleting them
```
kubectl rollout restart -n kube-system deployment/coredns
```

Install MetalLB
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

Create all local volume directories
```
mkdir /mnt/data/kwetter-authorization-db
mkdir /mnt/data/kwetter-follow-db
mkdir /mnt/data/kwetter-user-db
mkdir /mnt/data/kwetter-kweet-db
mkdir /mnt/data/rabbit-store
mkdir /mnt/data/rabbit-store-1
mkdir /mnt/data/rabbit-store-2
mkdir /mnt/data/rabbit-store-3
mkdir /mnt/data/eventstore
chown -R 10001:0 /mnt/data
```

Install the kwetter storage class
```
kubectl apply -f ./K8s/kwetter-storage-class.yaml
```

Install rabbitmq
```
kubectl apply -f ./K8s/rabbitmq/rabbit-pv.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-pv-1.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-pv-2.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-pv-3.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-rbac.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-configmap.yaml
kubectl apply -f ./K8s/rabbitmq/rabbit-statefulset.yaml
```

Install EventStore
```
kubectl apply -f ./K8s/EventStore/eventstore-storage-persistent-volume-and-claim.yaml
kubectl apply -f ./K8s/EventStore/eventstore.deployment.yaml
```

Deploy the services
Let's start deploying those services!
Starting with the gateway:
```
kubectl apply -f ./K8s/Istio/kwetter-istio-gateway.yaml
```

Spin up user service!
```
kubectl apply -f ./K8s/services/user-service/kwetter-user-storage-persistent-volume-and-claim.yaml
kubectl apply -f ./K8s/services/user-service/kwetter-user-db.deployment.yaml
kubectl apply -f ./K8s/services/user-service/kwetter-user-service.deployment.yaml
```

Spin up follow service!
```
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-storage-persistent-volume-and-claim.yaml
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-db.deployment.yaml
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-service.deployment.yaml
```

Spin up kweet service!
```
kubectl apply -f ./K8s/services/kweet-service/kwetter-kweet-storage-persistent-volume-and-claim.yaml
kubectl apply -f ./K8s/services/kweet-service/kwetter-kweet-db.deployment.yaml
kubectl apply -f ./K8s/services/kweet-service/kwetter-kweet-service.deployment.yaml
```

Spin up authorization service!
```
kubectl apply -f ./K8s/services/authorization-service/kwetter-authorization-storage-persistent-volume-and-claim.yaml
kubectl apply -f ./K8s/services/authorization-service/kwetter-authorization-db.deployment.yaml
kubectl apply -f ./K8s/services/authorization-service/kwetter-authorization-service.deployment.yaml
```

Validate deployment
Let Istio validate the deployment
```
istioctl analyze
```
if there are any warnings or errors, restart the deployment
```
kubectl rollout restart deployment
```

Validate rights for volumes
https://blog.dbi-services.com/using-non-root-sql-server-containers-on-docker-and-k8s/
Should be properly setup.
Quick and dirty fix is to chmod 777. (DON'T DO THIS IN PRODUCTION!)

Useful commands
Port forwards the rabbitmq management ui.
```
kubectl -n kwetter port-forward rabbitmq-0 8080:15672 --address 192.168.1.136
```

Port forwards the eventstore
```
kubectl port-forward service/eventstore 2113:2113 --address 192.168.1.136
```

Port forwards the kiali ui.
```
istioctl dashboard kiali --address 192.168.1.136
```

> **_NOTE:_**  When using baremetal, don't forget to portforward the ingress-gateway!
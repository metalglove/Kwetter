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
kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyC0m9l&xP@ssw0rd"
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
istioctl install -y
```
Label the kwetter namespace with `istio-injection`
```
kubectl label namespace kwetter istio-injection=enabled
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

Deploy the services
Let's start deploying those services!
Starting with the gateway, storage class and volume:
```
kubectl apply -f ./K8s/Istio/kwetter-istio-gateway.yaml
kubectl apply -f ./K8s/kwetter-storage-class.yaml
kubectl apply -f ./K8s/kwetter-storage-persistent-volume.yaml
```
The follow service:
```
kubectl apply -f ./K8s/services/follow-service/kwetter-follow-service.deployment.yaml
```
The user service:
``` 
kubectl apply -f ./K8s/services/user-service/kwetter-user-db-persistent-volume-claim.yaml
kubectl apply -f ./K8s/services/user-service/kwetter-user-db.deployment.yaml
kubectl apply -f ./K8s/services/user-service/kwetter-user-service.deployment.yaml
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
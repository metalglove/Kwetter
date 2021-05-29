# Testing with K6
## Installation
Following: https://k6.io/blog/running-distributed-tests-on-k8s/

Install make
```
apt install make
```

Install go
```
apt install golang-go
```

Install Kustomize
```
cd /root/go/bin/
curl -s "https://raw.githubusercontent.com/kubernetes-sigs/kustomize/master/hack/install_kustomize.sh" | bash
```

Download the K6 operator
```
git clone https://github.com/k6io/operator && cd operator
```

Deploy the operator
```
make deploy
```

## Setting up the tests
Tests in K6 are based on javascript files embedded in configmaps.
```
kubectl create namespace kwetter-testing
kubectl create configmap -n kwetter-testing verify-username-uniqueness-load-test-configmap --from-file src/Testing/Kwetter.LoadTests/test.js --dry-run=client -o yaml | kubectl apply -n kwetter-testing -f -
```

## Run the tests
Tests are run by applying the custom resource definition.
```
kubectl apply -n kwetter-testing -f ./K8s/k6/verify-username-uniqueness-load-test.yaml
```

Check if the test is running
```
kubectl get k6
```

Get the logs from the container
```
kubectl logs -n kwetter-testing -l k6_cr=verify-username-uniqueness-load-test
```

Delete the resources
```
kubectl delete -f ./K8s/k6/verify-username-uniqueness-load-test.yaml
```
# Uninstalling K6 operator
Delete the operator
```
make delete
```
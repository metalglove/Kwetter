build
```
docker build -t neuralm.net:7676/kwetter-user-service:latest -f ./kwetter-user-service.Dockerfile .
```
publish
```
docker push neuralm.net:7676/kwetter-user-service:latest
```
kiali
```
istioctl dashboard kiali
```
minikube useful commands
```
minikube dashboard
minikube ip
minikube service <SERVICE-NAME>
minikube service list
```
get the gateway ip
```
export INGRESS_PORT=$(kubectl -n istio-system get service istio-ingressgateway -o jsonpath='{.spec.ports[?(@.name=="http2")].nodePort}')
export INGRESS_HOST=$(minikube ip)
export GATEWAY_URL=$INGRESS_HOST:$INGRESS_PORT
echo "$GATEWAY_URL"
```
example
```
echo "http://$GATEWAY_URL/api/Follow"
```
for the lazy
```
gate() {
export INGRESS_PORT=$(kubectl -n istio-system get service istio-ingressgateway -o jsonpath='{.spec.ports[?(@.name=="http2")].nodePort}')
export INGRESS_HOST=$(minikube ip)
export GATEWAY_URL=$INGRESS_HOST:$INGRESS_PORT
echo "http://$GATEWAY_URL/api/Follow"
}
```
For windows, the above commands might not work.
A workaround is to expose the istio-ingressgateway using the `minikube service` command.
```
minikube service -n istio-system istio-ingressgateway --url
```
Then the second url is the one for http.

For transfering the secrets
```
scp -r secrets glovali@kmaster:secrets
```

For checking the available resources in the nodes
```
kubectl describe nodes
```

For scavenging the event store
```
curl -i -d {} -X POST http://localhost:2113/admin/scavenge -u "admin:changeit"
```

Docker run for eventstore
```
docker run --name esdb-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore:21.2.0-bionic --insecure --run-projections=All --enable-external-tcp --enable-atom-pub-over-http
```

if persistent volumes get stuck
```
kubectl patch pv pvname -p '{"metadata":{"finalizers":null}}'
```
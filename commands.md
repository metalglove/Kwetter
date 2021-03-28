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
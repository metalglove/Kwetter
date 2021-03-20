build
```
docker build -t 192.168.1.99:7676/kwetter-user-service:v1 -f ./kwetter-user-service.Dockerfile .
```
publish
```
docker push 192.168.1.99:7676/kwetter-user-service:v1
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
build
```
PS C:\Users\mario\source\repos\Kwetter\src> docker build -t 192.168.1.99:7676/kwetter-user-service:v1 -f .\kwetter-user-service.Dockerfile .
```
publish
```
PS C:\Users\mario\source\repos\Kwetter\src> docker push 192.168.1.99:7676/kwetter-user-service:v1
```
kiali
```
kubectl port-forward svc/kiali -n istio-system 20001
```
minikube useful commands
```
minikube dashboard
minikube ip
minikube service <SERVICE-NAME>
minikube service list
```

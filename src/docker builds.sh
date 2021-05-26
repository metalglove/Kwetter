docker build --tag neuralm.net:7676/kwetter-timeline-service:latest -f ./kwetter-timeline-service.Dockerfile .
docker push neuralm.net:7676/kwetter-timeline-service:latest

docker build --tag neuralm.net:7676/kwetter-notification-service:latest -f ./kwetter-notification-service.Dockerfile .
docker push neuralm.net:7676/kwetter-notification-service:latest

docker build --tag neuralm.net:7676/kwetter-user-service:latest -f ./kwetter-user-service.Dockerfile .
docker push neuralm.net:7676/kwetter-user-service:latest

docker build --tag neuralm.net:7676/kwetter-kweet-service:latest -f ./kwetter-kweet-service.Dockerfile .
docker push neuralm.net:7676/kwetter-kweet-service:latest

docker build --tag neuralm.net:7676/kwetter-follow-service:latest -f ./kwetter-follow-service.Dockerfile .
docker push neuralm.net:7676/kwetter-follow-service:latest

docker build --tag neuralm.net:7676/kwetter-authorization-service:latest -f ./kwetter-authorization-service.Dockerfile .
docker push neuralm.net:7676/kwetter-authorization-service:latest

docker build --tag neuralm.net:7676/kwetter-frontend:latest -f ./kwetter-frontend.Dockerfile . 
docker push neuralm.net:7676/kwetter-frontend:latest
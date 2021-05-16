# Kwetter
[![Solution code coverage](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml) [![KweetService](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml) [![FollowService](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml) [![UserService](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml) [![AuthorizationService](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=alert_status)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=coverage)](https://sonarcloud.io/component_measures?id=metalglove_Kwetter&metric=coverage&view=list) [![SonarCloud Security Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=security_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=ncloc)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter)

Kwetter is a social platform for posting kweets, following friends and keeping up with trending topics.

## 1. Goals
This monorepository aims to implement a Microservice architecture using Domain-Driven Design in .NET 5.
The goal is to learn about enterprise software development; scalable architectures, data distribution, messaging patterns and DevOps.

## 2. Scalable Architecture
![Kwetter platform](/docs/diagrams/kwetter%20platform.png)

### 2.1 Messaging Patterns
![Kwetter messaging](/docs/diagrams/kwetter%20messaging.png)
![Kwetter notification](/docs/diagrams/kwetter%20notification.png)

### 2.2 Data Distribution
UserService, KweetService, AuthorizationService, FollowService

![Relational databases](/docs/diagrams/kwetter%20relational%20databases.png)

TimelineService

![Timeline graph](/docs/diagrams/timeline%20graph.png)

NotificationService

![NotificationService KeyValueStore](/docs/diagrams/kwetter%20keyvalue%20stores.png)

## 3. DevOps
![Kwetter pipeline](/docs/diagrams/kwetter%20service%20pipeline.png)

# Conclusion
soonTM
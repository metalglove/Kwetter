# Kwetter
[![Solution code coverage](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/solution-code-coverage.yml) [![KweetService](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/kweet-service.yml) [![FollowService](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/follow-service.yml) [![UserService](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/user-service.yml) [![Frontend](https://github.com/metalglove/Kwetter/actions/workflows/frontend.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/frontend.yml) [![AuthorizationService](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/authorization-service.yml) 
[![TimelineService](https://github.com/metalglove/Kwetter/actions/workflows/timeline-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/timeline-service.yml) [![NotificationService](https://github.com/metalglove/Kwetter/actions/workflows/notification-service.yml/badge.svg)](https://github.com/metalglove/Kwetter/actions/workflows/notification-service.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=alert_status)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=coverage)](https://sonarcloud.io/component_measures?id=metalglove_Kwetter&metric=coverage&view=list) [![SonarCloud Security Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=security_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![SonarCloud Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=metalglove_Kwetter&metric=ncloc)](https://sonarcloud.io/dashboard?id=metalglove_Kwetter)

> **An enterprise-grade microservices platform demonstrating Domain-Driven Design, event-driven architecture, and modern DevOps practices.**
> 
> *Built during Fontys University 6th semester (2021) as an educational project - achieved through extensive online research, architectural study, and dedicated coding - **before generative AI tools were available**.*

Kwetter is a Twitter-like social platform for posting kweets, following friends, and keeping up with trending topics. Built with .NET 5, it showcases production-ready microservices architecture with comprehensive testing, event sourcing, and polyglot persistence.

📖 **[Read the Complete Architecture Review](docs/kwetter-architecture-review.md)** - Comprehensive analysis of design decisions, code quality, and recommendations.

## Overview

**Overall Architecture Grade: A (Excellent)**

Kwetter implements a true microservices architecture with:
- ✅ 7 independent services with clear bounded contexts
- ✅ Event-driven communication via RabbitMQ
- ✅ CQRS pattern with MediatR throughout
- ✅ Event sourcing with EventStoreDB
- ✅ Polyglot persistence (PostgreSQL, Neo4j, Redis)
- ✅ Production-ready Kubernetes deployments
- ✅ Comprehensive test coverage (unit, integration, domain)

## 1. Goals

This monorepository was developed as a 6th semester project at **Fontys University of Applied Sciences** (2021) to demonstrate mastery of enterprise software development.

### Project Context
- **Timeline**: Completed within one semester (~4 months)
- **Development Era**: Built **before generative AI tools** (ChatGPT, Copilot) were available
- **Learning Approach**: Self-taught through online research, documentation, books, and architectural studies
- **Scope**: Full-stack implementation from architecture design to deployment

### Learning Objectives
The project aims to demonstrate deep understanding of:
- ✅ **Microservice Architecture**: Building distributed systems with clear bounded contexts
- ✅ **Domain-Driven Design**: Implementing DDD tactical and strategic patterns
- ✅ **Event-Driven Architecture**: Asynchronous communication and event sourcing
- ✅ **Scalability**: Designing for horizontal scaling and high availability
- ✅ **Data Distribution**: Polyglot persistence and data consistency patterns
- ✅ **Messaging Patterns**: Event-driven communication with RabbitMQ
- ✅ **DevOps Practices**: CI/CD pipelines, containerization, and Kubernetes orchestration
- ✅ **Testing**: Comprehensive testing strategies (unit, integration, domain)
- ✅ **Security**: Authentication, authorization, and secure communication

### Achievement
The project received **Grade A (Excellent)** in an independent architecture review, validating its production-ready quality and demonstrating that enterprise-grade software can be built through dedication, research, and systematic learning.

## Key Features

### Architecture & Design
- **Domain-Driven Design (DDD)**: Rich domain models with aggregates, value objects, and domain events
- **CQRS**: Command Query Responsibility Segregation with MediatR pipeline
- **Event Sourcing**: Full audit trail with EventStoreDB
- **Event-Driven**: Asynchronous communication between services via integration events

### Technology Stack
- **Backend**: .NET 5, ASP.NET Core, Entity Framework Core
- **Frontend**: Vue.js 3 with TypeScript and Vuex
- **Messaging**: RabbitMQ with dead-letter queues
- **Databases**: PostgreSQL (relational), Neo4j (graph), Redis (cache/state)
- **Event Store**: EventStoreDB for event sourcing
- **Container Orchestration**: Kubernetes with Istio service mesh
- **CI/CD**: GitHub Actions with SonarCloud integration

### Services

| Service | Purpose | Grade | Tech Stack |
|---------|---------|-------|------------|
| **KweetService** | Manages kweets, likes, mentions, hashtags | A+ | PostgreSQL, EventStoreDB |
| **UserService** | User profiles and profile management | A | PostgreSQL, EventStoreDB |
| **FollowService** | Follow/unfollow relationships | A | PostgreSQL, EventStoreDB |
| **AuthorizationService** | Authentication & authorization (JWT, OAuth) | A+ | PostgreSQL, Google OAuth |
| **TimelineService** | Personalized timelines with graph queries | A+ | Neo4j graph database |
| **NotificationService** | Real-time WebSocket notifications | A- | Redis, WebSockets |
| **Frontend** | Vue.js SPA with real-time updates | A- | Vue.js, TypeScript, Vuex |

## 2. Scalable Architecture

### Architecture Highlights
- **Microservices**: 7 independent services with clear bounded contexts
- **Event-Driven**: RabbitMQ for asynchronous inter-service communication
- **Polyglot Persistence**: Right database for each use case
- **API Gateway**: Istio service mesh for routing and load balancing
- **Observability**: Structured logging, health checks, and CI/CD integration

![Kwetter platform](/docs/diagrams/kwetter%20platform.png)

### 2.1 Messaging Patterns

The system uses RabbitMQ with:
- **Topic Exchanges**: Flexible routing of integration events
- **Dead Letter Queues**: Automatic handling of failed messages
- **Channel Pooling**: Optimized connection management
- **Event Sourcing**: EventStoreDB for complete audit trail

![Kwetter messaging](/docs/diagrams/kwetter%20messaging.png)
![Kwetter notification](/docs/diagrams/kwetter%20notification.png)

### 2.2 Data Distribution

**Polyglot Persistence Strategy:**
- **PostgreSQL**: Transactional data (UserService, KweetService, AuthorizationService, FollowService)
- **Neo4j**: Social graph queries for timelines (TimelineService)
- **Redis**: WebSocket connection state and caching (NotificationService)
- **EventStoreDB**: Domain events for all services

#### UserService, KweetService, AuthorizationService, FollowService

![Relational databases](/docs/diagrams/kwetter%20relational%20databases.png)

#### TimelineService (Neo4j Graph Database)

The TimelineService uses Neo4j for efficient graph queries, combining:
- User's own kweets
- Kweets from followed users
- Kweets where user is mentioned

![Timeline graph](/docs/diagrams/kwetter%20timeline%20frontend.png)
![Timeline graph](/docs/diagrams/timeline%20graph.png)
![Timeline graph](/docs/diagrams/kwetter%20timeline%20tags.png)

#### NotificationService (Redis)

Real-time notifications via WebSockets with Redis-backed state management:

![NotificationService example](/docs/diagrams/kwetter%20notifications.png)
![NotificationService KeyValueStore](/docs/diagrams/kwetter%20keyvalue%20stores.png)

## 3. DevOps & CI/CD

### Continuous Integration & Deployment
- **GitHub Actions**: Automated builds and tests per service
- **SonarCloud**: Code quality, security, and coverage analysis
- **Docker**: Multi-stage builds for optimized images
- **Kubernetes**: Production-ready deployments with rolling updates
- **Istio**: Service mesh for traffic management and observability

![Kwetter pipeline](/docs/diagrams/kwetter%20service%20pipeline.png)

### Infrastructure Features
- ✅ **Kubernetes Deployments**: Rolling updates with health checks
- ✅ **StatefulSets**: For RabbitMQ clustering and persistent storage
- ✅ **HorizontalPodAutoscaler**: Automatic scaling based on load
- ✅ **Persistent Volumes**: Data persistence across pod restarts
- ✅ **Secrets Management**: Secure credential handling
- ✅ **MetalLB**: Load balancer for on-premise deployments
- ✅ **Istio Gateway**: API gateway with traffic routing

## 4. Code Quality & Testing

### Testing Strategy
- **Unit Tests**: Domain logic and business rules (MSTest)
- **Integration Tests**: API endpoints and database operations
- **Command Tests**: CQRS command handlers with MediatR
- **Query Tests**: Read operations and projections
- **Domain Event Tests**: Event handler verification

### Quality Metrics
- **SonarCloud Quality Gate**: ✅ Passing
- **Code Coverage**: High coverage across all services
- **Security Rating**: A rating on SonarCloud
- **Maintainability**: A rating on SonarCloud
- **Reliability**: A rating on SonarCloud

## 5. Getting Started

### Prerequisites
- .NET 5 SDK
- Docker & Docker Compose
- Kubernetes (Minikube or cloud provider)
- Node.js (for frontend)

### Local Development Setup
```bash
# Clone the repository
git clone https://github.com/metalglove/Kwetter.git
cd Kwetter

# Run local setup script
./local-setup.sh

# Build and run services
cd src
./docker builds.sh
```

### Documentation
- 📖 [Complete Architecture Review](docs/kwetter-architecture-review.md) - In-depth analysis and recommendations
- 📖 [Minikube Setup Guide](docs/minikube-setup.md) - Local Kubernetes deployment
- 📖 [Production Setup Guide](docs/production-setup.md) - Production deployment guide
- 📖 [Testing Guide](docs/testing-setup.md) - Running tests and load testing

## 6. Architecture Review Highlights

From the [comprehensive architecture review](docs/kwetter-architecture-review.md):

### Strengths ✅
1. **Architectural Excellence**: True microservices with DDD
2. **Event-Driven Design**: Proper event sourcing and integration events
3. **Code Quality**: Clean, maintainable, SOLID principles
4. **Testing**: Comprehensive coverage (unit, integration, domain)
5. **Infrastructure**: Production-ready Kubernetes configurations
6. **Security**: JWT, OAuth, claims-based authorization

### Recommended Improvements 🎯
1. **Observability** (High Priority): Add distributed tracing (OpenTelemetry)
2. **Resilience** (High Priority): Implement circuit breakers (Polly)
3. **API Documentation** (Medium): Complete Swagger/OpenAPI docs
4. **Caching** (Medium): Add distributed caching strategy
5. **Frontend Testing** (Medium): Add unit and E2E tests

## 7. License

This project is for educational and demonstration purposes, developed as a university project at Fontys University of Applied Sciences.

## 8. Acknowledgments

This project was built through:
- 📚 Extensive study of Domain-Driven Design (Eric Evans, Vaughn Vernon)
- 📚 Microservices patterns and best practices
- 📚 Event-driven architecture resources
- 📚 Official Microsoft .NET documentation
- 📚 RabbitMQ, EventStoreDB, Neo4j documentation
- 📚 Kubernetes and cloud-native patterns
- 💪 Countless hours of coding, debugging, and learning
- 🎓 Fontys University 6th semester software engineering curriculum

**Note**: This entire codebase was developed **before the advent of generative AI tools** (pre-2023), representing genuine learning and implementation from traditional resources and documentation.

## 9. Contributing

This is a completed educational project demonstrating enterprise software architecture patterns. Feel free to fork and experiment with the patterns and implementations showcased here!
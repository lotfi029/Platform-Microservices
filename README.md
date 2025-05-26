# Platform-Microservices

A .NET 9 microservices platform showcasing independent deployments, asynchronous messaging, and gRPC—fully containerized and Kubernetes-ready behind an NGINX ingress gateway.

## 📦 Tech Stack & Rationale

- **.NET 9 & ASP.NET Core Web API**  
  Leveraging the latest C# features (pattern matching, minimal hosting) for a unified framework hosting both REST and gRPC endpoints.

- **Entity Framework Core**  
  - **In-Memory:** Lightning-fast setup for smoke and integration tests without external dependencies.  
  - **SQL Server 2022:** Production-grade ACID store with rich tooling and enterprise familiarity.

- **RabbitMQ (native client)**  
  A battle-tested message broker offering reliable delivery and flexible routing. Used directly via `RabbitMQ.Client` for simplicity and control.

- **gRPC + REST**  
  - **gRPC:** Strongly-typed, binary-efficient, contract-first RPC—ideal for internal, high-performance service-to-service calls.  
  - **REST:** Universally consumable via browsers and HTTP tools—great for public APIs or quick manual testing.

- **Docker**  
  Containerizes each service for consistent behavior across environments. Images built per service and deployed via Kubernetes.

- **Ingress NGINX**  
  Acts as an API gateway: SSL termination, path-based routing (`/platforms` → PlatformService, `/api/c/platforms` → CommandsService), with future auth or rate-limit capabilities.

- **Kubernetes**  
  YAML manifests under `/k8s` define Deployments, Services, PVCs, Secrets, and Ingress—mirroring production best practices for scaling, rolling updates, and service discovery.

## 🗂 Project Structure

├── CommandsService/
│ ├── Controllers/ # REST & gRPC endpoints
│ ├── Data/ # EF Core DbContext & migrations
│ ├── Dtos/ # Request/response models
│ ├── Protos/ # .proto definitions
│ └── Dockerfile
│
├── PlatformService/
│ ├── Controllers/ # REST & gRPC endpoints
│ ├── Data/ # EF Core DbContext & migrations
│ ├── Protos/ # .proto definitions
│ └── Dockerfile
│
├── k8s/ # Kubernetes manifests (Deployments, Services, PVCs, Secrets, Ingress)
├── README.md

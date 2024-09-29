# ContentRelay

Content Relay is a Dapr application that aggregates data from various sources, and expose Metadata through an interface.

Consists of the following components:

- **ContentRelay.MAM**: The application that aggregates data from various sources.
- **ContentRelay.CDS**: The application that exposes metadata, content, and assets.

"MAM" stands for "Metadata and Asset Management" and "CDS" stands for "Content Delivery Service".

## Prerequisites

- Docker
- Docker Compose
- Dapr CLI

## Getting Started

1. Clone the repository
2. Run `dapr -v` to check if Dapr is installed
3. Run `docker-compose up --build`

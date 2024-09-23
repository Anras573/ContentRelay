# ContentRelay
Aggregates data from various sources, and expose them through an interface.

## Prerequisites

- Docker
- Docker Compose
- Dapr CLI

## Getting Started

1. Clone the repository
2. Run `dapr init`
3. Run `docker-compose up -d`
4. Run `dapr run --app-id contentrelay --app-port 5000 --port 3500 python app.py`


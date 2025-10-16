# 🔭 Observability TechTalk Repository

A comprehensive hands-on TechTalk environment for learning modern observability practices using OpenTelemetry, OpenSearch, and ASP.NET Core.

## 📋 Overview

This repository provides a complete observability stack for TechTalk purposes, demonstrating how to instrument applications, collect telemetry data (traces, metrics, and logs), and visualize them using industry-standard open-source tools.

## 🏗️ Architecture
```
┌─────────────────┐
│ ASP.NET Core │
│ Application │──► Logs, Traces, Metrics
└────────┬────────┘
│ OTLP (gRPC)
▼
┌─────────────────┐
│ OpenTelemetry │
│ Collector │──► Process & Route
└────────┬────────┘
│
▼
┌─────────────────┐
│ Data Prepper │──► Transform & Enrich
└────────┬────────┘
│
▼
┌─────────────────┐
│ OpenSearch │──► Store & Index
└────────┬────────┘
│
▼
┌─────────────────┐
│ OpenSearch │──► Visualize & Analyze
│ Dashboards │
└─────────────────┘
```


## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- 8GB RAM minimum (recommended for all containers)

### Setup Instructions

1. **Clone the repository**

```bash
git clone
cd Observability
```

2. **Start the observability stack**

```bash
docker-compose up -d
```

3. **Verify services are running**

```bash
docker-compose ps
```

4. **Run the ASP.NET Core application**
```bash
cd src/Observability
dotnet run
```
5. **Access the services**
   - Application: `http://localhost:5000` (or check console output)
   - Swagger UI: `http://localhost:5000/swagger`
   - OpenSearch Dashboards: `http://localhost:5601`
     - Username: `admin`
     - Password: `PleaseChangeThisPassword2025!`
   - OpenTelemetry Collector Metrics: `http://localhost:8888/metrics`

## 📦 Technology Stack

### Application Layer
- **ASP.NET Core 8.0** - Web application framework
- **OpenTelemetry SDK** - Instrumentation library
- **Serilog** - Structured logging

### Observability Infrastructure
- **OpenTelemetry Collector** - Telemetry data collection and processing
- **Data Prepper** - Data transformation and enrichment
- **OpenSearch** - Search and analytics engine
- **OpenSearch Dashboards** - Data visualization

## 🔌 API Endpoints

### Health Check
```http
GET /api/observability/health
```

Returns application health status.

### Weather Forecast (Demo)

```http
GET /weatherforecast
```

Returns sample weather forecast data with automatic tracing.

### Observability Events
```http
POST /api/observability
Content-Type: application/json

{
    "url": { "href": "https://example.com" },
    "navigator": { "userAgent": "Mozilla/5.0…" },
    "geolocation": {
        "latitude": 37.7749,
        "longitude": -122.4194
    },
    "screen": {
        "width": 1920,
        "height": 1080
    },
    "meta": {
        "clientGeneratedId": "unique-id"
    }
}
```

Logs structured observability events with geolocation data.

## 📊 Features

### Instrumentation
- ✅ Distributed tracing with OpenTelemetry
- ✅ Metrics collection (ASP.NET Core and HTTP client metrics)
- ✅ Structured logging with Serilog
- ✅ OTLP export via gRPC

### Data Collection
- ✅ OTLP receiver (gRPC and HTTP)
- ✅ Automatic trace correlation
- ✅ Geolocation tracking
- ✅ Custom metadata enrichment

### Visualization
- ✅ OpenSearch Dashboards for log analysis
- ✅ Trace visualization
- ✅ Metrics dashboards
- ✅ Geospatial data visualization support

## 🗂️ Project Structure

├── src/
│ └── Observability/
│ ├── Program.cs # Application entry point
│ ├── Observability.csproj # Project configuration
│ ├── appsettings.json # Application settings
│ ├── Models/ # Data models
│ └── Helper/ # Utility classes
├── docker-compose.yml # Observability stack definition
├── otel-config.yaml # OpenTelemetry Collector config
├── data-prepper/
│ ├── pipelines.yaml # Data processing pipelines
│ ├── data-prepper-config.yaml # Data Prepper configuration
└── StaticFrontEnd/ # TechTalk presentation materials
└── index.html # Interactive slides

## 🔧 Configuration

### OpenTelemetry Collector

The collector is configured to:
- Receive OTLP data on ports `4317` (gRPC) and `4318` (HTTP)
- Forward traces and metrics to Data Prepper

Configuration file: `otel-config.yaml`

### Data Prepper Pipelines

Two main pipelines:
1. **Trace Pipeline** - Processes and indexes traces to OpenSearch
2. **Metrics Pipeline** - Processes and indexes metrics to OpenSearch

Configuration file: `data-prepper/pipelines.yaml`

### Application Settings

Customize logging and OpenTelemetry settings in `appsettings.json` and `appsettings.Development.json`.

## 📚 Learning Resources

The repository includes presentation materials in the `StaticFrontEnd` directory covering:
- Observability fundamentals
- OpenTelemetry concepts
- Instrumentation best practices
- OpenSearch integration

Open `StaticFrontEnd/index.html` in a browser to view the slides.

## 🧪 Testing the Setup

1. **Generate some traffic**
```bash
curl http://localhost:5000/weatherforecast
```

2. **Send observability events**

```bash
curl -X POST http://localhost:5000/api/observability \
-H "Content-Type: application/json" \
-d '{"meta": {"clientGeneratedId": "test-123"}}'
```

3. **View in OpenSearch Dashboards**
   - Navigate to `http://localhost:5601`
   - Log in with credentials
   - Go to "Discover" to view logs
   - Go to "Observability" → "Traces" to view distributed traces

## 🛑 Shutdown

Stop all services:

```bash
docker-compose down
```

Remove volumes (⚠️ deletes all data):
```bash
docker-compose down -v
```

## 🐛 Troubleshooting

### Containers won't start
- Ensure ports `4317`, `4318`, `5601`, `8888`, `8889`, `9200`, `2021`, `21890`, `21891` are available
- Check Docker Desktop has sufficient memory allocated (8GB minimum)

### Application can't connect to OpenTelemetry Collector
- Verify the collector is running: `docker-compose ps`
- Check collector logs: `docker logs opentelemetry`

### No data in OpenSearch
- Check Data Prepper health: `curl http://localhost:2021/health`
- Verify pipeline configuration: `docker logs data-prepper`

## 📝 License

This is a TechTalk repository. All free to copy/paste at will! :)

## 🤝 Contributing

This is a TechTalk project. For suggestions or improvements, please contact the repository maintainers.

---

**Happy Learning! 🎓**

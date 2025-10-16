# This file contains documentation for Data Prepper, including setup instructions and configuration details.

## Data Prepper

Data Prepper is a tool designed to process and prepare data for storage in OpenSearch. It allows you to define pipelines that can transform, filter, and route data from various sources to your desired destinations.

### Setup Instructions

1. **Prerequisites**: Ensure you have Docker and Docker Compose installed on your machine.

2. **Configuration**: Modify the `pipelines.yaml` file to define your data processing pipeline. This includes specifying input sources, processing steps, and output destinations.

3. **Running Data Prepper**: Use Docker Compose to start the Data Prepper service along with other services defined in the `docker-compose.yml` file. Run the following command in the root of the project:

   ```bash
   docker-compose up
   ```

### Configuration Details

- **Pipelines**: The `pipelines.yaml` file contains the configuration for your data processing pipelines. You can define multiple pipelines to handle different data sources and processing requirements.

- **Input Sources**: Specify the sources from which Data Prepper will receive data. This could include logs, traces, or metrics from your ASP.NET Core application.

- **Processing Steps**: Define the transformations and filtering that should be applied to the incoming data.

- **Output Destinations**: Configure where the processed data should be sent, such as OpenSearch for storage and querying.

### Additional Resources

- [Data Prepper Documentation](https://opensearch.org/docs/latest/data-prepper/index/)
- [OpenSearch Documentation](https://opensearch.org/docs/latest/)

For any issues or questions, please refer to the official documentation or reach out to the community for support.
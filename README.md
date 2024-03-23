# Azure Functions Deep Dive

## Contact / Links

Mikkel Norsgaard Thomsen <mnt@integration-it.com>

[Webpage](www.integration-it.com)

### Other links

[AIS](https://github.com/xemmel/dti_ais)

[AZ204](https://github.com/xemmel/az204)

[DTI_AIS](https://github.com/xemmel/dti_ais)

## Tools

[Chocolately](https://chocolatey.org/)
[Microsoft webpage](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)

## Azure App Services

[App Service Overview](https://learn.microsoft.com/en-us/azure/app-service/overview)

## What are Azure Functions

Introduction in Microsoft's documentation:
[Azure Functions overview](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview?pivots=programming-language-csharp)

## Azure Functions Plan types

[Hosting options](https://learn.microsoft.com/en-us/azure/azure-functions/functions-scale)

### Consumption plan

Completely serverless. No docker support. Suffers cold start.

### Premium plan

Auto scaling. Use EP plans. VNET. No cold start.

### Dedicated plan

Dedicated App Service plan. Use which ever App Service plan is needed. VNET. No cold start.

### Pricing

[Pricing](https://azure.microsoft.com/en-gb/pricing/details/functions/)

A storage account is also created.

### Network (Premium / Dedicated plans)

[Networking](https://learn.microsoft.com/en-us/azure/azure-functions/functions-networking-options?tabs=azure-portal)

### Which one to use?

The one that is needed.

## Basic Function App

Function App is the main instance which contains individual Functions. Each Function must have a trigger (event to initiate the Function). Can have Bindings which makes handling data easier. Can run either In-process or Isolated. Use Isolated.

[Road map](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/net-on-azure-functions-roadmap-update/ba-p/3619066)

[Road map - Update](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/net-on-azure-functions-august-2023-roadmap-update/ba-p/3910098)

In-Process to Isolated migration:
[Migration guide](https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8)

### Creation

Use either the Portal, Visual Studio, Visual Studio Code, Core Tools (Function App) CLI or something else to create a Function App

Core Tools CLI command:

`func init`

Add a new Function to the newly created Function App by any means.

Core Tools CLI command:

`func new`

### Triggers / Bindings

Triggers are used to run a Function. Must have 1.

Bindings can be input or output and are used to connect to other resources. Are provided as parameters in the Function. You can have multiple Input or Output bindings on a Function.

[Triggers and Bindings](https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings)

Custom Triggers and/or Bindings are possible.

### Security

Each Function is authenticated via the AuthorizationLevel enum in the parameters.

- ADMIN: Master key is required (Do not distribute.)
- ANONYMOUS: No key is required (Do not use.)
- FUNCTION: A key is required on the Function level

IP restrictions are available under Networking.

### Exercises

[How to create a function app - YouTube guide](https://www.youtube.com/watch?v=BEIZKCDElMs)

1. Create a Function App and Function in the Portal (.net 6 / inprocess )
2. Create a Function App and Function locally using Core Tools
3. Create a Function App and Function locally using an IDE
4. Publish a Function App to Azure (If using VS, enable SCM basic )

## Deployment

Use `func azure functionapp publish <FUNCTION_APP_NAME>` powershell command to publish.

Setup Azure DevOps pipelines. This requires access to Azure DevOps.

## Managed Identity

To use MI locally, log in to the correct Azure account with:

`az login --use-device-code`

Enable System Managed Identity under Identity.

Needs a different type of "Connection String". For Blobs use:

|                                                |                                              |
| ---------------------------------------------- | -------------------------------------------- |
| BlobStorageConnectionString\_\_blobServiceUri  | https://functionappst.blob.core.windows.net  |
| BlobStorageConnectionString\_\_queueServiceUri | https://functionappst.queue.core.windows.net |

Example: Blob Trigger required roles

- Trigger: Storage Blob Data Owner and Storage Queue Data Contributor
- Input binding: Storage Blob Data Reader
- Output binding: Storage Blob Data Owner

[Blob Trigger](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-trigger)

**WARNING: DO NOT USE BLOB TRIGGER IN PRODUCTION. IT IS NOT RELIABLE FOR PRODUCTION.**

See warning in above link.

### Exercisees

1. Set up a Function App to use Managed Identities.

## Containerized Function App

Only for Isolated.

Use `func init --docker` powershell command or select in VS.

Build to registry.

`docker build -t deepdivefunctionapp:1.0.0 .`

Run to registry.

`docker run --name deepdivefunctionapp -p 9999:80 deepdivefunctionapp:1.0.0`

Create an Container Registry resource in the portal.

Update the Container Registry with the function app

`az acr build --registry azurecontainerregistry --image dmazurecontainerreg.azurecr.io/azurefunctionsimage:v1.0.0 .`

### Exercises

1. Create a containerizable Function App.
2. Build it in docker and see that it is running.
3. Deploy the Function App using Azure Container Registry.

## Durable Function Apps

[Durable Functions](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview)

Extension of Function App using the Durable Task framework to write stateful functions in a serverless compute environment. Uses _Orchestrator Functions_ to call other Durable Functions in the Function App, e.g. Activity/Entity functions

### Exercises

1. Create a Durable Function where an approver can approve a request. (See example code in repo)

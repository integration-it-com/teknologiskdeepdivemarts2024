targetScope = 'resourceGroup'

param appName string
param location string
param env string
param sku string = 'Standard_LRS'
param kind string = 'StorageV2'

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${appName}${env}st'
  location: location
  sku: {
    name: sku
  }
  kind: kind
}

output name string = storageAccount.name
output id string = storageAccount.id

var storageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
output connectionString string = storageConnectionString

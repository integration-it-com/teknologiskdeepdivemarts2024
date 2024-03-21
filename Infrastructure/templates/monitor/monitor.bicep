targetScope = 'resourceGroup'

param appName string
param env string
param location string

resource monitor 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${appName}-${env}-log'
  location: location
}

var kind = 'web'
resource appInsight 'Microsoft.Insights/components@2020-02-02' = {
  name: '${appName}-${env}-appi'
  location: location
  kind: kind
  properties: {
    Application_Type: kind
    WorkspaceResourceId: monitor.id
  }
}

output instrumentationKey string = appInsight.properties.InstrumentationKey
output monitorId string = monitor.id
output appInsightConnectionString string = appInsight.properties.ConnectionString


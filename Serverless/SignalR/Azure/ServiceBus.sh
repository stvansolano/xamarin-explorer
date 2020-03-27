# sh ServiceBus.sh

# Create a resource group
resourceGroupName="cloud-services"
location="southcentralus"
queueName="MyServiceBus"

az group create --name $resourceGroupName --location $location

# Create a Service Bus messaging namespace with a unique name
namespaceName=myNameSpace$RANDOM
az servicebus namespace create --resource-group $resourceGroupName --name $namespaceName --location $location

# Create a Service Bus queue
az servicebus queue create --resource-group $resourceGroupName --namespace-name $namespaceName --name $queueName
 
# Get the connection string for the namespace
connectionString=$(az servicebus namespace authorization-rule keys list --resource-group $resourceGroupName --namespace-name $namespaceName --name RootManageSharedAccessKey --query primaryConnectionString --output tsv)

# 
# 
# {
#   "id": "/subscriptions/your-subscription-id/resourceGroups/cloud-services",
#   "location": "southcentralus",
#   "managedBy": null,
#   "name": "cloud-services",
#   "properties": {
#     "provisioningState": "Succeeded"
#   },
#   "tags": null,
#   "type": "Microsoft.Resources/resourceGroups"
# }
# {     
#   "createdAt": "2020-03-27T03:39:41.313000+00:00",
#   "id": "/subscriptions/your-subscription-id/resourceGroups/cloud-services/providers/Microsoft.ServiceBus/namespaces/myNameSpace",
#   "location": "South Central US",
#   "metricId": "metric-id:myNameSpace",
#   "name": "myNameSpace",
#   "provisioningState": "Succeeded",
#   "resourceGroup": "cloud-services",
#   "serviceBusEndpoint": "https://myNameSpace.servicebus.windows.net:443/",
#   "sku": {
#     "capacity": null,
#     "name": "Standard",
#     "tier": "Standard"
#   },
#   "tags": {},
#   "type": "Microsoft.ServiceBus/Namespaces",
#   "updatedAt": "2020-03-27T03:40:24.370000+00:00"
# }
# {
#   "accessedAt": "0001-01-01T00:00:00",
#   "autoDeleteOnIdle": "10675199 days, 2:48:05.477581",
#   "countDetails": {
#     "activeMessageCount": 0,
#     "deadLetterMessageCount": 0,
#     "scheduledMessageCount": 0,
#     "transferDeadLetterMessageCount": 0,
#     "transferMessageCount": 0
#   },
#   "createdAt": "2020-03-27T03:40:48.897000+00:00",
#   "deadLetteringOnMessageExpiration": false,
#   "defaultMessageTimeToLive": "10675199 days, 2:48:05.477581",
#   "duplicateDetectionHistoryTimeWindow": "0:10:00",
#   "enableBatchedOperations": true,
#   "enableExpress": false,
#   "enablePartitioning": false,
#   "forwardDeadLetteredMessagesTo": null,
#   "forwardTo": null,
#   "id": "/subscriptions/your-subscription-id/resourceGroups/cloud-services/providers/Microsoft.ServiceBus/namespaces/myNameSpace/queues/MyServiceBus",
#   "location": "South Central US",
#   "lockDuration": "0:01:00",
#   "maxDeliveryCount": 10,
#   "maxSizeInMegabytes": 5120,
#   "messageCount": 0,
#   "name": "MyServiceBus",
#   "requiresDuplicateDetection": false,
#   "requiresSession": false,
#   "resourceGroup": "cloud-services",
#   "sizeInBytes": 0,
#   "status": "Active",
#   "type": "Microsoft.ServiceBus/Namespaces/Queues",
#   "updatedAt": "2020-03-27T03:40:49.093000+00:00"
# }
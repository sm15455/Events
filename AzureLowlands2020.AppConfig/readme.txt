Steps:

create a resource group

Create a Web App named app-appconfig-demo
Publish WebApplication in this web app

Create a keyvault instance named kv-appconfig-demo
Assign the visual studio user the role Key Vault Secret User (needed for managed identity)
Add a secret named SecretValue and set a value

Create a Service Bus named sb-appconfig-demo
Assign the visual studio user the role Azure Service Bus Data Owner (needed for managed identity)
Create a queue named myqueue
add a message to the queue

Create an App configuration instance named cfg-appconfig-demo
Assign the visual studio user the role App Configuration Data Reader (needed for managed identity)
import file cfg-appconfig-demo.json
Add an event subscription and configure to send notification to a webhooks. The webhook must point to the web app url api/updates/ (needed only for testing eventgrid integration)



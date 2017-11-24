# GDAXClient
GDAX C# API Client Library - https://docs.gdax.com/

<h1>How to Use</h1>

<i>Generate your key at https://www.gdax.com/settings/api</i>

````
//create an authenticator with your apiKey, signature and passphrase
var authenticator = new Authenticator("<apiKey>", "<signature>", "<passphrase>");

//create the GDAX client
var gdaxClient = new GDAXClient.GDAXClient(authenticator);

//use one of the services 
var allAccounts = await gdaxClient.accountsService.GetAllAccountsAsync();
````

<h1>What services are provided?</h1>

###### Accounts ######
- GetAllAccountsAsync() - retrieve all accounts
- GetAccountByIdAsync(id) - retrieve account by id

###### Orders ######
- PlaceMarketOrderAsync(orderSide, productType, size) - place market order
- PlaceLimitOrderAsync(orderSide, productType, size, price) - place limit order
- CancelAllOrdersAsync() - cancel all orders
- CancelOrderByIdAsync(string id) - cancel order by id
- GetAllOrdersAsync() - get all orders (only open or un-settled orders)
- GetOrderByIdAsync(string id) - get order by id

<h1>Sandbox Support</h1>

<i>Generate your key at https://public.sandbox.gdax.com/settings/api</i>

````
//create an authenticator with your apiKey, signature and passphrase
var authenticator = new Authenticator("<apiKey>", "<signature>", "<passphrase>");

//create the GDAX client and set the sandbox flag to true
var gdaxClient = new GDAXClient.GDAXClient(authenticator, true);

//use one of the services 
var response = await gdaxClient.OrdersService.PlaceMarketOrderAsync(OrderSide.Buy, ProductType.BtcUsd, 1));
````

<h1>Examples</h1>

###### Place a market order ######

`var response = await gdaxClient.OrdersService.PlaceMarketOrderAsync(OrderSide.Buy, ProductType.BtcUsd, 1));`

###### Place a limit order ######

`var response = await gdaxClient.OrdersService.PlaceLimitOrderAsync(OrderSide.Sell, ProductType.EthUsd, 1, 400.0M));`

###### Cancel all open or un-settled orders ######

`var response = await gdaxClient.OrdersService.CancelAllOrdersAsync();`



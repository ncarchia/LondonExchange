### Introduction
LondonExchange is a Web API for receiving trades from authorised brokers and expose the trades information to them.
### Project Support Features
Users can retrieve:
* A list of all the trades
* A list of all the stocks values
* A single stock value
* A specific list of stocks values
* Persist a stock transaction
* Persist a list of stock transactions
### Installation Guide
* Clone this [LondonExchange](https://github.com/ncarchia/LondonExchange.git) repository.
### Usage
The Web API application:
* Can be opened and run from any software IDE (Visual Studio, Rider, etc.)
* Can be run from the command line by navigating to the project folder and running the dotnet run command and then by pasting the following Url in the browser: https://localhost:7117/swagger/index.html
* Connect to the API using Postman on port 7117.
### API Endpoints
* GET api/LondonExchangeTransactions - To retrieve a list of all the trades
* GET api/LondonExchangeTransactions/AllStocksValue - To retrieve a list of all the stocks values
* GET api/LondonExchangeTransactions/SingleStockValue/{tickerSymbol} - To retrieve a single stock value
* GET api/LondonExchangeTransactions/StocksValue/{tickerSymbols} - To retrieve a specific list of stocks values
* POST api/LondonExchangeTransactions/PersistStock - To persist a single stock
* POST api/LondonExchangeTransactions/PersistStocks - To persist a list of stocks
### Future improvements
This is a MVP that can be improved by:
* adding a caching mechanism to retrieve/access data more quickly
* having a webjob which runs to pull data from the 3rd party platform
* a more performant alternative to continuously polling is having web-hooks to which requests can be sent from the 3rd party to provide a payload containing new trades
* avoiding to fail the full transactions batch if there is any invalid transaction and, instead have a smarter logic which keeps track of 
the failed transactions whilst still persisting the valid ones. I.e. failed transactions can be stored in an azure storage table.
* having full test coverage at unit level to cover all the implementation scenarios. It would be good to have a E2E test.
### Authors
* [NCarchia](https://github.com/ncarchia)
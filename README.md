# Aviva Test

## Solution Projects

|Project|Note|
|--|--|
|AvivaUI|Graphic Interface in BLAZOR C# .NET 10|
|AvivaAPI|REST API in C# .NET 10|
|AvivaLibrary|Common class between UI and API|
|TestAvivaAPI|XUnit test for API|

## General Application Diagram
```mermaid
flowchart LR

 subgraph Aviva
 UI["Aviva UI"] ==> API["Aviva API"]
 API["Aviva API"] .- LIB["Aviva Library"]
 UI["Aviva UI"] .- LIB["Aviva Library"]
 TEST["TEST Aviva API (Xunit)"] .-> API
 API["Aviva API"] ==> DB@{ shape: cyl,label: "Aviva DB(Products, Orders)"}
 end
 subgraph External Resources 
 API["Aviva API"] ==> C@{ shape: docs, label: "Payment Providers (CazaPagos, PagaFacil)"}

 end
 
```

 ## Constants and definitions used in the program

 We should standardize the names in the program, 
 because they are strings and easy to mistakes.

 ### RULES PER PROVIDER

 |Rule for Method|PagaFacil|CazaPagos|
 |--|--|--|
 |CASH|YES|NO|
 |CREDIT|YES|YES|
 |TRANSFER|NO|YES|

 NOTE: The methods are implemented in the API , but if there is not rules for the method
 in provider, then the provider is not selected for that order, 
 and the program select the other provider if it is possible. If there is not any provider with the method, then the program return an error to the user.
 
 ### PAYMENT METHODS IN API

|Aviva(input/output)|PagaFacil(input/output)|CazaPagos(input/output)|Note|
|--|--|--|--|
|x|None|None|Not supported|
|CASH|Cash|x|Cash payment|
|CREDIT|Card|CreditCard|Credit card payment|
|TRANSFER|Note 1|Transfer|Bank transfer payment|

NOTE: Note 1: PagaFacil has defined TRANSFER in the API interface but the API rejected
your call because there is not rules defined in Paga Facil for Transfer


### PROVIDER NAME

|Provider|AvivaName|
|--|--|
|Paga Facil|PAGAFACIL|
|Caza Pagos|CAZAPAGOS|

### ORDER STATUS

|Aviva|PagaFacil|CazaPagos|
|--|--|--|
|NONE|None|None|
|PENDING|Pending|Pending|
|CANCELLED|Cancelled|Cancelled|
|PAID|Paid|Paid|



## Manage Create a Order.....................

```mermaid
sequenceDiagram
participant C as PaymentController
participant BO as Payment BO
participant SE as Service Provider
participant FA as CreateOrderFacade
participant SP as SelectProveedor Service
participant DA as Data
participant PR as Proxy

    C->>  BO: Create Payment Order
    BO ->> SP: Select Proveedr
    SP -->> BO: Proveedor Selected  
    BO ->> SE: Create Payment Order
    SE ->> PR: Create Order in Selected Proxy
    PR -->> SE: Order
    SE ->> FA: Store in Order Database
    FA ->> DA: Store Order
    DA -->> FA: Done
    FA -->> SE: Done 
    SE -->> BO: All Done  
    BO -->> C: OK
```
### Provider Client Module (proxy) Instantiation

The program uses a Proxy Factory (or client factory) to instantiate the
desired client in the program. The client is created based in the Provider
Name.

```mermaid
flowchart LR
CODE["Caller"] =="Provider Name"==> FACTORY["Object Factory"]
FACTORY["Proxy Factory"] ==> INSTANCE["Specific Proxy Object"]

```

### Provider Client Module Structure.

```mermaid
flowchart LR
CODE["Caller"] =="Aviva Order"==> INADAPTER["Create Specific Provider Input Order object"]
subgraph "Provider Custom Module"
INADAPTER =="Provider Input"==> INSTANCE["Connection Logic with the Provider"]
INSTANCE =="Provider Output"==> OUTADAPTER["Return Aviva Output Order"]
end
AVIVASETTING["Program General Settings"] <-.- INSTANCE
OUTADAPTER =="Aviva Response"==>CODERECEIVE["Caller"]

```

#### Integrate a new provider client (proxy)

- Create the provider module
- Create a class with a Factory to move from aviva object to
provider input
- Create a class with a Factory to move from provider output to 
aviva object
- Create the class code for the particular provider
- Create in settings the data for the provider. 
- Register the new service in the `Programs.cs`

Example settings.json:
```json

 "ProveedoresPago": [       
  {         
      "Nombre": "PAGAFACIL",    
      "Url": "https://xxxxxxxxxx.net/",
      "Key": "apikey-xxxxxxxx"
    },
    {
      "Nombre": "CAZAPAGOS",
      "Url": "https://xxxxxxxxxxxxxx.net/",
      "Key": "apikey-xxxxxxxxxx"
    }    
  ]
```
NOTE: By limitation of the test environment, we put the key here,
but in real live must be in a secret local, and encrypted or in a 
vault in production!

### Provider Rules Selection Process

The selection of the provider is based in rules. The winner provider will 
be the provider that is less expensive in the fees and taxes.
In the program fees and taxes are consider together.
The Provider Selector test all the rules per register provider and 
return the name of the Provider that charge less for the specific operation.

#### To enter a new provider Selector:
- Create the module with the rules based in `IProviderRules`
- Declare the module in Program as service to be injected
- The Provider selector automatically get the new module to use it.


```mermaid
flowchart LR
CODE["Aviva Caller"] =="Payment Method and Amount"==> PS["Provider Selector"]
subgraph "Provider Selector"
PS <--"Check All"--> RM@{ shape:docs, label: "Fees Rule (CazaPagos, PagaFacil)"}
end
PS =="Provider Name Selected"==> CODE


```



## Manage Get All Orders

The program goes to the stored tables to get the orders.

```mermaid
sequenceDiagram
    participant C as PaymentController
    participant BO as Payment BO
    participant SE as Service Provider
    participant FA as CreateOrderFacade
    participant DA as (Database)
    C->>BO: Get All Orders
    BO->>SE: Get All Orders
    SE->>FA: Get All Orders
    FA->>DA: Get All Orders
    DA-->>FA: Orders
    FA-->>SE: Orders 
    SE-->>BO: Orders  
    BO-->>C: OK(Orders)
```

## Manage Get one OrderCreated using the ID

Same to get one order created

```mermaid
sequenceDiagram
participant C as PaymentController
participant BO as Payment BO
participant SE as PaymentService
participant FA as CreateOrderFacade
participant DA as Data

    C->>  BO: Get Order by Id
    BO ->> SE: Get Order By Id
    SE ->> FA: Get Order By Id
    FA ->> DA: Get Order By Id
    DA -->> FA: Order?
    FA -->> SE: Order? 
    SE -->> BO: Order?  
    BO -->> C: OK(Order?)
```
## Get Cancel Order

To cancel an order you need to cancel in the provider, and also
modified the value in the Database


```mermaid
sequenceDiagram
participant C as PaymentController
participant BO as Payment BO
participant SP as Service Provider
participant FO as OrderFacade
participant DA as Data
participant PR as Proxy

    C->>  BO: Cancel Order by Id
    BO ->> SP: Cancel Order
    SP ->> FO: Get Order  
    FO ->> DA: Get Order with Id
    DA -->> FO: Order
    FO -->> SP: Order
    SP->> PR: Cancel Order in Provider
    PR -->> SP: Done
    SP ->> FO: Update Order to Cancel
    FO ->> DA: Update Order to Cancel
    DA -->>FO: Done
    FO -->> SP: Done 
    SP -->> BO: All Done  
    BO -->> C: OK(Done)
```
## Get Cancel Order

To pay an order you need to pay in the provider, and also
modified the value in the Database


```mermaid
sequenceDiagram
participant C as PaymentController
participant BO as Payment BO
participant SP as Service Provider
participant FO as OrderFacade
participant DA as Data
participant PR as Proxy

    C->>  BO: Pay Order by Id
    BO ->> SP: Pay Order
    SP ->> FO: Get Order  
    FO ->> DA: Get Order with Id
    DA -->> FO: Order
    FO -->> SP: Order
    SP->> PR: Pay Order in Provider
    PR -->> SP: Done
    SP ->> FO: Update Order to Cancel
    FO ->> DA: Update Order to Cancel
    DA -->>FO: Done
    FO -->> SP: Done 
    SP -->> BO: All Done  
    BO -->> C: OK(Done)
```

## What is next

- More XUnit test
- Logger System to observability
- Health Check
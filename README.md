# aviva Test

## Manage Create a Order
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
## Constants relatives to Order

### METHOD

|Aviva|PagaFacil|CazaPagos|
|--|--|--|
|x|None|None|
|CASH|Cash|x|
|CREDIT|Card|CreditCard|
|TRANSFER|x|Transfer|

### ProviderName

|Provider|AvivaName|
|--|--|
|Paga Facil|PAGAFACIL|
|Caza Pagos|CAZAPAGOS|

### Status

|Aviva|PagaFacil|CazaPagos|
|--|--|--|
|NONE|None|None|
|PENDING|Pending|Pending|
|CANCELLED|Cancelled|Pending|
|PAID|Paid|Cancelled|




## Manage Get All Orders

```mermaid
sequenceDiagram
participant C as PaymentController
participant BO as Payment BO
participant SE as Service Provider
participant FA as CreateOrderFacade
participant DA as Data

    C->>  BO: Get All Orders
    BO ->> SE: Get All Orders
    SE ->> FA: Get All Orders
    FA ->> DA: Get All Orders
    DA -->> FA: Orders
    FA -->> SE: Orders 
    SE -->> BO: Orders  
    BO -->> C: OK(Orders)
```
## Manage Get one OrderCreated using the ID
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
### Note define waht to do if the order is Paid, in this case is assumed that if the order if PAID is not possible Cancel.

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

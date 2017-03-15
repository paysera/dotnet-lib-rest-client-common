### Paysera Common REST Client for .Net

Provides simple RESTfull Client

### Installation

```
PM> Install-Package Paysera.RestClientCommon
```

### Creating ApiClient

Basically you need to create `ApiClient` instance with required configuration. 
Here is sample configuration for `ApiClient` using `ClientCertificateAuthentication`

```csharp
var apiClient = new ApiClient(
    new SecureRestClient(
        baseUri, 
        new ClientCertificateAuthentication(
            certificateFilePath, 
            privateKeyFilePath)), 
    new NoopExceptionMapper());
```

##### Parameters

`ApiClient` accepts following arguments:
* `SecureRestClient` accepts following arguments:
  * `baseUri` - base uri for this `ApiClient`
  * `ClientCertificateAuthentication` accepts following arguments:
    * `certificateFilePath` A certificate at path
    * `privateKeyFilePath` private key at path
    * `password` (optional) will be used to unlock the key
  * `ServerCertificateValidator` (optional) - in case you need to verify server certificate manually, accepts following arguments:
    * `serverCertificateFilePath` - A certificate at path
* `IExceptionMapper` - you can use existing `NoopExceptionMapper`, which throws `System.Exception` or implement your own if you need to throw custom one.


### Usage

This library uses [Newtonsoft.Json](http://www.newtonsoft.com/json/help/html/SerializeObject.htm) for serialization/deserialization between JSON and Objects. 
Please read detailed documentation there.

```csharp
var transfer = new
    {
        amount = new
        {
            amount = 100,
            currency = "EUR"
        },
        beneficiary = new
        {
            type = "bank",
            name = "Name Surname",
            bank_account = new
            {
                iban = "LT12345678901234567890"
            }
        },
        payer = new
        {
            account_number = "EVP1234567890123"
        },
        purpose = new
        {
            details = "Test transfer details"
        }
    };

var result = apiClient.PostAsync<object, object>("/transfers", transferInput).Task.Result;
```
In example above you can see that you don't have to specify exact types for result and payload objects. 
In case you need to use specific types, you just need to specify result and paylod data types: `PostAsync<TResult, TPayload>`.
Regular `ApiClient` methods (`Post*`, `Put*`, `Get*`) returns `ApiTask<T>`, where `T` is your requested Type.

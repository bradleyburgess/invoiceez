# BusinessDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**name** | **string** |  | [default to undefined]
**tagline** | **string** |  | [optional] [default to undefined]
**address** | **string** |  | [default to undefined]
**phone** | **string** |  | [default to undefined]
**email** | **string** |  | [default to undefined]
**website** | **string** |  | [optional] [default to undefined]
**defaultCurrency** | [**CurrencyCode**](CurrencyCode.md) |  | [optional] [default to undefined]
**defaultPaymentInstructions** | **string** |  | [optional] [default to undefined]

## Example

```typescript
import { BusinessDto } from './api';

const instance: BusinessDto = {
    id,
    name,
    tagline,
    address,
    phone,
    email,
    website,
    defaultCurrency,
    defaultPaymentInstructions,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

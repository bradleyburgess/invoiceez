# InvoiceSummaryDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**invoiceNumber** | **string** |  | [optional] [default to undefined]
**invoiceDate** | **string** |  | [optional] [default to undefined]
**paymentStatus** | [**InvoicePaymentStatus**](InvoicePaymentStatus.md) |  | [optional] [default to undefined]
**totalAmount** | **number** |  | [optional] [default to undefined]
**itemCount** | **number** |  | [optional] [default to undefined]
**discountCount** | **number** |  | [optional] [default to undefined]
**currency** | [**CurrencyCode**](CurrencyCode.md) |  | [optional] [default to undefined]
**customerId** | **string** |  | [optional] [default to undefined]
**customerName** | **string** |  | [default to undefined]
**businessId** | **string** |  | [optional] [default to undefined]
**businessName** | **string** |  | [optional] [default to undefined]
**userId** | **string** |  | [optional] [default to undefined]
**createdAtUtc** | **string** |  | [optional] [default to undefined]
**modifiedAtUtc** | **string** |  | [optional] [default to undefined]

## Example

```typescript
import { InvoiceSummaryDto } from './api';

const instance: InvoiceSummaryDto = {
    id,
    invoiceNumber,
    invoiceDate,
    paymentStatus,
    totalAmount,
    itemCount,
    discountCount,
    currency,
    customerId,
    customerName,
    businessId,
    businessName,
    userId,
    createdAtUtc,
    modifiedAtUtc,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

# InvoiceEditDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**invoiceNumber** | **string** |  | [optional] [default to undefined]
**invoiceDate** | **string** |  | [optional] [default to undefined]
**paymentStatus** | [**InvoicePaymentStatus**](InvoicePaymentStatus.md) |  | [optional] [default to undefined]
**totalAmount** | **number** |  | [optional] [default to undefined]
**items** | [**Array&lt;InvoiceItemEditDto&gt;**](InvoiceItemEditDto.md) |  | [optional] [default to undefined]
**discounts** | [**Array&lt;InvoiceDiscountEditDto&gt;**](InvoiceDiscountEditDto.md) |  | [optional] [default to undefined]
**currency** | [**CurrencyCode**](CurrencyCode.md) |  | [optional] [default to undefined]
**paymentInstructions** | **string** |  | [optional] [default to undefined]
**businessId** | **string** |  | [optional] [default to undefined]
**businessName** | **string** |  | [default to undefined]
**businessTagline** | **string** |  | [optional] [default to undefined]
**businessAddress** | **string** |  | [default to undefined]
**businessEmail** | **string** |  | [default to undefined]
**businessPhone** | **string** |  | [default to undefined]
**businessWebsite** | **string** |  | [optional] [default to undefined]
**shouldSaveBusiness** | **boolean** |  | [optional] [default to undefined]
**customerId** | **string** |  | [optional] [default to undefined]
**customerName** | **string** |  | [default to undefined]
**customerAddress** | **string** |  | [default to undefined]
**customerEmail** | **string** |  | [default to undefined]
**customerPhone** | **string** |  | [default to undefined]
**shouldSaveCustomer** | **boolean** |  | [optional] [default to undefined]

## Example

```typescript
import { InvoiceEditDto } from './api';

const instance: InvoiceEditDto = {
    id,
    invoiceNumber,
    invoiceDate,
    paymentStatus,
    totalAmount,
    items,
    discounts,
    currency,
    paymentInstructions,
    businessId,
    businessName,
    businessTagline,
    businessAddress,
    businessEmail,
    businessPhone,
    businessWebsite,
    shouldSaveBusiness,
    customerId,
    customerName,
    customerAddress,
    customerEmail,
    customerPhone,
    shouldSaveCustomer,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

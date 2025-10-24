# InvoiceDetailDtoApiResponse


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**code** | [**ApiResponseCode**](ApiResponseCode.md) |  | [optional] [default to undefined]
**message** | **string** |  | [optional] [default to undefined]
**formErrors** | **{ [key: string]: Array&lt;string&gt;; }** |  | [optional] [default to undefined]
**data** | [**InvoiceDetailDto**](InvoiceDetailDto.md) |  | [optional] [default to undefined]

## Example

```typescript
import { InvoiceDetailDtoApiResponse } from './api';

const instance: InvoiceDetailDtoApiResponse = {
    code,
    message,
    formErrors,
    data,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

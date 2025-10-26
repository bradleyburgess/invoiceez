# InvoicesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createInvoice**](#createinvoice) | **POST** /api/Invoices | Create a new invoice for the authenticated user|
|[**deleteInvoice**](#deleteinvoice) | **DELETE** /api/Invoices/{id} | Delete an invoice by its ID|
|[**duplicateInvoice**](#duplicateinvoice) | **POST** /api/Invoices/duplicate-invoice/{id} | Duplicate an invoice|
|[**generateInvoiceNumber**](#generateinvoicenumber) | **GET** /api/Invoices/generate-invoice-number | Generate an invoice number from a given date|
|[**generateInvoicePdf**](#generateinvoicepdf) | **GET** /api/Invoices/generate-invoice-pdf | Retrieves the PDF rendering of the given invoice|
|[**getBusinessInvoices**](#getbusinessinvoices) | **GET** /api/Invoices/for-business/{id} | Get all invoices for a specific business owned by the authenticated user|
|[**getInvoiceById**](#getinvoicebyid) | **GET** /api/Invoices/{id} | Get a specific invoice by its ID|
|[**getUserInvoices**](#getuserinvoices) | **GET** /api/Invoices/for-me | Get all invoices for the authenticated user|
|[**updateInvoice**](#updateinvoice) | **PUT** /api/Invoices/{id} | Update an existing invoice by its ID|
|[**updateInvoiceStatus**](#updateinvoicestatus) | **PUT** /api/Invoices/update-status/{id} | Update the payment status of an invoice by its ID|
|[**validateInvoiceNumber**](#validateinvoicenumber) | **GET** /api/Invoices/validate-invoice-number | Validate an invoice number, checking for uniqueness to the user|

# **createInvoice**
> InvoiceDetailDtoApiResponse createInvoice()


### Example

```typescript
import {
    InvoicesApi,
    Configuration,
    InvoiceEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let invoiceEditDto: InvoiceEditDto; // (optional)

const { status, data } = await apiInstance.createInvoice(
    invoiceEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **invoiceEditDto** | **InvoiceEditDto**|  | |


### Return type

**InvoiceDetailDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteInvoice**
> EmptyDtoApiResponse deleteInvoice()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteInvoice(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**EmptyDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **duplicateInvoice**
> InvoiceDetailDtoApiResponse duplicateInvoice()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.duplicateInvoice(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**InvoiceDetailDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **generateInvoiceNumber**
> StringApiResponse generateInvoiceNumber()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let invoiceDate: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.generateInvoiceNumber(
    invoiceDate
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **invoiceDate** | [**string**] |  | (optional) defaults to undefined|


### Return type

**StringApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **generateInvoicePdf**
> File generateInvoicePdf()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let invoiceId: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.generateInvoicePdf(
    invoiceId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **invoiceId** | [**string**] |  | (optional) defaults to undefined|


### Return type

**File**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/pdf


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getBusinessInvoices**
> InvoiceListDtoApiResponse getBusinessInvoices()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getBusinessInvoices(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**InvoiceListDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getInvoiceById**
> InvoiceDetailDtoApiResponse getInvoiceById()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getInvoiceById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**InvoiceDetailDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getUserInvoices**
> InvoiceListDtoApiResponse getUserInvoices()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

const { status, data } = await apiInstance.getUserInvoices();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**InvoiceListDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateInvoice**
> InvoiceDetailDtoApiResponse updateInvoice()


### Example

```typescript
import {
    InvoicesApi,
    Configuration,
    InvoiceEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)
let invoiceEditDto: InvoiceEditDto; // (optional)

const { status, data } = await apiInstance.updateInvoice(
    id,
    invoiceEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **invoiceEditDto** | **InvoiceEditDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**InvoiceDetailDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateInvoiceStatus**
> InvoiceSummaryDtoApiResponse updateInvoiceStatus()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)
let body: string; // (optional)

const { status, data } = await apiInstance.updateInvoiceStatus(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **string**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**InvoiceSummaryDtoApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **validateInvoiceNumber**
> BooleanApiResponse validateInvoiceNumber()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let invoiceNumber: string; // (optional) (default to undefined)
let invoiceId: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.validateInvoiceNumber(
    invoiceNumber,
    invoiceId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **invoiceNumber** | [**string**] |  | (optional) defaults to undefined|
| **invoiceId** | [**string**] |  | (optional) defaults to undefined|


### Return type

**BooleanApiResponse**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


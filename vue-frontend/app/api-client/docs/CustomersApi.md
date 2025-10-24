# CustomersApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createCustomer**](#createcustomer) | **POST** /api/Customers | Create a new customer for the authenticated user|
|[**deleteCustomer**](#deletecustomer) | **DELETE** /api/Customers/{id} | Delete a customer by ID for the authenticated user|
|[**getCustomer**](#getcustomer) | **GET** /api/Customers/{id} | Get a customer by ID for the authenticated user|
|[**getCustomers**](#getcustomers) | **GET** /api/Customers | Get all customers for the authenticated user|
|[**updateCustomer**](#updatecustomer) | **PUT** /api/Customers/{id} | Update a customer by ID for the authenticated user|

# **createCustomer**
> CustomerDtoApiResponse createCustomer()


### Example

```typescript
import {
    CustomersApi,
    Configuration,
    CustomerEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new CustomersApi(configuration);

let customerEditDto: CustomerEditDto; // (optional)

const { status, data } = await apiInstance.createCustomer(
    customerEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **customerEditDto** | **CustomerEditDto**|  | |


### Return type

**CustomerDtoApiResponse**

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

# **deleteCustomer**
> EmptyDtoApiResponse deleteCustomer()


### Example

```typescript
import {
    CustomersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CustomersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteCustomer(
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

# **getCustomer**
> CustomerDtoApiResponse getCustomer()


### Example

```typescript
import {
    CustomersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CustomersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getCustomer(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**CustomerDtoApiResponse**

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

# **getCustomers**
> CustomerDtoIEnumerableApiResponse getCustomers()


### Example

```typescript
import {
    CustomersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CustomersApi(configuration);

const { status, data } = await apiInstance.getCustomers();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**CustomerDtoIEnumerableApiResponse**

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

# **updateCustomer**
> CustomerDtoApiResponse updateCustomer()


### Example

```typescript
import {
    CustomersApi,
    Configuration,
    CustomerEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new CustomersApi(configuration);

let id: string; // (default to undefined)
let customerEditDto: CustomerEditDto; // (optional)

const { status, data } = await apiInstance.updateCustomer(
    id,
    customerEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **customerEditDto** | **CustomerEditDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**CustomerDtoApiResponse**

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


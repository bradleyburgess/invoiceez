# BusinessesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createBusiness**](#createbusiness) | **POST** /api/Businesses | Create a new business for the authenticated user|
|[**deleteBusiness**](#deletebusiness) | **DELETE** /api/Businesses/{id} | Delete a business by ID for the authenticated user|
|[**getBusiness**](#getbusiness) | **GET** /api/Businesses/{id} | Get a business by ID for the authenticated user|
|[**getBusinesses**](#getbusinesses) | **GET** /api/Businesses | Get all businesses for the authenticated user|
|[**updateBusiness**](#updatebusiness) | **PUT** /api/Businesses/{id} | Update a business by ID for the authenticated user|

# **createBusiness**
> BusinessDtoApiResponse createBusiness()


### Example

```typescript
import {
    BusinessesApi,
    Configuration,
    BusinessEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new BusinessesApi(configuration);

let businessEditDto: BusinessEditDto; // (optional)

const { status, data } = await apiInstance.createBusiness(
    businessEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **businessEditDto** | **BusinessEditDto**|  | |


### Return type

**BusinessDtoApiResponse**

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

# **deleteBusiness**
> EmptyDtoApiResponse deleteBusiness()


### Example

```typescript
import {
    BusinessesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new BusinessesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteBusiness(
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

# **getBusiness**
> BusinessDtoApiResponse getBusiness()


### Example

```typescript
import {
    BusinessesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new BusinessesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getBusiness(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**BusinessDtoApiResponse**

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

# **getBusinesses**
> BusinessesResponseDtoApiResponse getBusinesses()


### Example

```typescript
import {
    BusinessesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new BusinessesApi(configuration);

const { status, data } = await apiInstance.getBusinesses();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**BusinessesResponseDtoApiResponse**

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

# **updateBusiness**
> BusinessDtoApiResponse updateBusiness()


### Example

```typescript
import {
    BusinessesApi,
    Configuration,
    BusinessEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new BusinessesApi(configuration);

let id: string; // (default to undefined)
let businessEditDto: BusinessEditDto; // (optional)

const { status, data } = await apiInstance.updateBusiness(
    id,
    businessEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **businessEditDto** | **BusinessEditDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**BusinessDtoApiResponse**

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


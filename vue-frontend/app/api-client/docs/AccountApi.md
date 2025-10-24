# AccountApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**changePassword**](#changepassword) | **PUT** /api/Account/change-password | Change current user\&#39;s password|
|[**getAccountInfo**](#getaccountinfo) | **GET** /api/Account | Get current user\&#39;s account information|
|[**updateAccountInfo**](#updateaccountinfo) | **PUT** /api/Account | Update current user\&#39;s account information|

# **changePassword**
> ObjectApiResponse changePassword()


### Example

```typescript
import {
    AccountApi,
    Configuration,
    UserChangePasswordRequestDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let userChangePasswordRequestDto: UserChangePasswordRequestDto; // (optional)

const { status, data } = await apiInstance.changePassword(
    userChangePasswordRequestDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **userChangePasswordRequestDto** | **UserChangePasswordRequestDto**|  | |


### Return type

**ObjectApiResponse**

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

# **getAccountInfo**
> UserDtoApiResponse getAccountInfo()


### Example

```typescript
import {
    AccountApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

const { status, data } = await apiInstance.getAccountInfo();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**UserDtoApiResponse**

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

# **updateAccountInfo**
> UserDtoApiResponse updateAccountInfo()


### Example

```typescript
import {
    AccountApi,
    Configuration,
    UserEditDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let userEditDto: UserEditDto; // (optional)

const { status, data } = await apiInstance.updateAccountInfo(
    userEditDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **userEditDto** | **UserEditDto**|  | |


### Return type

**UserDtoApiResponse**

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


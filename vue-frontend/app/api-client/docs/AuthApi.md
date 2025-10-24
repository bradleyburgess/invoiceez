# AuthApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**checkRegistrationAccepted**](#checkregistrationaccepted) | **GET** /api/Auth/check-registration-accepted | Check if registrations are being accepted|
|[**login**](#login) | **POST** /api/Auth/login | Login a user|
|[**logout**](#logout) | **POST** /api/Auth/logout | Logout the current user|
|[**refreshToken**](#refreshtoken) | **POST** /api/Auth/refresh-token | Refresh access token using refresh token|
|[**register**](#register) | **POST** /api/Auth/register | Register a new user|

# **checkRegistrationAccepted**
> boolean checkRegistrationAccepted()


### Example

```typescript
import {
    AuthApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AuthApi(configuration);

const { status, data } = await apiInstance.checkRegistrationAccepted();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**boolean**

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

# **login**
> AuthResponseDtoApiResponse login()


### Example

```typescript
import {
    AuthApi,
    Configuration,
    AuthLoginRequestDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AuthApi(configuration);

let authLoginRequestDto: AuthLoginRequestDto; // (optional)

const { status, data } = await apiInstance.login(
    authLoginRequestDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **authLoginRequestDto** | **AuthLoginRequestDto**|  | |


### Return type

**AuthResponseDtoApiResponse**

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

# **logout**
> logout()


### Example

```typescript
import {
    AuthApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AuthApi(configuration);

const { status, data } = await apiInstance.logout();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **refreshToken**
> AuthResponseDtoApiResponse refreshToken()


### Example

```typescript
import {
    AuthApi,
    Configuration,
    AuthRefreshRequestDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AuthApi(configuration);

let authRefreshRequestDto: AuthRefreshRequestDto; // (optional)

const { status, data } = await apiInstance.refreshToken(
    authRefreshRequestDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **authRefreshRequestDto** | **AuthRefreshRequestDto**|  | |


### Return type

**AuthResponseDtoApiResponse**

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

# **register**
> AuthResponseDtoApiResponse register()


### Example

```typescript
import {
    AuthApi,
    Configuration,
    AuthRegisterRequestDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AuthApi(configuration);

let authRegisterRequestDto: AuthRegisterRequestDto; // (optional)

const { status, data } = await apiInstance.register(
    authRegisterRequestDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **authRegisterRequestDto** | **AuthRegisterRequestDto**|  | |


### Return type

**AuthResponseDtoApiResponse**

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


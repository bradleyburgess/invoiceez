# CurrencyApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getCurrencies**](#getcurrencies) | **GET** /api/Currency | Get all available currencies|

# **getCurrencies**
> CurrencyListDtoApiResponse getCurrencies()


### Example

```typescript
import {
    CurrencyApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CurrencyApi(configuration);

const { status, data } = await apiInstance.getCurrencies();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**CurrencyListDtoApiResponse**

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


import {
  AccountApi,
  BusinessesApi,
  Configuration,
  CustomersApi,
  InvoicesApi,
} from "~/api-client";
import { getApiClient } from "~/lib/axiosClient";

export function useApi() {
  const baseUrl = useRuntimeConfig().public.apiBaseUrl;

  const config = new Configuration({
    basePath: baseUrl,
    baseOptions: { credentials: "include" },
  });

  const apiClient = getApiClient();

  const invoices = new InvoicesApi(config, undefined, apiClient);
  const businesses = new BusinessesApi(config, undefined, apiClient);
  const account = new AccountApi(config, undefined, apiClient);
  const customers = new CustomersApi(config, undefined, apiClient);

  return {
    invoices,
    businesses,
    account,
    customers,
  };
}

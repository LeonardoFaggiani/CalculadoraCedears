type HttpMethod = "GET" | "POST" | "PUT" | "DELETE" | "PATCH";

interface ApiRequestOptions {
  endpoint: string;
  method: HttpMethod;
  body?: any;
  headers?: Record<string, string>;
}
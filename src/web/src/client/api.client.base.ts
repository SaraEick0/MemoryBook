export class BaseClient {
  protected transformOptions(options: any): Promise<any> {

    // Add authentication if needed
    if (false) {
      let token = 'Bearer ' + sessionStorage.getItem('access_token');
      
      options.headers = options.headers.append('Authorization', token);
      options.withCredentials = true;
    }

    return Promise.resolve(options);
  }

  protected transformResult(
    url: string,
    response: Response,
    processor: (response: Response) => Promise<any>): Promise<any> {
      // Handle unauthorized if needed
      if (response?.status === 401) {
      }

      return processor(response);
    }
}

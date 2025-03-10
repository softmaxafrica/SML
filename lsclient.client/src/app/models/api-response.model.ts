 export interface ApiResponse<T> {
    success: boolean;
    statusCode: number;
    message: string;
    totalCount?: number;
    data: any;
    dataList?: T[];
  }

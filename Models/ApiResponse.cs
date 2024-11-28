using System;

namespace SensorApis.Models {
	public class ApiResponse<T>
	{
		public required string Message { get; set; }
		public required T Data { get; set; }
		public ApiResponse(string message, T data)
		{
			Message = message;
			Data = data;
		}
	}
}
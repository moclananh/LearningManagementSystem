using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.Response;

public class Response
{
    public string Status { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }

    public Response(HttpStatusCode status,string message,object result )
    {
        this.Status = status.ToString();
        this.Message = message;
        this.Result = result;
    }
    public Response(HttpStatusCode status, string message)
    {
        this.Status = status.ToString();
        this.Message = message;
    }
}

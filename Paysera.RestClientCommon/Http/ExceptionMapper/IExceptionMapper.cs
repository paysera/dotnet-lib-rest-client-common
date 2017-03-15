using System;
using RestSharp;

namespace Paysera.RestClientCommon.Http.ExceptionMapper
{
    public interface IExceptionMapper
    {
        Exception MapFromResponse(IRestResponse response);
    }
}

using System;
using System.Net;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Paysera.RestClientCommon.Http.Client;
using Paysera.RestClientCommon.Http.ExceptionMapper;
using Paysera.RestClientCommon.Test.Entity;
using RestSharp;

namespace Paysera.RestClientCommon.Test
{
    [TestFixture]
    public class ApiClientTest
    {
        private ApiClient _apiClient;
        private User _testEntity;

        [SetUp]
        protected void SetUp()
        {
            _testEntity = new User()
            {
                Name = "my-name"
            };

            var restClientMock = new Mock<IRestClient>();
            restClientMock
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<IRestRequest>(),
                    It.IsAny<Action<IRestResponse, RestRequestAsyncHandle>>()))
                .Callback<IRestRequest, Action<IRestResponse, RestRequestAsyncHandle>>((request, callback) =>
                {
                    callback(new RestResponse()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonConvert.SerializeObject(_testEntity),
                        ContentType = "application/json"
                    }, null);
                });

            _apiClient = new ApiClient(
                restClientMock.Object, 
                new NoopExceptionMapper());
        }

        [Test]
        public void TestRequestBody()
        {
            var result = _apiClient.GetAsync<User>("/some-resource").Task.Result;

            Assert.IsInstanceOf(typeof(User), result);
            Assert.AreEqual(_testEntity.Name, result.Name);
        }
    }
}

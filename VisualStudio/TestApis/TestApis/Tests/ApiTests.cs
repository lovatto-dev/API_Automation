
using RestSharp;
using NUnit.Framework;
using Newtonsoft.Json;
using static TestApis.Utils.Utils;
using TestApis.Utils;
using System.Reflection;
using Moq;




[TestFixture]
public class ApiTests
    {

   private RestClient client;

   [SetUp]
   public void Setup()
   {
       string url = Configuration.GetParameter("BaseURL");
       client = new RestClient(url);
   }


   [Test]
   public void TestCreateUser()
   {
        // Definir el endpoint 
        string createUserURL = Configuration.GetParameter("CreateUserURL");
        
        var request = new RestRequest(createUserURL, Method.Post);

        // Definir body
        int id = Utils.GenerateRandomNumber(1,100) ;
        string username = Utils.GenerateRandomText(10);

       var userBody = new List<dynamic>
           {
               new
               {
                   id = id,
                   username = username,
                   firstName = "nombre",
                   lastName = "apellido",
                   email = "algo@mail.com",
                   password = "abcd1234",
                   phone = "1234567890",
                   userStatus = 1
               }
           };
   
       request.AddJsonBody(userBody);
       var response = client.Execute(request);

       // Verificaciones
       Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created);
       Assert.IsNotNull(response.Content);
       Assert.IsTrue(response.Content.Contains("code"));
       Assert.IsTrue(response.Content.Contains("type"));
       Assert.IsTrue(response.Content.Contains("message"));


        var apiResponse = Utils.DeserializeResponse<ApiCreateUserResponse>(response.Content);
        // Validar que los valores de la respuesta sean los esperados
        Assert.AreEqual(200, apiResponse.Code, "El código de respuesta no es el esperado.");
        Assert.AreEqual("unknown", apiResponse.Type, "El tipo de respuesta no es el esperado.");
        Assert.AreEqual("ok", apiResponse.Message, "El mensaje de respuesta no es el esperado.");
    }


    [Test]
    public void TestValidateIncorrectBody()
    {
        // Definir el endpoint 
        string createUserURL = Configuration.GetParameter("CreateUserURL");

        var request = new RestRequest(createUserURL, Method.Post);

        var userBody = new List<dynamic>
           {
               new
               {
                   test = "test"
               }
           };

        request.AddJsonBody(userBody);
        var response = client.Execute(request);

        // Verificaciones
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.IsNotNull(response.Content);
        Assert.IsTrue(response.Content.Contains("code"));
        Assert.IsTrue(response.Content.Contains("type"));
        Assert.IsTrue(response.Content.Contains("message"));


        var apiResponse = Utils.DeserializeResponse<ApiCreateUserResponse>(response.Content);
        // Validar que los valores de la respuesta sean los esperados
        Assert.AreEqual(400, apiResponse.Code);
    }

    [Test]
    public void TestValidateIncorrectFields()
    {
        // Definir el endpoint 
        string createUserURL = Configuration.GetParameter("CreateUserURL");

        var request = new RestRequest(createUserURL, Method.Post);


        var userBody = new List<dynamic>
           {
               new
               {
                  test = "test"
               }
           };

        request.AddJsonBody(userBody);
        var response = client.Execute(request);

        // Estas verificaciones no se realizaran ya que el endPoint que uso para la prueba no las realiza
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.IsNotNull(response.Content);
        Assert.IsTrue(response.Content.Contains("id"));
        Assert.IsTrue(response.Content.Contains("username"));
        Assert.IsTrue(response.Content.Contains("firstName"));
        Assert.IsTrue(response.Content.Contains("lastname"));
        Assert.IsTrue(response.Content.Contains("email"));
        Assert.IsTrue(response.Content.Contains("password"));
        Assert.IsTrue(response.Content.Contains("phone"));
        Assert.IsTrue(response.Content.Contains("userStatus"));


        var apiResponse = Utils.DeserializeResponse<ApiCreateUserBadResponse>(response.Content);
        Assert.AreEqual("El campo es obligatorio" , apiResponse.id);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.username);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.firstName);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.lastName);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.email);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.password);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.phone);
        Assert.AreEqual("El campo es obligatorio.", apiResponse.userStatus);
    }

    [Test]
    public void TestValidateRequiredFields()
    {
        // Definir el endpoint 
        string createUserURL = Configuration.GetParameter("CreateUserURL");

        var request = new RestRequest(createUserURL, Method.Post);


        var userBody = new List<dynamic>
           {
               new
               {
                   id = "AAA",
                   username = 123,
                   firstName = 123,
                   lastName = 123,
                   email = "algomail.com",
                   password = 123,
                   phone = "AAA",
                   userStatus = 1
               }
           };

        request.AddJsonBody(userBody);
        var response = client.Execute(request);

        // Estas verificaciones no se realizaran ya que el endPoint que uso para la prueba no las realiza
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.IsNotNull(response.Content);
        Assert.IsTrue(response.Content.Contains("id"));
        Assert.IsTrue(response.Content.Contains("username"));
        Assert.IsTrue(response.Content.Contains("firstName"));
        Assert.IsTrue(response.Content.Contains("lastname"));
        Assert.IsTrue(response.Content.Contains("email"));
        Assert.IsTrue(response.Content.Contains("password"));
        Assert.IsTrue(response.Content.Contains("phone"));
        Assert.IsTrue(response.Content.Contains("userStatus"));


        var apiResponse = Utils.DeserializeResponse<ApiCreateUserBadResponse>(response.Content);
        Assert.AreEqual("El id debe ser un número positivo.", apiResponse.id);
        Assert.AreEqual("El campo username debe ser un string.", apiResponse.username);
        Assert.AreEqual("El campo firstName debe ser un string.", apiResponse.firstName);
        Assert.AreEqual("El campo lastName debe ser un string.", apiResponse.lastName);
        Assert.AreEqual("El formato del email es incorrecto.", apiResponse.email);
        Assert.AreEqual("El campo password debe ser un string.", apiResponse.password);
        Assert.AreEqual("El phone debe ser un número positivo.", apiResponse.phone);
        Assert.AreEqual("El campo userStatus debe ser un string.", apiResponse.userStatus);
    }


    [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
}

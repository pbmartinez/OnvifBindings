using ServiceReference1;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //var wsBinding = new WSHttpBinding()
            //{
            //    Security = new WSHttpSecurity()
            //    {
            //        Message = new()
            //        {
            //            ClientCredentialType = MessageCredentialType.UserName 
            //        },
            //        Mode = SecurityMode.
            //    },
                
                
            //};
            var wsBinding = new BasicHttpBinding()
            {
                Security = new BasicHttpSecurity()
                {
                    Message = new()
                    {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName 
                    },
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Digest
                    }
                },                
            };
            var myEndpoint = new EndpointAddress("http://sdkgcore.geutebrueck.com:6203/onvif/device_service");
            

            //Add credentials 
            var loginCredentials = new ClientCredentials();
            loginCredentials.UserName.UserName = "root";
            loginCredentials.UserName.Password = "mySuperPass556";
            var myChannelFactory = new ChannelFactory<Device>(wsBinding, myEndpoint);
            var defaultCredentials = myChannelFactory.Endpoint.EndpointBehaviors.FirstOrDefault(a => a.GetType() == typeof(ClientCredentials));
            myChannelFactory.Endpoint.EndpointBehaviors.Remove(defaultCredentials); //remove default ones
            myChannelFactory.Endpoint.EndpointBehaviors.Add(loginCredentials); //add required ones

            Device client = myChannelFactory.CreateChannel();            
            var response = await client.GetDeviceInformationAsync(new GetDeviceInformationRequest());
            Console.Read();
        }
    }
}
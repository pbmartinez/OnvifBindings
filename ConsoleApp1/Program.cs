using ServiceReference1;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace ConsoleApp1
{
    internal class Program
    {
        /// <summary>
        /// Creates the Binding for connection
        /// </summary>
        /// <returns></returns>
        static Binding CreateBinding()
        {
            var binding = new CustomBinding();
            var textBindingElement = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)                
            };
            var httpBindingElement = new HttpTransportBindingElement
            {
                AllowCookies = true,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,                
                AuthenticationScheme = AuthenticationSchemes.Digest,
                
            };
            binding.Elements.Add(textBindingElement);
            binding.Elements.Add(httpBindingElement);
            return binding;
        }
        /// <summary>
        /// It connects to the camera and gathers the services
        /// </summary>
        /// <returns></returns>
        static async Task ConnectAsync(string uri = "http://sdkgcore.geutebrueck.com:6203/onvif/device_service", 
            string user = "root", string pass = "mySuperPass556")
        {
            var binding = CreateBinding();
            var endpoint = new EndpointAddress(new Uri(uri));
            var device = new DeviceClient(binding, endpoint);
            device.ClientCredentials.HttpDigest.ClientCredential.UserName = user;
            device.ClientCredentials.HttpDigest.ClientCredential.Password = pass;
            var foo = await device.GetSystemDateAndTimeAsync();
            Console.WriteLine($"{foo.UTCDateTime.Date.Year} received from camera {uri}");
            try
            {
                //var services = await device.GetServicesAsync(true);
                //var users = await device.GetUsersAsync();
                var req = new User[] {
                    new User(){
                        Username= "regular",
                        Password="mySuperPass789",
                        UserLevel=UserLevel.Administrator
                    }
                };
                //var response = await device.CreateUsersAsync(req);
                var response = await device.SetUserAsync(req);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            await ConnectAsync(uri: "http://sdkgcore.geutebrueck.com:6201/onvif/device_service");
            await ConnectAsync(uri: "http://sdkgcore.geutebrueck.com:6202/onvif/device_service");
            await ConnectAsync(uri:"http://sdkgcore.geutebrueck.com:6203/onvif/device_service");
        }
    }
}
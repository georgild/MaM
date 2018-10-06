using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Transcoder.Contracts;
using Transcoder.Implementations;
using MetadataExtractor.Contracts;
using MetadataExtractor.Implementations;

namespace WorkerServices.SiloServerGeneral {
    class Program {

        private static readonly double EXIT_TIMEOUT = 5000;

        private static readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);

        public static int Main(string[] args) {
            // Register sigterm event handler. Don't forget to import System.Runtime.Loader!
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += SigTermEventHandler;

            // Register sigint event handler
            Console.CancelKeyPress += CancelHandler;

            Console.Title = @"Poisson Silo Server";

            return MainInternal(args).Result;
        }

        private static void SigTermEventHandler(System.Runtime.Loader.AssemblyLoadContext obj) {

        }

        private static void CancelHandler(object sender, ConsoleCancelEventArgs e) {

            _exitEvent.Set();
        }

        private static async Task<int> MainInternal(string[] args) {

            int result = 0;

            try {
                // Server will be able to load any referenced (as a project) Grain automatically.
                // If it does not, then most likely some Orleans package is missing.
                ISiloHost siloHost = await StartSilo();

                _exitEvent.WaitOne();

                CancellationTokenSource exit =
                    new CancellationTokenSource(TimeSpan.FromMilliseconds(EXIT_TIMEOUT));

                // TODO Seems that silo host is inclinable to hang on 'StopAsync'.
                //      Needs to be investigated.
                try {
                    // When the cancelation token fires the silo is expected to exit ungracefully.
                    await siloHost.StopAsync(exit.Token);
                }
                catch (Exception e) {
                    result = -1;
                }


            }
            catch (Exception e) {
                result = -1;
            }

            return result;
        }

        private static async Task<ISiloHost> StartSilo() {

            ISiloHostBuilder siloHostBuilder =
                new SiloHostBuilder()
#if MSOPERSISTED
#warning "Building with MySQL persistence ..."

                    // Persistence storage using MySQL is initialized here.
                    // However It is not used atm as it doesn't work properly.
                    .AddAdoNetGrainStorage(
                        "poissonmysqlstore",
                        p => {
                            p.ConnectionString = @"server=127.0.0.1;uid=root;pwd=playboxM#;database=tests";
                            p.Invariant = @"MySql.Data.MySqlClient";
                            p.UseJsonFormat = true;
                        })
                    // Very basic configuration. We are going to use Consul cluster in real life.
#else
#warning "Building SiloServer with NO (MySQL) persistence ..."
#endif // MSOPERSISTED
                    .UseLocalhostClustering()
                    .UseServiceProviderFactory(p => {
                        //p.AddDbContextPool<Context>(
                        //    options =>
                        //        options.UseMySql(
                        //             ConfigUtils.ConfigurationProvider
                        //            .GetDefaultConfig()
                        //            .GetSection("ConnectionStrings").GetValue<string>("MainDB")
                        //        ));
                        p.AddTransient<ITranscoder, FFMPEGTranscoder>();
                        p.AddTransient<IMetadataExtractor, DefaultMetadataExtractor>();

                        return p.BuildServiceProvider();
                    })
                    .Configure<EndpointOptions>(p => p.AdvertisedIPAddress = IPAddress.Loopback);
            ISiloHost siloHost = siloHostBuilder.Build();
            await siloHost.StartAsync();
            return siloHost;
        }
    }
}

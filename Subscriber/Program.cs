using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Persistence;

namespace Subscriber
{
    class Program
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=kgdnet;Integrated Security=True";

        static void Main(string[] args)
        {
            var hibernateConfig = new Configuration();
            hibernateConfig.DataBaseIntegration(x =>
            {
                x.ConnectionString = ConnectionString;
                x.Dialect<MsSql2012Dialect>();
            });
            var mapper = new ModelMapper();
            mapper.AddMapping<OrderMap>();
            hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            new SchemaExport(hibernateConfig).Execute(false, true, false);

            var sessionFactory = hibernateConfig.BuildSessionFactory();

            var busConfig = new BusConfiguration();
            busConfig.UsePersistence<NHibernatePersistence>().ConnectionString(ConnectionString);
            busConfig.UseTransport<SqlServerTransport>().ConnectionString(ConnectionString);
            busConfig.RegisterComponents(c => c.RegisterSingleton(sessionFactory));

            using (var bus = Bus.Create(busConfig).Start())
            {
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
        }
    }
}

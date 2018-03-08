/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using BaseEAM.Core;
using BaseEAM.Core.Caching;
using BaseEAM.Core.Configuration;
using BaseEAM.Core.Data;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Core.Infrastructure.DependencyManagement;
using BaseEAM.Core.Fakes;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Framework.UI;
using System.Configuration;
using BaseEAM.Services.Pdf;

namespace BaseEAM.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff  
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //controllers
            var foundAssemblies = typeFinder.GetAssemblies().ToArray();
            builder.RegisterControllers(foundAssemblies);

            //data layer
            string dataConnectionString = ConfigurationManager.ConnectionStrings["BaseEAM"].ConnectionString;
            builder.Register<IDbContext>(c => new BaseEamObjectContext("BaseEAM"))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DapperRepository<>)).As(typeof(IDapperRepository<>)).InstancePerLifetimeScope();
            builder.Register(c => new DapperContext(dataConnectionString)).InstancePerLifetimeScope();

            builder.Register(c => new QuartzScheduler(ConfigurationManager.AppSettings["QuartzServer"], 
                Convert.ToInt32(ConfigurationManager.AppSettings["QuartzPort"]), 
                ConfigurationManager.AppSettings["QuartzScheduler"])).SingleInstance();

            //cache manager
            bool redisCachingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["RedisCachingEnabled"]);
            if (redisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("baseeam_cache_static").InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("baseeam_cache_static").SingleInstance();
            }
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("baseeam_cache_per_request").InstancePerLifetimeScope();

            builder.RegisterType<WkHtmlToPdfConverter>().As<IPdfConverter>().InstancePerLifetimeScope();

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //services
            builder.RegisterType<ContractService>().As<IContractService>().InstancePerLifetimeScope();
            builder.RegisterType<AuditEntityConfigurationService>().As<IAuditEntityConfigurationService>().InstancePerLifetimeScope();
            builder.RegisterType<AssignmentService>().As<IAssignmentService>().InstancePerLifetimeScope();
            builder.RegisterType<AttachmentService>().As<IAttachmentService>().InstancePerLifetimeScope();
            builder.RegisterType<ImportProfileService>().As<IImportProfileService>().InstancePerLifetimeScope();
            builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
            builder.RegisterType<PreventiveMaintenanceService>().As<IPreventiveMaintenanceService>().InstancePerLifetimeScope();
            builder.RegisterType<VisualService>().As<IVisualService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceRequestService>().As<IServiceRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskGroupService>().As<ITaskGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<AuditTrailService>().As<IAuditTrailService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkOrderService>().As<IWorkOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<CodeService>().As<ICodeService>().InstancePerLifetimeScope();
            builder.RegisterType<FilterService>().As<IFilterService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<PhysicalCountService>().As<IPhysicalCountService>().InstancePerLifetimeScope();
            builder.RegisterType<AssignmentGroupService>().As<IAssignmentGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowBaseService>().As<IWorkflowBaseService>().InstancePerLifetimeScope();
            builder.RegisterType<ReturnService>().As<IReturnService>().InstancePerLifetimeScope();
            builder.RegisterType<TransferService>().As<ITransferService>().InstancePerLifetimeScope();
            builder.RegisterType<AdjustService>().As<IAdjustService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceItemService>().As<IServiceItemService>().InstancePerLifetimeScope();
            builder.RegisterType<IssueService>().As<IIssueService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
            builder.RegisterType<TechnicianService>().As<ITechnicianService>().InstancePerLifetimeScope();
            builder.RegisterType<ReceiptService>().As<IReceiptService>().InstancePerLifetimeScope();
            builder.RegisterType<CraftService>().As<ICraftService>().InstancePerLifetimeScope();
            builder.RegisterType<ShiftService>().As<IShiftService>().InstancePerLifetimeScope();
            builder.RegisterType<CalendarService>().As<ICalendarService>().InstancePerLifetimeScope();
            builder.RegisterType<AutoNumberService>().As<IAutoNumberService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<AssetService>().As<IAssetService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();
            builder.RegisterType<ItemService>().As<IItemService>().InstancePerLifetimeScope();
            builder.RegisterType<ItemGroupService>().As<IItemGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowDefinitionService>().As<IWorkflowDefinitionService>().InstancePerLifetimeScope();
            builder.RegisterType<EntityAttributeService>().As<IEntityAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AttributeService>().As<IAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<MeterGroupService>().As<IMeterGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<MeterService>().As<IMeterService>().InstancePerLifetimeScope();
            builder.RegisterType<LocationService>().As<ILocationService>().InstancePerLifetimeScope();
            builder.RegisterType<SiteService>().As<ISiteService>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityGroupService>().As<ISecurityGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganizationService>().As<IOrganizationService>().InstancePerLifetimeScope();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerLifetimeScope();
            builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<EcbExchangeRateProvider>().As<IExchangeRateProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<ValueItemService>().As<IValueItemService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfMeasureService>().As<IUnitOfMeasureService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitConversionService>().As<IUnitConversionService>().InstancePerLifetimeScope();
            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<PermissionService>().As<IPermissionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("baseeam_cache_static"))
                .InstancePerLifetimeScope();

            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("baseeam_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());

            //pass MemoryCacheManager as cacheManager (cache locales between requests)
            builder.RegisterType<LocalizationService>().As<ILocalizationService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("baseeam_cache_static"))
                .InstancePerLifetimeScope();

            builder.RegisterType<LanguageService>().As<ILanguageService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("baseeam_cache_static"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();

            builder.RegisterType<UserActivityService>().As<IUserActivityService>().InstancePerLifetimeScope();

            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>();
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}

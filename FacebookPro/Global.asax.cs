using FacebookPro.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
/*Global.asax file contain event handlers (M) */
namespace FacebookPro
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          
            /*implement Dependency injection :technique by which i can create loosely coupled application -->
             * ???? ??????? ???? ??????? ?? ????? ?? ???? ????? ??? ?????? ?? ??????? ??? ?????? interface ????? ?? ?? ?? 
             * simple injection open source dependency injection library for .net (M) */
           /*Simple injector,s main type  is the Container class , 
            * an instance of container is used to register mappings between each abstraction and its corresponding implementation   */
            var container = new Container();/*instance */

            container.Register<ISecurityServices, SecurityServices>();
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}

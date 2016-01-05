using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;

namespace x_nova_template.Service
{
    public class NinjectControllerFactory:DefaultControllerFactory

    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory() {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        
       
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings() {
            ninjectKernel.Bind<IProductRepository>().To<ProductRepository>();
            ninjectKernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            ninjectKernel.Bind<IMenuRepository>().To<MenuRepository>();
            ninjectKernel.Bind<IConfigRepository>().To<ConfigRepository>();
            ninjectKernel.Bind<IPostRepository>().To<PostRepository>();
            ninjectKernel.Bind<IPhotoGallery>().To<PhotoGalleryRepository>();
            ninjectKernel.Bind<IOrderRepository>().To<OrderRepository>();
            ninjectKernel.Bind<IOrderStatusRepository>().To<OrderStatusRepository>();
            ninjectKernel.Bind<IOrderItemRepository>().To<OrderItemRepository>();
            ninjectKernel.Bind<IOrderProcessor>().To<OrderProcessor>();
            ninjectKernel.Bind<IUserRepository>().To<UsersRepository>();
            ninjectKernel.Bind<IEmailSender>().To<EmailSender>();
            ninjectKernel.Bind<ISliderRepository>().To<SliderRepository>();
            ninjectKernel.Bind<IStaticSectionRepository>().To<StaticSectionRepository>();

        }
    }
}
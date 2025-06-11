using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Utilites;

namespace MyBlog.Services.ServicesImpl
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class ViewServiceImpl
    {
        private IUnitOfWork<BlogDbContext> _unitOfWork;
        public ViewServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}

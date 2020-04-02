using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;

namespace Repository
{
    public static class Module
    {
        public static void Init()
        {
            IoC.Manager.Register<DbContext,Context>();
            IoC.Manager.Register<IAnalitics, AnalyticsMongoRepository>();
            IoC.Manager.Register<IUser, UserRepository>();
        }
    }
}
using System;
using System.Collections.Generic;



namespace IoC
{
    public static class Manager
    {
        private static Dictionary<Type,Type> conteiner= new Dictionary<Type,Type>();
        public static Dictionary<Type,Type> GetContainers()
        {
            return conteiner;
        }
        
        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            conteiner[typeof(TInterface)] = typeof(TImplementation);
        }
        
    }
}
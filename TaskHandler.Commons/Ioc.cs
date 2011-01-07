using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;

namespace TaskHandler.Commons
{
    /// <summary>
    /// Naming convention is used:
    /// Names of the interfaces for the objects should be id parameters in the configuration
    /// This simplifies initialization, otherwise lookup by type is performed
    /// </summary>
    public static class IoC
    {
        private static IApplicationContext container;
        private static readonly object LocalContainerKey = new object();

        [ThreadStatic]
        private static Dictionary<object, object> Local = new Dictionary<object, object>();

        public static void Initialize(IApplicationContext springContainer)
        {
            GlobalContainer = springContainer;
        }

        /// <summary>
        /// Tries to resolve the component, but return default value
        /// instead of throwing if it is not there.
        /// Useful for optional dependencies.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T TryResolve<T>()
        {
            try
            {
                var t = Resolve<T>();

                return t;
            }
            catch(Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// aratnikov 30.10.2009 
        /// Will try to resolve by using naming convention first,
        /// then by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            T obj = default(T);
            
            try
            {
                obj = (T) Resolve(typeof (T).Name, typeof (T), null);

                if(obj == null)
                {
                    T[] all = ResolveAll<T>();

                    if (all == null || all.Length == 0)
                    {
                        throw new TypeResolveException("No objects of type {0} registered", typeof(T).FullName);
                    }
                    else
                    {
                        if (all.Length > 1)
                        {
                            throw new TypeResolveDuplicateObjects("More than one object of this type {0} found. Please use ResolveAll method to get multiple results.", typeof(T).FullName);
                        }
                        else
                        {
                            obj = all[0];
                        }
                    }
                }
            }
            catch(IocException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new TypeResolveException(e, "Error while trying to resolve object {0}", typeof(T).FullName);
            }

            return obj;
        }

        /// <summary>
        /// Only available for the name convention objects!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Resolve<T>(object[] parameters)
        {
            object o = Resolve(typeof(T).Name, typeof(T), parameters);
 
            return (T)o;
        }

        private static object Resolve(string name, Type type, object[] parameters)
        {
            try
            {
                object o = Container.ContainsObject(name) ? Container.GetObject(name, type, parameters) : null;

                return o;
            }
            catch (Exception e)
            {
                throw new TypeResolveException(e,
                                               "Object with the name {0} was not resolved. Please, ensure all the needed configuration is presented.",
                                               name);
            }
        }

        private static IDictionary ResolveAllByType(Type type)
        {
            try
            {
                IDictionary objectsOfType = Container.GetObjectsOfType(type);

                return objectsOfType;
            }
            catch(Exception e)
            {
                throw new TypeResolveException(e,
                                               "Objects of type {0} were not resolved. Please, ensure all the needed configuration is presented.",
                                               type.FullName);
            }
        }

        public static IApplicationContext Container
        {
            get
            {
                IApplicationContext result = LocalContainer ?? GlobalContainer;
                if (result == null)
                {
                    // added initialization from the xmlcontext
                    GlobalContainer = ContextRegistry.GetContext("default");
                    result = GlobalContainer;
                }
                //throw new InvalidOperationException(
                //    "The container has not been initialized! Please call IoC.Initialize(container) before using it.");
                return result;
            }
        }

        private static IApplicationContext LocalContainer
        {
            get
            {
                if (LocalContainerStack.Count == 0)
                    return null;
                return LocalContainerStack.Peek();
            }
        }

        private static Stack<IApplicationContext> LocalContainerStack
        {
            get
            {
                object stack;

                Local.TryGetValue(LocalContainerKey, out stack);

                if (stack == null)
                {
                    Local[LocalContainerKey] =  new Stack<IApplicationContext>();
                }

                return (Stack<IApplicationContext>)Local[LocalContainerKey];
            }
        }

        public static bool IsInitialized
        {
            get { return GlobalContainer != null; }
        }

        internal static IApplicationContext GlobalContainer
        {
            get { return container; }
            set { container = value; }
        }

        ///// <summary>
        ///// This allows you to override the global container locally
        ///// Useful for scenarios where you are replacing the global container
        ///// but needs to do some initializing that relies on it.
        ///// </summary>
        ///// <param name="localContainer"></param>
        ///// <returns></returns>
        //public static IDisposable UseLocalContainer(IApplicationContext localContainer)
        //{
        //    LocalContainerStack.Push(localContainer);
        //    return new DisposableAction(delegate
        //                                    {
        //                                        Reset(localContainer);
        //                                    });
        //}

        public static void Reset(IApplicationContext containerToReset)
        {
            if (containerToReset == null)
                return;
            if (ReferenceEquals(LocalContainer, containerToReset))
            {
                LocalContainerStack.Pop();
                if (LocalContainerStack.Count == 0)
                    Local[LocalContainerKey] = null;
                return;
            }
            if (ReferenceEquals(GlobalContainer, containerToReset))
            {
                GlobalContainer = null;
            }
        }

        public static void Reset()
        {
            IApplicationContext springContainer = LocalContainer ?? GlobalContainer;
            Reset(springContainer);
        }

        public static IDictionary ResolveAll(Type service)
        {
            return Container.GetObjectsOfType(service);
        }

        public static T[] ResolveAll<T>()
        {
            IDictionary objectsOfType = Container.GetObjectsOfType(typeof(T));
            T[] array = new T[objectsOfType.Count];
            objectsOfType.Values.CopyTo(array, 0);

            return array;
        }
    }
}
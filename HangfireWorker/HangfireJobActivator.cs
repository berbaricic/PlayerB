using Hangfire;
using System;
using Unity;

namespace HangfireWorker
{
    public class HangfireJobActivator : JobActivator
    {
        private readonly IUnityContainer hangfireContainer;

        public HangfireJobActivator(IUnityContainer hangfireContainer)
        {
            this.hangfireContainer = hangfireContainer;
            //don't forget to register child dependencies as well
        }


        public override object ActivateJob(Type type)
        {
            return hangfireContainer.Resolve(type);
        }
    }
}

using System;
namespace Adventures.Common.Interfaces
{
	public interface IMvpEventAggregator
	{
        void Send<TSender>(TSender sender, string message)
            where TSender : class;

        void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
            where TSender : class;


        void Subscribe<TSender>(object subscriber, string message,
            Action<TSender> callback) where TSender : class;
    }
}


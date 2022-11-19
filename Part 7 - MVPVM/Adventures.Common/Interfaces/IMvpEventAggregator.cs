namespace Adventures.Common.Interfaces
{
    public interface IMvpEventAggregator
	{
        void Publish<TSender>(TSender sender)
            where TSender : class;

        void Publish<TSender>(TSender sender, string message)
            where TSender : class;

        void Publish<TSender, TArgs>(TSender sender, string message, TArgs args)
            where TSender : class;


        void Subscribe<TSender>(object subscriber,
            Action<TSender> callback) where TSender : class;

        void Subscribe<TSender>(object subscriber, string message,
            Action<TSender> callback) where TSender : class;
    }
}


namespace Adventures.Common.Events
{
    public class SimpleEventAggregator : IMvpEventAggregator
	{
        public void Publish<TSender>(TSender sender)
            where TSender : class
        {
            MessagingCenter.Send<TSender>(sender,nameof(TSender));
        }

        public void Publish<TSender>(TSender sender, string message)
            where TSender : class
        {
            MessagingCenter.Send<TSender>(sender, message);
        }

        public void Publish<TSender,TArgs>(TSender sender,
            string message, TArgs args) where TSender : class
        {
			MessagingCenter.Send<TSender,TArgs>(sender, message, args);
		}


        public void Subscribe<TSender>(object subscriber, 
            Action<TSender> callback) where TSender : class
        {
            MessagingCenter.Subscribe<TSender>(subscriber,
                nameof(TSender), callback);
        }

        public void Subscribe<TSender>(object subscriber, string  message,
			Action<TSender> callback) where TSender : class
        {
			MessagingCenter.Subscribe<TSender>(subscriber, message, callback);
        }
	}
}

namespace RowdyRuff.Core.Common
{
    public class Subscription
    {
        public int Id { get; private set; }
        public string ClientProfileId { get; private set; }
        public string CalendarId { get; private set; }

        public Subscription(string calendarId)
        {
            CalendarId = calendarId;
        }

        private Subscription() { }
    }
}

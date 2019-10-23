namespace EventSourcing.Tests.TestDomain
{
    public class TestAggregateNameChanged
    {
        public long TestAggregateId { get; set; }
        public string PreviousName { get; set; }
        public string CurrentName { get; set; }
    }
}
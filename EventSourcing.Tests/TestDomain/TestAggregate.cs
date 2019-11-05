namespace EventSourcing.Tests.TestDomain
{
    public class TestAggregate : Aggregate
    {
        private long _id;
        private string _name;
        private bool _isRemoved;

        public TestAggregate()
        {
            On<TestAggregateCreated>(e =>
            {
                AggregateId = e.TestAggregateId.ToString();
                _id = e.TestAggregateId;
                _name = e.Name;
            });
            On<TestAggregateNameChanged>(e => _name = e.CurrentName);
            On<TestAggregateRemoved>(e => _isRemoved = true);
        }

        public TestAggregate(long aggregateId, string name)
            :this()
        {
            ApplyChange(new TestAggregateCreated
            {
                TestAggregateId = aggregateId,
                Name = name
            });
        }
        
        public void ChangeName(string name) => ApplyChange(new TestAggregateNameChanged
        {
            TestAggregateId = _id,
            PreviousName = _name,
            CurrentName = name
        });

        public void Remove()
        {
            if (_isRemoved)
                return;
            
            ApplyChange(new TestAggregateRemoved {TestAggregateId = _id});
        }
    }
}
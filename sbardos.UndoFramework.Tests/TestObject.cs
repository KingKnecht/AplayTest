namespace sbardos.UndoFramework.Tests
{

    //public class TestObject2 : IUndoable
    //{
    //     private static int _nextId = 10000;
    //    private bool _booleanValue;
    //    private readonly ITransactionService _transactionManager;
    //    private readonly int _clientId;

    //    public TestObject2(ITransactionService transactionManager, int clientId)
    //    {
    //        _transactionManager = transactionManager;
    //        _clientId = clientId;
    //        Id = ++_nextId;
    //    }

    //    private TestObject2()
    //    {
            
    //    }

    //    public int Id { get; private set; }


    //    public IUndoable UndoClone()
    //    {
    //        return new TestObject2() { Id = Id, BooleanValue = BooleanValue };
    //    }

    //    public bool BooleanValue
    //    {
    //        get { return _booleanValue; }
    //        set
    //        {
    //            if (value == _booleanValue)
    //                return;

    //            _booleanValue = value; 
    //            _transactionManager.InsertAt(new Change(ChangeReason.ObjectUpdate, Id,this.UndoClone()),_clientId);
    //        }
    //    }


    //    protected bool Equals(TestObject2 other)
    //    {
    //        return Id == other.Id && BooleanValue == other.BooleanValue;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (ReferenceEquals(null, obj)) return false;
    //        if (ReferenceEquals(this, obj)) return true;
    //        if (obj.GetType() != this.GetType()) return false;
    //        return Equals((TestObject2)obj);
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            return (Id * 397) ^ BooleanValue.GetHashCode();
    //        }
    //    }
    //}

    public class TestObject : IUndoable
    {
        private static int _nextId = 10000;

        public TestObject()
        {
            Id = ++_nextId;

        }

        public int Id { get;private set; }
        public string Dump()
        {
            return "fill me";
        }


        public IUndoable UndoClone()
        {
            return new TestObject() {Id = Id, BooleanValue = BooleanValue};
        }

        public bool BooleanValue { get; set; }


        protected bool Equals(TestObject other)
        {
            return Id == other.Id && BooleanValue == other.BooleanValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ BooleanValue.GetHashCode();
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace MemberTrack.Common
{
    //This helper class is used to easily initialize a list of Tuples
    //REF: http://stackoverflow.com/questions/8002455/how-to-easily-initialize-a-list-of-tuples
    public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }
    }
}
